using System;
using Entitas.Unity;
using Main.Globals;
using UnityEngine;

namespace UI.Events
{
    public class UiHandler : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////
        #region Interface
        
        public void ActConfirm() => OnEvent(uiEntity => uiEntity.isConfirm = true);

        public void ActReject() => OnEvent(uiEntity => uiEntity.isReject = true);
        
        public void ActCancel() => OnEvent(uiEntity => uiEntity.isCancel = true);
        
        public void ActPressed(string buttonName) => OnEvent(uiEntity => uiEntity.ReplacePressed(buttonName));

        public void ActCheck(bool isOn) => OnEvent(uiEntity => uiEntity.ReplaceCheck(isOn));

        public void ActCloseSelf() => OnEvent(uiEntity => uiEntity.isClose = true);
        
        public void ActOpenWindow(GameObject prefab) => EcsEnvironment.UiEventsEntity.ReplaceOpenWindow(prefab);

        public void ActClose(GameObject prefab) => EcsEnvironment.UiEventsEntity.ReplaceCloseWindows(prefab);

        public void ActCloseAllWindows() => EcsEnvironment.UiEventsEntity.isCloseAll = true;
        
        //public void ActLoadScene(SceneAsset scene) => OnEvent(UiEventArgs.Type.LoadScene, scene);

        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Implementation

        private void OnEvent(Action<UiEntity> action)
        {
            if (gameObject.GetComponentInParent<EntityLink>()?.entity is UiEntity uiEntity)
            {
                action(uiEntity);
            }
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}
