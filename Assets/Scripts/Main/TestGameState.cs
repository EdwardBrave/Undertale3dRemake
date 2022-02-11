using Entitas;

namespace Main
{
    public class TestGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]{ new Feature() };
        }
    }
}