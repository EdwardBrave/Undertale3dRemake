using System;
using Entitas.Unity;
using Main.Globals;
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
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Interface

        public void ActOpenWindow(InitUiEntity data) => EcsEnvironment.UiEventsEntity.ReplaceOpenWindow(data);

        public void ActClose(InitUiEntity data) => EcsEnvironment.UiEventsEntity.ReplaceCloseWindows(data, false);
        
        public void ActForceClose(InitUiEntity data) => EcsEnvironment.UiEventsEntity.ReplaceCloseWindows(data, true);

        public void ActCloseAllWindows(bool isForce = false) => EcsEnvironment.UiEventsEntity.ReplaceCloseAll(isForce);

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
        
        private void OnEvent(UIButton sender)
        {
            buttonEvents[sender]?.Invoke();
            LinkedEntity?.ReplaceUiEvent(sender);
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}
