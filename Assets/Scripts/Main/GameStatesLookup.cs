using System;
using System.Collections.Generic;
using System.Linq;
using Main.GameStates;

namespace Main
{
    public enum RegisteredGameState
    {
        None,
        InitGameState,
        BattleGameState,
        CutSceneGameState,
    }

    public static class GameStatesLookup
    {
        private static Dictionary<RegisteredGameState, Type> _states = new Dictionary<RegisteredGameState, Type>
        {
            {RegisteredGameState.InitGameState, typeof(InitGameState)},
            {RegisteredGameState.BattleGameState, typeof(BattleGameState)},
            {RegisteredGameState.CutSceneGameState, typeof(CutSceneGameState)},
        };

        public static GameState GetState(RegisteredGameState gameStateTag)
        {
            if (!_states.ContainsKey(gameStateTag))
                throw new NotImplementedException();
            return (GameState) Activator.CreateInstance(_states[gameStateTag]);
        }
        
        public static RegisteredGameState GetTag(GameState gameState)
        {
            var type = gameState.GetType();
            var gameStateTag = _states.FirstOrDefault(pair => pair.Value == type).Key;
            if (gameStateTag == RegisteredGameState.None)
                throw new NotImplementedException();
            return gameStateTag;
        }
    }
}