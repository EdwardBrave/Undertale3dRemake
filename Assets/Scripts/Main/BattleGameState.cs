using Entitas;
using Main.Features;

namespace Main
{
    public class BattleGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]{ new BattleSystems(contexts) };
        }
    }
}