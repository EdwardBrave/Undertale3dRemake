using Core.UnityScene;
using Entitas;

namespace Main.GameStates
{
    public class InitGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]
            {
                new SceneLoadingSystem(contexts),
                new SceneInitSystem(contexts),
                new SceneOffsetSystem(contexts),
                new SceneCleanupSystem(contexts)
            };
        }
    }
}