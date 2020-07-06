using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Input
{
    public class UIHandlerSystem : ReactiveSystem<InputEntity>
    {
        private readonly UiContext _uiContext;
        private readonly InputEntity _uiEventsEntity;

        public UIHandlerSystem(Contexts contexts): base(contexts.input)
        {
            _uiContext = contexts.ui;
            _uiEventsEntity = contexts.input.uiEventsEntity;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.UiRequest);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasUiRequest;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var inputEntity in entities)
            {
                var data = inputEntity.uiRequest.data;
                var windowEntity = (UiEntity)inputEntity.uiRequest.sender.GetComponent<EntityLink>()?.entity;
                switch (data.type)
                {
                    case UIEventArgs.Confirm:
                        if (windowEntity == null) continue;
                        windowEntity.isConfirm = true;
                        break;
                    case UIEventArgs.Reject:
                        if (windowEntity == null) continue;
                        windowEntity.isReject = true;
                        break;
                    case UIEventArgs.Open:
                        var uiEntity = _uiContext.CreateEntity();
                        var windowArgs = data.args[0].Split(':');
                        uiEntity.AddWindow((data.args.Length > 2 && data.args[1] == "in") ? data.args[2] : "0", windowArgs[0]);
                        if (windowArgs.Length > 1)
                            uiEntity.ReplaceUiData(windowArgs[1]);
                        break;
                    case UIEventArgs.Close:
                        if (windowEntity == null) continue;
                            windowEntity.isClose = true;
                        break;
                    case UIEventArgs.Command:
                        if (windowEntity == null) continue;
                        windowEntity.AddCommand(data.args);
                        break;
                    case UIEventArgs.GlobalCommand:
                        _uiEventsEntity.AddCommand(data.args);
                        break;
                    default:
                        Debug.LogWarning($"The UI event type \"{data.type}\" is undefined!");
                        break;
                }
            }
        }
    }
}
