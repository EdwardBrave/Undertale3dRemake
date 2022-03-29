using Core.UnityScene;
using Entitas;
using Input;
using UI;

namespace Main.GameStates
{
    public class InitGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]
            {
                new UnityInputInitSystem(contexts),
                new UiInitSystem(contexts),
                new InputProcessingSystems(contexts),
                new UISystems(contexts),
                new SceneInitSystem(contexts),
                new SceneOffsetSystem(contexts),
                new SceneCleanupSystem(contexts)
            };
        }
    }
}