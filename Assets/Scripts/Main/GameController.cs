using System;
using Entitas.CodeGeneration.Attributes;
using Main.Features;
using UnityEngine;

namespace Main
{
    [Core, Unique]
    public sealed class GameController
    {
        ////////////////////////////////////////////////////////////////////
        #region Variables
        
        private readonly Contexts _contexts;
        private Feature _systems;

        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Interface
        
        public void ClearContexts()
        {
            if (_systems != null)
            {
                _systems.DeactivateReactiveSystems();
                _systems.TearDown();
            }
            _contexts.input.Reset();
            _contexts.game.Reset();
            _contexts.ui.Reset();
            _systems?.ActivateReactiveSystems();
        }

        public void SwitchState(GameState newState, bool clearContexts = false)
        {
            if (_systems != null)
            {
                _systems.DeactivateReactiveSystems();
                _systems.TearDown();
            }
            
            if (clearContexts)
            {
                _contexts.input.Reset();
                _contexts.game.Reset();
                _contexts.ui.Reset();
            }

            switch (newState)
            {
                case GameState.None:
                {
                    _systems = new Feature();
                    Debug.LogWarning("GameState is None. All systems are disabled.");
                    break;
                }
                case GameState.Travel:
                {
                    _systems = new TravelSystems(_contexts);
                    break;
                }
                case GameState.Battle:
                {
                    _systems = new BattleSystems(_contexts);
                    break;
                }
                case GameState.CutScene:
                {
                    _systems = new CutSceneSystems(_contexts);
                    break;
                }
                case GameState.UI:
                {
                    _systems = new UISystems(_contexts);
                    break;
                }
                case GameState.MainMenu:
                {
                    _systems = new MainMenuSystems(_contexts);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, $"GameState is not implemented");
            }
        }
        
        internal GameController(GameInitialization gameData, Contexts contexts, GameState gameState)
        {
            _contexts = contexts;
            contexts.Reset();
            contexts.core.SetGameController(this);
            contexts.core.SetCoreConfig(gameData.config);
            contexts.core.SetGameSettings(gameData.settings);
            SwitchState(gameState);
        }

        internal void Initialize()
        {
            _systems.Initialize();
        }

        internal void Execute()
        {
            _systems.Execute();
        }

        internal void Cleanup()
        {
            _systems.Cleanup();
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////

        public enum GameState
        {
            None,
            Travel,
            Battle,
            CutScene,
            UI,
            MainMenu
        }
    }
}
