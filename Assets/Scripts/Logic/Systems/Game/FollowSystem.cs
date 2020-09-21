using Entitas;

namespace Logic.Systems.Game
{
    public class FollowSystem: IExecuteSystem
    {
        private readonly GameContext _context;
        private readonly IGroup<GameEntity> _followers;

        public FollowSystem(Contexts contexts)
        {
            _context = contexts.game;
            _followers = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Follow, GameMatcher.View));
        }
        
        public void Execute()
        {
            foreach (var entity in _followers)
            {
                var target = entity.follow.target;
                if (target == null || !target.hasView)
                    continue;
                entity.view.obj.transform.position = target.view.Position + entity.follow.relativePosition;
            }
        }
    }
}