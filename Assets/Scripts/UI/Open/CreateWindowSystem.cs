using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Open
{
    public class CreateWindowSystem: ReactiveSystem<UiEntity>
    {
        private readonly UiContext _uiContext;
        private readonly IGroup<UiEntity> _containers;
        private readonly IGroup<UiEntity> _views;
        
        public CreateWindowSystem(Contexts contexts) : base(contexts.ui)
        {
            _uiContext = contexts.ui;
            _containers = _uiContext.GetGroup(UiMatcher.AllOf(UiMatcher.View, UiMatcher.Container));
            _views = _uiContext.GetGroup(UiMatcher.AllOf(UiMatcher.View).NoneOf(UiMatcher.Container));
        }

        protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
        {
            return context.CreateCollector(UiMatcher.CreateWindow);
        }

        protected override bool Filter(UiEntity entity)
        {
            return !entity.hasView;
        }

        protected override void Execute(List<UiEntity> entities)
        {
            foreach (var entity in entities)
            {
                InitWindow(entity, entity.createWindow.data);
            }
        }

        private UiEntity InitWindow(UiEntity uiEntity, InitUiEntity initData)
        {
            Transform parent = null;
            UiEntity containerEntity = null;
            
            if (initData.containerPrefab)
            {
                containerEntity = 
                    _containers.AsEnumerable().FirstOrDefault(entity => entity.view.obj.name == initData.containerPrefab.name) ?? 
                    _views.AsEnumerable().FirstOrDefault(entity => entity.view.obj.name == initData.containerPrefab.name) ?? 
                    InitWindow(_uiContext.CreateEntity(), initData.containerPrefab);

                var windowsList = containerEntity.hasContainer ? containerEntity.container.windows : new List<UiEntity>();
                windowsList.Add(uiEntity);
                containerEntity.ReplaceContainer(windowsList);
                parent = containerEntity.view.obj.transform;
            }
            
            var newView = Object.Instantiate(initData.gameObject, parent);
            newView.name = initData.name;
            newView.Link(uiEntity);
            uiEntity.AddView(newView, containerEntity);

            foreach (var component in initData.components)
            {
                int index = Array.IndexOf(UiComponentsLookup.componentTypes, component.GetType());
                uiEntity.AddComponent(index, component);
            }

            newView.GetComponent<InitUiEntity>()?.DestroySelf();
            if (uiEntity.hasCreateWindow)
            {
                uiEntity.RemoveCreateWindow();
            }
            
            return uiEntity;
        }
    }
}