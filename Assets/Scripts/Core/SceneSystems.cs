using Core.UnityScene;

namespace Core
{
    public class SceneSystems: Feature
    {
        public SceneSystems(Contexts contexts)
        {
            Add(new SceneInitSystem(contexts));
            Add(new SceneOffsetSystem(contexts));
            Add(new SceneCleanupSystem(contexts));
        }
    }
}