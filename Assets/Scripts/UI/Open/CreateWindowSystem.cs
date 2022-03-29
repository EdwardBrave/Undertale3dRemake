using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Open
{
    public class CreateWindowSystem: ReactiveSystem<UiEntity>
    {

        public CreateWindowSystem(Contexts contexts) : base(contexts.ui)
        {
        }

        protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
        {
            return context.CreateCollector(UiMatcher.CreateWindow);
        }

        protected override bool Filter(UiEntity entity)
        {
            return entity.hasCreateWindow && !entity.hasView;
        }

        protected override void Execute(List<UiEntity> entities)
        {
            foreach (var entity in entities)
            {
                InitWindow(entity, entity.createWindow.prefab, entity.createWindow.container);
            }
        }

        private void InitWindow(UiEntity uiEntity, InitUiEntity basePrefab, UiEntity container)
        {
            Transform parent = null;

            if (container is {hasContainer: true, hasView: true})
            {

                var windowsList = container.container.windows;
                windowsList.Add(uiEntity);
                container.ReplaceContainer(windowsList);
                parent = container.view.obj.transform;
            }
            else
            {
                container = null;
            }
            
            var newViewObject = Object.Instantiate(basePrefab.gameObject, parent);
            newViewObject.Link(uiEntity);
            uiEntity.AddView(newViewObject, container);

            foreach (var component in basePrefab.components)
            {
                int index = Array.IndexOf(UiComponentsLookup.componentTypes, component.GetType());
                uiEntity.AddComponent(index, component);
            }

            newViewObject.GetComponent<InitUiEntity>()?.DestroySelf();
            uiEntity.RemoveCreateWindow();
        }
    }
}