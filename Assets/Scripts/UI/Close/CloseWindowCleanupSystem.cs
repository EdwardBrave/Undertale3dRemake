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
                if (!entity.close.isForce && IsRecursiveProtected(entity))
                {
                    entity.RemoveClose();
                    continue;
                }

                if (entity.hasContainer && entity.container.windows.Count > 0)
                {
                    foreach (var childEntity in entity.container.windows)
                    {
                        if (childEntity is UiEntity {hasClose: true} uiChildEntity)
                        {
                            uiChildEntity.AddClose(entity.close.isForce);
                        }
                    }
                    continue;
                }

                if (entity.hasView)
                {
                    entity.view.obj.Unlink();
                    entity.view.obj.DestroyGameObject();
                    entity.view.parent?.container.windows.Remove(entity);
                }

                entity.Destroy();
            }
        }

        private static bool IsRecursiveProtected(UiEntity entity)
        {
            if (entity.hasContainer)
            {
                foreach (var subEntity in entity.container.windows)
                {
                    if (subEntity is UiEntity uiSubEntity && IsRecursiveProtected(uiSubEntity))
                    {
                        return true;
                    }
                }
            }
            
            return entity.isProtected;
        }
    }
}