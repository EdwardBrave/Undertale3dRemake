using System.Collections.Generic;
using Entitas;
using UnityEngine.InputSystem;

namespace Input.Processing
{
    public class UIInputProcessingSystem: ReactiveSystem<InputEntity>, GameControls.IUIActions
    {
        private readonly InputContext _context;

        public UIInputProcessingSystem(Contexts contexts) : base(contexts.input)
        {
            _context = contexts.input;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(new TriggerOnEvent<InputEntity>(InputMatcher.Active, GroupEvent.AddedOrRemoved));
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasUiInput;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.isActive)
                {
                    entity.uiInput.actions.SetCallbacks(this);
                    entity.uiInput.actions.Enable();
                }
                else
                {
                    entity.uiInput.actions.SetCallbacks(null);
                    entity.uiInput.actions.Disable();
                }
            }
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            // TODO implement on the UI integration stage
            throw new System.NotImplementedException();
        }
    }
}