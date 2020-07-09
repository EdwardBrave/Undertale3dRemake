using Systems.Game;
using Systems.Input;

namespace Systems.Features
{
    public sealed class GameSystems: Feature
    {
        public GameSystems(Contexts contexts)
        {
            Add(new CreatePlayerSystem(contexts));
            Add(new InitViewSystem(contexts));
            
            Add(new InputGameEventsSystem(contexts));
            Add(new PlayerControllerSystem(contexts));
            Add(new MotionSystem(contexts));
            Add(new PositionSystem(contexts));
            Add(new RotationSystem(contexts));
            Add(new AnimatorSystem(contexts));

            Add(new UISystems(contexts));
            
            Add(new DestroyedCleanupSystem(contexts));
        }
    }
}