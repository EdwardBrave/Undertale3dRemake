using System.Collections.Generic;
using Entitas;
using UnityEngine.SceneManagement;

namespace Core.UnityScene
{
    public class SceneLoadingSystem: ReactiveSystem<CoreEntity>
    {

        public SceneLoadingSystem(Contexts contexts) : base(contexts.core)
        {
        }

        protected override ICollector<CoreEntity> GetTrigger(IContext<CoreEntity> context)
        {
            return context.CreateCollector(CoreMatcher.LoadScene);
        }

        protected override bool Filter(CoreEntity entity)
        {
            return entity.hasLoadScene;
        }

        protected override void Execute(List<CoreEntity> entities)
        {
            foreach (var entity in entities)
            {
                var operation = SceneManager.LoadSceneAsync(entity.loadScene.name, entity.loadScene.mode);
                entity.AddSceneLoading(operation);
            }
        }
    }
}