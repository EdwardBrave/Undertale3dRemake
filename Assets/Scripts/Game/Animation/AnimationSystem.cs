using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Game.Animation
{
    public class AnimationSystem: ReactiveSystem<GameEntity>, ITearDownSystem
    {
        private static readonly int Speed = Animator.StringToHash("speed");
        
        private readonly IGroup<GameEntity> _animationGroup;

        public AnimationSystem(Contexts contexts) : base(contexts.game)
        {
            _animationGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Animator, GameMatcher.Motion));
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
                entity.animator.value.SetFloat(Speed, entity.motion.speed);
            }
        }

        public void TearDown()
        {
            foreach (var entity in _animationGroup.GetEntities())
            {
                if (entity.animator.value)
                {
                    entity.animator.value.SetFloat(Speed, entity.motion.speed);
                }
            }
        }
    }
}