using Game.Collision;

namespace Game
{
    public class CollisionSystems: Feature
    {
        public CollisionSystems(Contexts contexts)
        {
            Add(new CollisionHandlerSystem(contexts));
            Add(new CollisionCleanupSystem(contexts));
        }
    }
}