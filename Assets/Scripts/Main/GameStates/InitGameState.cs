using Core.UnityScene;
using Entitas;
using Input;

namespace Main.GameStates
{
    public class InitGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]
            {
                new UnityInputInitSystem(contexts),
                new InputProcessingSystems(contexts),
                new SceneInitSystem(contexts),
                new SceneOffsetSystem(contexts),
                new SceneCleanupSystem(contexts)
            };
        }
    }
}