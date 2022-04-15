using Core;
using Core.Camera;
using Core.Localization;
using Entitas;
using Game;
using Input;
using UI;

namespace Main
{
    public class MainGameLogic: GameState
    {
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]
            {
                new LocalizationSystem(contexts),
                new CameraInitSystem(contexts),
                new UnityInputInitSystem(contexts),
                new UiInitSystem(contexts),
                new InputProcessingSystems(contexts),
                new UISystems(contexts),
                new GameBindingSystems(contexts),
                new SceneSystems(contexts),
            };
        }
    }
}