using System;
using Systems.Features;
using Data;
using UnityEngine;

namespace Main
{
    public sealed class GameInitialization : MonoBehaviour
    {
        public Features systems;
        public CoreConfig config;
        public GameSettings settings;
        private GameController _gameController;
        void Awake() => _gameController = new GameController(this, Contexts.sharedInstance, GetFeatureType(systems));
        void Start() => _gameController.Initialize();
        void Update() => _gameController.Execute();
        
        private static Type GetFeatureType(Features systems)
        {
            switch (systems)
            {
                case Features.Default:
                    return typeof(MainMenuSystems);
                case Features.MainMenu:
                    return typeof(MainMenuSystems);
                default:
                    throw new ArgumentOutOfRangeException(nameof(systems), systems, null);
            }
        }
    }
        
    public enum Features
    {
        Default,
        MainMenu
    }
}
