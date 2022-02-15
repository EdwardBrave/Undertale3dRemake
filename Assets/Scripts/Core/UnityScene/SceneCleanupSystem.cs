using Entitas;
using UnityEngine.SceneManagement;

namespace Core.UnityScene
{
    public class SceneCleanupSystem: ICleanupSystem
    {
        private readonly IGroup<CoreEntity> _closeSceneGroup;

        public SceneCleanupSystem(Contexts contexts)
        {
            _closeSceneGroup = contexts.core.GetGroup(CoreMatcher.Scene);
        }

        public void Cleanup()
        {
            var entities = _closeSceneGroup.GetEntities();
            foreach (var entity in entities)
            {
                if (!entity.scene.value.isLoaded)
                {
                    entity.Destroy();
                }
                else if (entity.isCloseScene)
                {
                    SceneManager.UnloadSceneAsync(entity.scene.value);
                    entity.Destroy();
                }
            }
        }
    }
}