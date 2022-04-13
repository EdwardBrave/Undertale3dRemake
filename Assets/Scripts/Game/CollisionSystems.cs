using Game.Collision;
using Game.Reaction.Collision;

namespace Game
{
    public class CollisionSystems: Feature
    {
        public CollisionSystems(Contexts contexts)
        {
            Add(new CollisionHandlerSystem(contexts));
            Add(new CollisionReactionSystem(contexts));
            Add(new CollisionCleanupSystem(contexts));
        }
    }
}