using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Processing
{
    public class MotionInputProcessingSystem: ReactiveSystem<InputEntity>, GameControls.IMotionActions
    {
        private readonly InputContext _context;

        public MotionInputProcessingSystem(Contexts contexts) : base(contexts.input)
        {
            _context = contexts.input;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(new TriggerOnEvent<InputEntity>(InputMatcher.Active, GroupEvent.AddedOrRemoved));
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasMotionInput;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.isActive)
                {
                    entity.motionInput.actions.SetCallbacks(this);
                    entity.motionInput.actions.Enable();
                }
                else
                {
                    entity.motionInput.actions.SetCallbacks(null);
                    entity.motionInput.actions.Disable();
                }
                
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _context.motionInputEntity.ReplaceMove(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _context.motionInputEntity.ReplaceLook(context.ReadValue<Vector2>());
        }
    }
}