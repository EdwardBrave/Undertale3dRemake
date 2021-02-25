using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Game.Animation
{
    public class AnimationSystem: ReactiveSystem<GameEntity>
    {
        private static readonly int Speed = Animator.StringToHash("speed");

        public AnimationSystem(Contexts contexts): base(contexts.game) { }

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
                entity.animator.value.SetFloat(Speed, entity.motion.speed);
            }
        }
    }
}