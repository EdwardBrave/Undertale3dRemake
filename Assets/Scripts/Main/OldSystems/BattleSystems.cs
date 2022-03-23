using Game.Animation;
using Game.Cleanup;
using Game.Controllers;
using Game.Follow;
using Game.InitLogic;
using Game.Motion;

namespace Main.GameStates
{
    public sealed class BattleSystems: Feature
    {
        public BattleSystems(Contexts contexts)
        {
            Add(new InitFromSceneSystem(contexts));

            // Add(new InputGameEventsSystem(contexts));
            Add(new PlayerControllerSystem(contexts));
            Add(new MotionSystem(contexts));
            Add(new AnimationSystem(contexts));
            Add(new FollowSystem(contexts));

            Add(new DestroyedCleanupSystem(contexts));
        }
    }
}