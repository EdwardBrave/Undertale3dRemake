using Entitas;

namespace Main.GameStates
{
    public class TravelGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]{ };
        }
    }
}