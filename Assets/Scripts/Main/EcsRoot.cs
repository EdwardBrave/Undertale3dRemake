using Data;
using UnityEngine;
using UnityEngine.Localization.Settings;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Main
{
    [DisallowMultipleComponent]
    public sealed class EcsRoot : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////
        #region Variables
        
        private static EcsRoot _instance;
        
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnStateChanged))]
#endif
        [SerializeField] private RegisteredGameState gameState;
        [SerializeField] private ChangeGameStateComponent.StateMode stateMode;
        [SerializeField] private GlobalGameConfigs globalGameConfigs;
        
        private RootStateMachine _rootStateMachine;

        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Implementation

#if UNITY_EDITOR
        private void OnStateChanged()
        {
            if (Application.isPlaying)
            {
                Contexts.sharedInstance.core.gameStateEntity.AddChangeGameState(gameState, stateMode);
            }
        }
#endif
        
        private void Awake()
        {
            var contexts = Contexts.sharedInstance;
            if (_instance)
            {
                
                if (contexts.core.gameState.type != gameState)
                {
                    contexts.core.gameStateEntity.AddChangeGameState(gameState, stateMode);
                }
                Destroy(this);
            }
            else
            {
                _instance = this;
                InitGlobals(contexts);
                _rootStateMachine = new RootStateMachine(gameState, Contexts.sharedInstance);
            }
        }

        private void InitGlobals(Contexts contexts)
        {
            contexts.Reset();
            var coreEntity = contexts.core.CreateEntity();
            coreEntity.AddLocale(LocalizationSettings.SelectedLocale.Identifier);
            coreEntity.AddGlobalGameConfigs(globalGameConfigs);
        }
        
        private void Update()
        {
            _rootStateMachine.Execute();
        }
        
        private void LateUpdate()
        {
            _rootStateMachine.Cleanup();
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}
