using System.Collections.Generic;
using Entitas;

namespace Systems.Game
{
    public class PositionSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _context;
        
        public PositionSystem(Contexts contexts) : base(contexts.game)
        {
            _context = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.View));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasView && entity.hasPosition;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.position.value.y = entity.view.obj.transform.localPosition.y;
                entity.view.obj.transform.localPosition = entity.position.value;
            }
        }
    }
}