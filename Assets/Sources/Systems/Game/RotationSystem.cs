namespace Systems.Game
{
    using System.Collections.Generic;
    using Entitas;
    public class RotationSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _context;
    
        public RotationSystem(Contexts contexts) : base(contexts.game)
        {
            _context = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Rotation, GameMatcher.View));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasView && entity.hasRotation;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.view.obj.transform.eulerAngles = entity.rotation.euler;
                entity.RemoveRotation();
            }
        }
    }
}