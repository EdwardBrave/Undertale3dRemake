﻿using Entitas;

namespace Logic.Systems.Input
{
    public class UIRequestsCleanupSystem : ICleanupSystem
    {
        private readonly ICollector<InputEntity> _collector;
        
        public UIRequestsCleanupSystem(Contexts contexts)
        {
            _collector = contexts.input.CreateCollector(InputMatcher.AnyOf(InputMatcher.UiRequest, InputMatcher.Command));
        }

        public void Cleanup()
        {
            foreach (var entity in _collector.collectedEntities)
            {
                if (entity.hasUiRequest)
                    entity.RemoveUiRequest();
                if (entity.hasCommand)
                    entity.RemoveCommand();
                /*if (list.hasThrowDices)
                    list.RemoveThrowDices();*/
            }
            _collector.ClearCollectedEntities();
        }
    }
}