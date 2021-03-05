using Core.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main
{
    public sealed class GameInitialization : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////
        #region Variables
        
        [OnValueChanged("OnStateChanged")]
        [SerializeField] private GameController.GameState gameState;
        [SerializeField] internal CoreConfig config;
        [SerializeField] internal GameSettings settings;
        
        private static GameController _gameController;

        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Interface

        public void SwitchState(GameController.GameState newGameState)
        {
            gameState = newGameState;
            _gameController.SwitchState(newGameState, false);
        }
        
        public void SwitchStateWithReset(GameController.GameState newGameState)
        {
            gameState = newGameState;
            _gameController.SwitchState(newGameState, true);
        }

        public void ClearContexts()
        {
            _gameController.ClearContexts();
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Implementation

        private void OnStateChanged()
        {
            if (Application.isPlaying)
            {
                SwitchState(gameState);
            }
        }
        
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
