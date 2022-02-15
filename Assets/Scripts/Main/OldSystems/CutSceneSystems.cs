using Game.InitLogic;

namespace Main.GameStates
{
    public sealed class CutSceneSystems: Feature
    {
        public CutSceneSystems(Contexts contexts)
        {
            Add(new InitFromSceneSystem(contexts));
        }
    }
}