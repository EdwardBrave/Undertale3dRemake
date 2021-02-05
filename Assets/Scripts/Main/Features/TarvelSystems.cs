using Logic.Systems.Game;
using Logic.Systems.Input;

namespace Main.Features
{
    public sealed class TravelSystems: Feature
    {
        public TravelSystems(Contexts contexts)
        {
            Add(new InitSceneSystem(contexts));
            Add(new CreatePlayerSystem(contexts));
            Add(new InitViewSystem(contexts));
            Add(new FindViewSystem(contexts));
            
            Add(new InputGameEventsSystem(contexts));
            Add(new PlayerControllerSystem(contexts));
            Add(new MotionSystem(contexts));
            Add(new PositionSystem(contexts));
            Add(new RotationSystem(contexts));
            Add(new AnimatorSystem(contexts));
            Add(new FollowSystem(contexts));

            Add(new UISystems(contexts));
            
            Add(new DestroyedCleanupSystem(contexts));
        }
    }
}