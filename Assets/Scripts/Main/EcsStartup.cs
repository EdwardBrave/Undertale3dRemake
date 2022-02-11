using Sirenix.OdinInspector;
using UI.Data;
using UnityEngine;

namespace Main
{
    public sealed class EcsStartup : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////
        #region Variables
        
        [OnValueChanged(nameof(OnStateChanged))]
        [SerializeField] private RegisteredGameState gameState;
        [SerializeField] private ChangeGameStateComponent.StateMode stateMode;
        [SerializeField] internal UiConfig uiConfig;

        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Implementation

        private void OnStateChanged()
        {
            if (Application.isPlaying)
            {
                Contexts.sharedInstance.core.gameStateEntity.AddChangeGameState(gameState, stateMode);
            }
        }
        
        private void Awake()
        {
            var ecsRootGameObject = new GameObject(">>> ECS Root <<<");
            var ecsRoot = ecsRootGameObject.AddComponent<EcsRoot>();
            ecsRoot.Init(gameState);
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}
