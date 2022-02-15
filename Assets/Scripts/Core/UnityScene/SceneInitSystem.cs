using Entitas;
using UnityEngine.SceneManagement;

namespace Core.UnityScene
{
    public class SceneInitSystem: IExecuteSystem
    {
        private readonly IGroup<CoreEntity> _loadingOperations;
        
        public SceneInitSystem(Contexts contexts)
        {
            _loadingOperations = contexts.core.GetGroup(CoreMatcher.SceneLoading);
        }
        
        public void Execute()
        {
            foreach (var entity in _loadingOperations.GetEntities())
            {
                if (!entity.sceneLoading.operation.isDone) continue;
                
                entity.AddScene(SceneManager.GetSceneByName(entity.loadScene.name));
                entity.RemoveLoadScene();
                entity.RemoveSceneLoading();
            }
        }
    }
}