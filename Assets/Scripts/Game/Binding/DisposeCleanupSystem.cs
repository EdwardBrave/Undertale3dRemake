using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;

namespace Game.Binding
{
    public class DisposeCleanupSystem: ICleanupSystem
    {
        private readonly IGroup<GameEntity> _disposingGroup;

        public DisposeCleanupSystem(Contexts contexts)
        {
            _disposingGroup = contexts.game.GetGroup(GameMatcher.Dispose);
        }
        
        public void Cleanup()
        {
            foreach (var entity in _disposingGroup.GetEntities())
            {
                if (!entity.dispose.IsAllProcessesDone())
                {
                    continue;
                }
                
                if (entity.hasView)
                {
                    entity.view.obj.Unlink();
                    entity.view.obj.DestroyGameObject();
                }
                entity.Destroy();
            }
        }
    }
}