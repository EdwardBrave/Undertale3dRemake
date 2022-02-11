using System;
using System.Collections.Generic;
using UnityEngine;
using StateMode = Main.ChangeGameStateComponent.StateMode;

namespace Main
{
    public class RootStateMachine
    {
        private readonly Contexts _contexts;
        
        private readonly CoreEntity _gameStateEntity;
        
        private readonly Stack<GameState> _states;
        
        private GameState CurrentState => _states.Count > 0 ? _states.Peek() : null;

        public RootStateMachine(RegisteredGameState initStateTag, Contexts contexts)
        {
            _contexts = contexts;
            _states = new Stack<GameState>();
            _gameStateEntity = _contexts.core.CreateEntity();
            ChangeState(initStateTag);
        }

        public void Execute()
        {
            CurrentState?.Execute();
        }

        public void Cleanup()
        {
            CurrentState?.Cleanup();
            
            if (_gameStateEntity.hasChangeGameState)
            {
                ChangeState(_gameStateEntity.changeGameState.value, _gameStateEntity.changeGameState.mode);
                _gameStateEntity.RemoveChangeGameState();
            }
        }

        private void ChangeState(RegisteredGameState gameState, StateMode mode = StateMode.Main)
        {
            switch (mode)
            {
                case StateMode.Main:
                {
                    TearDown();
                    var newState = GameStatesLookup.GetState(gameState);
                    _states.Push(newState);
                    newState.InitSystems(_contexts);
                    newState.Activate();
                    _gameStateEntity.ReplaceGameState(gameState);
                    break;
                } 
                case StateMode.Additional:
                {
                    CurrentState?.Deactivate();
                    var newState = GameStatesLookup.GetState(gameState);
                    _states.Push(newState);
                    newState.InitSystems(_contexts);
                    newState.Activate();
                    _gameStateEntity.ReplaceGameState(gameState);
                    break;
                } 
                case StateMode.Previous:
                {
                    var state = _states.Pop();
                    state.Deactivate();
                    state.TearDown();
                    if (CurrentState == null)
                    {
                        throw new NullReferenceException("CurrentGameState is undefined!");
                    }
                    CurrentState.Activate();
                    _gameStateEntity.ReplaceGameState(GameStatesLookup.GetTag(CurrentState));
                    break;
                }
            }
        }

        private void TearDown()
        {
            while (_states.Count > 0)
            {
                var state = _states.Pop();
                state.Deactivate();
                state.TearDown();
            }
        }
    }
}