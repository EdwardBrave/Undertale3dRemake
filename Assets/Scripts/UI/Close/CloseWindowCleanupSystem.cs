using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;

namespace UI.Close
{
    public class CloseWindowCleanupSystem: ICleanupSystem
    {
        private readonly IGroup<UiEntity> _group;

        public CloseWindowCleanupSystem(Contexts contexts)
        {
            _group = contexts.ui.GetGroup(UiMatcher.Close);
        }

        public void Cleanup()
        {
            foreach (var entity in _group.GetEntities())
            {
                if (entity.isProtected)
                {
                    entity.isClose = false;
                    continue;
                }
                
                if (entity.hasView)
                {
                    entity.view.obj.Unlink();
                    entity.view.obj.DestroyGameObject();
                    entity.view.parent.canvas.windows.Remove(entity);
                }

                entity.Destroy();
            }
        }
    }
}