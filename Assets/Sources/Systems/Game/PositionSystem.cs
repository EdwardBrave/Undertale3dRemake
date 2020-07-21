using System.Collections.Generic;
using Entitas;

namespace Systems.Game
{
    public class PositionSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _context;
        private readonly IGroup<GameEntity> _positionGroup;

        public PositionSystem(Contexts contexts) : base(contexts.game)
        {
            _context = contexts.game;
            _positionGroup = _context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Position, GameMatcher.View, GameMatcher.Motion));
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
                entity.view.obj.transform.localPosition = entity.position.value;
                entity.RemovePosition();
            }
        }
    }
}