using System.Collections.Generic;
using Entitas;
using UI;
using UnityEngine;

namespace Systems.UI
{
    public class LoadUIDataSystem: ReactiveSystem<UiEntity>
    {
        private readonly string _dataPath;
        public LoadUIDataSystem(Contexts contexts) : base(contexts.ui)
        {
            _dataPath = contexts.core.coreConfig.value.dataPath;
        }

        protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
        {
            return context.CreateCollector(UiMatcher.AllOf(UiMatcher.View, UiMatcher.UiData));
        }

        protected override bool Filter(UiEntity entity)
        {
            return entity.hasView && entity.hasUiData;
        }

        protected override void Execute(List<UiEntity> entities)
        {
            foreach (var entity in entities)
            {
                var binder = entity.view.obj.GetComponent<UIBinder>();
                if (!binder)
                    continue;
                binder.LoadUIData(Resources.Load<UIData>(_dataPath + entity.uiData.path));
            }
        }
    }
}