using System;
using Entitas.Unity;
using Main.Globals;
using UI.Open;
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

        public void ActCloseSelf(bool isForce = false) => OnEvent(uiEntity => uiEntity.ReplaceClose(isForce));
        
        public void ActOpenWindow(InitUiEntity data) => EcsEnvironment.UiEventsEntity.ReplaceOpenWindow(data);

        public void ActClose(InitUiEntity data) => EcsEnvironment.UiEventsEntity.ReplaceCloseWindows(data, false);
        
        public void ActForceClose(InitUiEntity data) => EcsEnvironment.UiEventsEntity.ReplaceCloseWindows(data, true);

        public void ActCloseAllWindows(bool isForce = false) => EcsEnvironment.UiEventsEntity.ReplaceCloseAll(isForce);
        
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
