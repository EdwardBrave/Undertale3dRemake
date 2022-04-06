using Core.Localization;
using Core.UnityScene;
using Entitas;
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
                new UnityInputInitSystem(contexts),
                new UiInitSystem(contexts),
                new InputProcessingSystems(contexts),
                new UISystems(contexts),
                new SceneInitSystem(contexts),
                new SceneOffsetSystem(contexts),
                new SceneCleanupSystem(contexts),
            };
        }
    }
}