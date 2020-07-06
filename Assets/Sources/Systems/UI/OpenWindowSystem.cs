using System.Collections.Generic;
using System.Linq;
using Data;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.UI
{
    public class OpenWindowSystem: ReactiveSystem<UiEntity>
    {
        private readonly UiContext _uiContext;
        private readonly CoreConfig _coreConfig;
        private readonly IGroup<UiEntity> _canvases;
        
        public OpenWindowSystem(Contexts contexts) : base(contexts.ui)
        {
            _uiContext = contexts.ui;
            _coreConfig = contexts.core.coreConfig.value;
            _canvases = contexts.ui.GetGroup(UiMatcher.Canvas);
        }

        protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
        {
            return context.CreateCollector(UiMatcher.Window);
        }

        protected override bool Filter(UiEntity entity)
        {
            return !entity.hasView;
        }

        protected override void Execute(List<UiEntity> entities)
        {
            foreach (var entity in entities)
            {
                var canvasName = entity.window.canvasName;
                GameObject window;
                if (canvasName != null)
                {
                    var prefab = Resources.Load<GameObject>(_coreConfig.uiPrefabsPath + entity.window.path);
                    if (!prefab) continue;

                    UiEntity canvasEntity = _canvases.AsEnumerable().First(o => o.canvas.name == canvasName);
                    window = Object.Instantiate(prefab, canvasEntity.view.obj.transform);
                    canvasEntity.canvas.windows.Add(entity);
                    entity.AddView(window, canvasEntity);
                }
                else
                {
                    var prefab = Resources.Load<GameObject>(_coreConfig.canvasesPath + entity.window.path);
                    if (!prefab) continue;
                    window = Object.Instantiate(prefab);
                    entity.AddView(window, null);
                }
                window.Link(entity);
            }
        }
    }
}