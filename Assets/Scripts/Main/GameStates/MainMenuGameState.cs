using Entitas;

namespace Main.GameStates
{
    public class MainMenuGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]{ };
        }
    }
}