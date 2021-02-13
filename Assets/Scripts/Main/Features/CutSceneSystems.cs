using Game.SceneLoad;

namespace Main.Features
{
    public sealed class CutSceneSystems: Feature
    {
        public CutSceneSystems(Contexts contexts)
        {
            Add(new InitFromSceneSystem(contexts));
        }
    }
}