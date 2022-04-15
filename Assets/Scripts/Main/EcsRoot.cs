using Data;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Main
{
    [Core, Unique][DisallowMultipleComponent]
    public sealed class EcsRoot : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////
        #region Variables

#if UNITY_EDITOR
        [OnValueChanged(nameof(OnStateChanged))]
#endif
        [Title("Game initialization configs")]
        [SerializeField] 
        private RegisteredGameState gameState;
        [SerializeField] 
        private ChangeGameStateComponent.StateMode stateMode;
        [SerializeField] 
        private GlobalGameConfigs globalGameConfigs;

        [Title("Scene references")] 
        [SerializeField]
        private Transform canvasContainer;

        private RootStateMachine _rootStateMachine;
        private MainGameLogic _mainGameLogic;

        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Interface

        public Transform CanvasContainer => canvasContainer;
        
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
            if (contexts.core.hasEcsRoot)
            {
                if (contexts.core.gameState.type != gameState)
                {
                    contexts.core.gameStateEntity.AddChangeGameState(gameState, stateMode);
                }
                Destroy(this);
            }
            else
            {
                contexts.core.SetEcsRoot(this);
                contexts.core.ecsRootEntity.AddGlobalGameConfigs(globalGameConfigs);
                _mainGameLogic = new MainGameLogic();
                _mainGameLogic.InitSystems(contexts);
                _mainGameLogic.Activate();
                _rootStateMachine = new RootStateMachine(gameState, contexts);
            }
        }
        
        private void Update()
        {
            _mainGameLogic.Execute();
            _rootStateMachine.Execute();
        }
        
        private void LateUpdate()
        {
            _mainGameLogic.Cleanup();
            _rootStateMachine.Cleanup();
        }
        
        private void OnDestroy()
        {
            _rootStateMachine?.TearDown();
            _mainGameLogic?.Deactivate();
            _mainGameLogic?.TearDown();
        }

        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}
