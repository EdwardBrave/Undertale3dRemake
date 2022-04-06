using Entitas;

namespace Main.GameStates
{
    public class CutSceneGameState: GameState
    {
        
        protected override ISystem[] GetSystems(Contexts contexts)
        {
            return new ISystem[]{ };
        }
    }
}