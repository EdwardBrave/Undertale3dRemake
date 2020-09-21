using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;

namespace Logic.Systems.UI
{
    public class CloseWindowCleanupSystem: ICleanupSystem
    {
        private readonly IGroup<UiEntity> _group;

        private readonly List<UiEntity> _buffer;
        public CloseWindowCleanupSystem(Contexts contexts)
        {
            _group = contexts.ui.GetGroup(UiMatcher.Close);
            _buffer = new List<UiEntity>();
        }

        public void Cleanup()
        {
            foreach (var entity in _group.GetEntities(_buffer))
            {
                if (entity.hasView)
                {
                    entity.view.obj.Unlink();
                    entity.view.obj.DestroyGameObject();
                    entity.view.canvas.canvas.windows.Remove(entity);
                }

                entity.Destroy();
            }
        }
    }
}