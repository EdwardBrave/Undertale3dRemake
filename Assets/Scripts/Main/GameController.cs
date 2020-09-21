using System;

namespace Main
{
    public sealed class GameController 
    {
        private readonly GameInitialization _gameData;
        private readonly Contexts _contexts;
        private readonly Feature _systems;

        public GameController(GameInitialization gameData, Contexts contexts, Type feature)
        {
            _gameData = gameData;
            _contexts = contexts;
            contexts.Reset();
            contexts.core.SetCoreConfig(gameData.config);
            contexts.core.SetGameSettings(gameData.settings);
            
            _systems = (Feature) Activator.CreateInstance(feature, contexts);
        }

        public void Initialize()
        {
            _systems.Initialize();
        }

        public void Execute()
        {
            _systems.Execute();
            _systems.Cleanup();
        }
    }
}
