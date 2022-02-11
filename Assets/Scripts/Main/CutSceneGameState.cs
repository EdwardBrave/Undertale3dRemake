using Entitas;
using Main.Features;

namespace Main
{
    public class CutSceneGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]{ new CutSceneSystems(contexts) };
        }
    }
}