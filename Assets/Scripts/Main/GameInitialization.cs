using System;
using Data;
using UnityEngine;

namespace Main
{
    public sealed class GameInitialization : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////
        #region Variables
        
        [SerializeField] private GameController.GameState gameState;
        [SerializeField] internal CoreConfig config;
        [SerializeField] internal GameSettings settings;
        
        private static GameController _gameController;

        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Interface

        public void SwitchState(GameController.GameState newGameState)
        {
            _gameController.SwitchState(newGameState, false);
        }
        
        public void SwitchStateWithReset(GameController.GameState newGameState)
        {
            _gameController.SwitchState(newGameState, true);
        }

        public void ClearContexts()
        {
            _gameController.ClearContexts();
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Implementation
        
        private void Awake()
        {
            if (_gameController == null)
            {
                _gameController = new GameController(this, Contexts.sharedInstance, gameState);
            }
            else
            {
                _gameController.SwitchState(gameState, true);
            }
        }
        
        private void Start()
        {
            _gameController.Initialize();
        }
        
        private void Update()
        {
            _gameController.Execute();
        }
        
        private void LateUpdate()
        {
            _gameController.Cleanup();
        }

        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}
