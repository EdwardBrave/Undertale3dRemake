using Entitas;

namespace Game.Follow
{
    public class FollowSystem: IExecuteSystem
    {
        private readonly IGroup<GameEntity> _followers;

        public FollowSystem(Contexts contexts)
        {
            _followers = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Follow, GameMatcher.View));
        }
        
        public void Execute()
        {
            foreach (var entity in _followers)
            {
                var target = entity.follow.target;
                if (target == null)
                    continue;
                entity.view.obj.transform.position = target.transform.position + entity.follow.relativePosition;
            }
        }
    }
}