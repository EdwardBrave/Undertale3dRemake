using System;
using Entitas;
using Entitas.Unity;
using Sirenix.OdinInspector;
using Tools;
using Tools.UiListeners;
using UI.Open;
using UnityEngine;
using UnityEngine.Events;
using UIButton = UnityEngine.UI.Button;

namespace UI.Events
{
    public class UiDispatcher : MonoBehaviour
    {
        [Serializable]
        public class ButtonEventsDictionary : UnitySerializedDictionary<UIButton, UnityEvent> { }
        
        ////////////////////////////////////////////////////////////////////
        #region Data
        
        [SerializeField]
        [DictionaryDrawerSettings(IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.Foldout, 
            KeyLabel = "Button", ValueLabel = "Event")]
        private ButtonEventsDictionary buttonEvents = new ButtonEventsDictionary();

        private UiEntity _linkedEntity = null;
        private UiContext _uiContext = null;
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Interface

        public void ActOpenWindow(InitUiEntity prefab)
        {
            var newEntity = _uiContext.CreateEntity();
            newEntity.AddCreateWindow(prefab, _uiContext.mainScreenEntity);
        }
        
        public void ActCloseSelf(bool isForce = false) => LinkedEntity.ReplaceClose(isForce);

        public void ActClose(InitUiEntity example) => CloseViewsWithName(example.name, false);
        
        public void ActForceClose(InitUiEntity example) => CloseViewsWithName(example.name, true);

        public void ActCloseAllWindows(bool isForce = false)
        {
            foreach (var entity in _uiContext.GetEntities(UiMatcher.AllOf(UiMatcher.View).NoneOf(UiMatcher.Container)))
            {
                entity.ReplaceClose(isForce);
            }
        }

        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Unity Events
        
        private void Awake()
        {
            _uiContext = Contexts.sharedInstance.ui;
        }

        private void OnEnable()
        {
            foreach (var button in buttonEvents.Keys)
            {
                button.AddListener(OnEvent);
            }
        }
        
        private void OnDisable()
        {
            foreach (var button in buttonEvents.Keys)
            {
                button.RemoveListener(OnEvent);
            }
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Implementation
        
        private UiEntity LinkedEntity
        {
            get
            {
                if (_linkedEntity == null && gameObject.GetComponentInParent<EntityLink>()?.entity is UiEntity uiEntity)
                {
                    _linkedEntity = uiEntity;
                }
                return _linkedEntity;
            }
        }

        [Button("Refresh dispatch events")]
        private void RefreshDispatchEvents()
        {
            var buttons = GetComponentsInChildren<UIButton>();
            var newButtonEvents = new ButtonEventsDictionary();
            
            foreach (var button in buttons)
            {
                newButtonEvents[button] = buttonEvents.ContainsKey(button) ? buttonEvents[button] : new UnityEvent();
            }
            buttonEvents = newButtonEvents;
        }

        private void OnEvent(UIButton sender)
        {
            buttonEvents[sender]?.Invoke();
            LinkedEntity?.ReplaceUiEvent(sender);
        }

        private void CloseViewsWithName(string viewName, bool isForce = false)
        {
            foreach(var entity in _uiContext.GetEntities(UiMatcher.View))
            {
                if (entity.view.obj.name == viewName)
                {
                    entity.ReplaceClose(isForce);
                }
            }
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}
