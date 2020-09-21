using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Logic.Systems.Game
{
    public class AnimatorSystem: ReactiveSystem<GameEntity>
    {
        private GameContext _gameContext;

        public AnimatorSystem(Contexts contexts): base(contexts.game)
        {
            _gameContext = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Motion);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasAnimator && entity.animator.value;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.animator.value.SetFloat("speed", entity.motion.speed);
            }
        }
    }
}