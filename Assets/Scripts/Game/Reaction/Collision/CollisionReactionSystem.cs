using System.Collections.Generic;
using Entitas;
using Tools;
using UnityEngine;

namespace Game.Reaction.Collision
{
    public class CollisionReactionSystem: ReactiveSystem<GameEntity>
    {
        
        public CollisionReactionSystem(Contexts contexts) : base(contexts.game)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher
                .AllOf(GameMatcher.Reaction)
                .AnyOf(GameMatcher.Collisions, GameMatcher.Triggers));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasReaction;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.hasCollisions)
                {
                    DispatchCollisions(entity, entity.collisions.list, false);
                }

                if (entity.hasTriggers)
                {
                    DispatchCollisions(entity, entity.triggers.list, true);
                }
            }
        }

        private void DispatchCollisions(GameEntity entity, List<Temporary<Collider>> collisions, bool isTriggers)
        {
            foreach (var collision in collisions)
            {
                if (collision.Status != TemporaryStatus.Stay)
                {
                    entity.SendReactionEvent(new CollisionEventArgs(collision.Data, collision.Status, isTriggers));
                }
            }
        }
    }
}