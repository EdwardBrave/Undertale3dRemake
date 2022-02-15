using Entitas;

namespace Main.GameStates
{
    public class BattleGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]{ new BattleSystems(contexts) };
        }
    }
}