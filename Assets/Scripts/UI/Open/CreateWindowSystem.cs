using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Open
{
    public class CreateWindowSystem: IExecuteSystem
    {
        private readonly IGroup<UiEntity> _creatorsGroup;

        public CreateWindowSystem(Contexts contexts)
        {
            _creatorsGroup = contexts.ui.GetGroup(UiMatcher.CreateWindow);
        }
        
        public void Execute()
        {
            foreach (var entity in _creatorsGroup.GetEntities())
            {
                InitWindow(entity, entity.createWindow.prefab, entity.createWindow.container);
            }
        }

        private void InitWindow(UiEntity uiEntity, InitUiEntity basePrefab, UiEntity container)
        {
            Transform parent = null;

            if (container is {hasContainer: true, hasView: true})
            {

                var windowsList = container.container.windows ?? new List<IEntity>();
                windowsList.Add(uiEntity);
                container.ReplaceContainer(windowsList);
                parent = container.view.obj.transform;
            }
            else
            {
                container = null;
            }
            
            var newViewObject = Object.Instantiate(basePrefab.gameObject, parent);
            newViewObject.name = basePrefab.gameObject.name;
            newViewObject.Link(uiEntity);
            uiEntity.AddView(newViewObject, container);
            var initUiEntity = newViewObject.GetComponent<InitUiEntity>();

            foreach (var component in initUiEntity.components)
            {
                int index = Array.IndexOf(UiComponentsLookup.componentTypes, component.GetType());
                if (!uiEntity.HasComponent(index))
                {
                    uiEntity.AddComponent(index, component);
                }
            }

            initUiEntity.DestroySelf();
            uiEntity.RemoveCreateWindow();
        }
    }
}