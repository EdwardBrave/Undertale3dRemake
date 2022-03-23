using System.Collections.Generic;
using Entitas;
using UnityEngine.InputSystem;

namespace Input.Processing
{
    public class BattleInputProcessingSystem: ReactiveSystem<InputEntity>, GameControls.IBattleActions
    {
        private readonly InputContext _context;

        public BattleInputProcessingSystem(Contexts contexts) : base(contexts.input)
        {
            _context = contexts.input;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(new TriggerOnEvent<InputEntity>(InputMatcher.Active, GroupEvent.AddedOrRemoved));
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasBattleInput;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.isActive)
                {
                    entity.battleInput.actions.SetCallbacks(this);
                    entity.battleInput.actions.Enable();
                }
                else
                {
                    entity.battleInput.actions.SetCallbacks(null);
                    entity.battleInput.actions.Disable();
                }
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            _context.battleInputEntity.isFire = context.performed;
        }
    }
}