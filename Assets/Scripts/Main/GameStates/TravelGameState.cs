using Entitas;
using Game;

namespace Main.GameStates
{
    public class TravelGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]
            {
                new CollisionSystems(contexts),
            };
        }
    }
}