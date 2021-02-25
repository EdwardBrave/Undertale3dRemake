using System.Collections.Generic;
using Entitas;
using Logic.Components.Input;
using UnityEngine;

namespace Game.Controllers
{
    public class PlayerControllerSystem: ReactiveSystem<InputEntity>
    {
        private readonly IGroup<GameEntity> _playerControllers; 
        
        public PlayerControllerSystem(Contexts contexts) : base(contexts.input)
        {
            _playerControllers = contexts.game.GetGroup(GameMatcher.PlayerController);
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(
                new TriggerOnEvent<InputEntity>(InputMatcher.KeyPressed, GroupEvent.AddedOrRemoved));
        }

        protected override bool Filter(InputEntity entity)
        {
            return true;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            var eventEntity = entities.SingleEntity();
            if (!eventEntity.hasKeyPressed)
            {
                foreach (var entity in _playerControllers)
                    if (entity.hasMoveInDirection)
                        entity.RemoveMoveInDirection();
                return;
            }

            Vector3 direction = Vector3.zero;
            var key = eventEntity.keyPressed.key;
            if ((key & GameKey.Up) != GameKey.None)
                direction.z += 1;
            if ((key & GameKey.Left) != GameKey.None)
                direction.x -= 1;
            if ((key & GameKey.Down) != GameKey.None)
                direction.z -= 1;
            if ((key & GameKey.Right) != GameKey.None)
                direction.x += 1;

            foreach (var entity in _playerControllers)
            {
                entity.ReplaceMoveInDirection(direction);
            }
        }
    }
}