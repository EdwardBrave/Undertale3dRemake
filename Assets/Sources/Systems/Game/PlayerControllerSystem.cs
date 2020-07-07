using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems.Game
{
    public class PlayerControllerSystem: ReactiveSystem<InputEntity>
    {
        private readonly GameContext _gameContext;
        private readonly IGroup<GameEntity> _playerControllers; 
        
        public PlayerControllerSystem(Contexts contexts) : base(contexts.input)
        {
            _gameContext = contexts.game;
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

            Vector3 direction;
            switch (eventEntity.keyPressed.key.ToUpper())
            {
                case "W":
                    direction = Vector3.forward;
                    break;
                case "A":
                    direction = Vector3.left;
                    break;
                case "S":
                    direction = Vector3.back;
                    break;
                case "D":
                    direction = Vector3.right;
                    break;
                default:
                    return;
            } 
            
            foreach (var entity in _playerControllers)
            {
                entity.ReplaceMoveInDirection(direction);
            }
        }
    }
}