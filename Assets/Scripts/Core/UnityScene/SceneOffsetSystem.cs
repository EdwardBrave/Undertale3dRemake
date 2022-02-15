using System.Collections.Generic;
using Entitas;

namespace Core.UnityScene
{
    public class SceneOffsetSystem: ReactiveSystem<CoreEntity>
    {
        public SceneOffsetSystem(Contexts contexts) : base(contexts.core)
        {
        }

        protected override ICollector<CoreEntity> GetTrigger(IContext<CoreEntity> context)
        {
            return context.CreateCollector(CoreMatcher.AllOf(
                CoreMatcher.Scene,
                CoreMatcher.SceneOffset
            ));
        }

        protected override bool Filter(CoreEntity entity)
        {
            return entity.hasScene && entity.hasSceneOffset;
        }

        protected override void Execute(List<CoreEntity> entities)
        {
            foreach (var entity in entities)
            {
                var gameObjects = entity.scene.value.GetRootGameObjects();
                var offset = entity.sceneOffset.value;
                foreach (var gameObject in gameObjects)
                {
                    gameObject.transform.position += offset;
                }
                entity.RemoveSceneOffset();
            }
        }
    }
}