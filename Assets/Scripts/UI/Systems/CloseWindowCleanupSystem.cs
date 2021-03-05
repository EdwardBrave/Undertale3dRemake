using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;
using UnityEngine;

namespace UI
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
                if (entity.isProtected)
                {
                    entity.isClose = false;
                    Debug.LogWarning("Attempt to close protected window! You should reset the protection.");
                    continue;
                }
                
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