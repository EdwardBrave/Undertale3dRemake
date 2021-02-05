using System.Collections.Generic;
using Logic.Components.Input;
using Entitas;
using Entitas.Unity;
using UI;
using UI.Binding;
using UnityEngine;

namespace Logic.Systems.Input
{
    public class UIEventHandlerSystem : IInitializeSystem, IExecuteSystem
    {
        private readonly UiContext _uiContext;
        private readonly InputContext _inputContext;

        private InputEntity _entity;

        private readonly Queue<UiRequestComponent> _requestBuffer;

        public UIEventHandlerSystem(Contexts contexts)
        {
            _uiContext = contexts.ui;
            _inputContext = contexts.input;
            _inputContext.isUiEvents = true;
            _requestBuffer = new Queue<UiRequestComponent>();
        }

        public void Initialize()
        {
            _entity = _inputContext.uiEventsEntity;
            UIHandler.UIEvent += OnUIEvent;
        }
        
        public void Execute()
        {
            if (_entity.hasUiRequest || _requestBuffer.Count == 0) return;
            var request = _requestBuffer.Dequeue();
                _entity.AddUiRequest(request.sender, request.data);
        }

        private void OnUIEvent(UIHandler sender, UIEventArgs e)
        {
            if (_entity.hasUiRequest)
                _requestBuffer.Enqueue( new UiRequestComponent{sender = sender, data = e});
            else
                _entity.AddUiRequest(sender, e);
        }
    }
}
