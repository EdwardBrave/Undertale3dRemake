using System;
using UnityEngine;

namespace Main
{
    [DisallowMultipleComponent]
    public class EcsRoot: MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////
        #region Data
        
        public static bool IsActive => _instance.enabled;

        private static EcsRoot _instance;
        
        private RootStateMachine _rootStateMachine;
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region UnityEvents Implementation
        
        private void Awake()
        {
            if (_instance)
            {
                throw new Exception("Attempt to instantiate a second time. EcsRoot is already instantiated!");
            }
            
            DontDestroyOnLoad(this);
        }

        public void Init(RegisteredGameState gameState)
        {
            var contexts = Contexts.sharedInstance;
            contexts.Reset();
            contexts.game.isGlobalEvents = true;
            contexts.ui.isGlobalEvents = true;
            contexts.core.isGlobalEvents = true;
            contexts.input.isGlobalEvents = true;
            
            _rootStateMachine = new RootStateMachine(gameState, Contexts.sharedInstance);
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