using System;
using System.Collections.Generic;
using Bolt;
using Entitas;

namespace Game.Reaction
{
    public class ReactionSystem: ReactiveSystem<GameEntity>
    {
        private const string ReactionEventName = "Reaction";
        
        public ReactionSystem(Contexts contexts) : base(contexts.game) { }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.CollisionTriggered);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasReaction;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                foreach(var gameEntity in entity.collisionTriggered.list)
                {
                    CustomEvent.Trigger(entity.reaction.onReaction.gameObject, ReactionEventName, gameEntity);
                }
            }
        }
    }
}