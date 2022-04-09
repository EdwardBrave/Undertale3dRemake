using System.Collections.Generic;
using Entitas;
using Tools;

namespace Game.Collision
{
    public class CollisionCleanupSystem: ICleanupSystem
    {
        private readonly IGroup<GameEntity> _collidersGroup;

        public CollisionCleanupSystem(Contexts contexts)
        {
            _collidersGroup = contexts.game.GetGroup(GameMatcher.Collider);
        }
        
        public void Cleanup()
        {
            foreach (var entity in _collidersGroup.GetEntities())
            {
                if (entity.hasCollisions)
                {
                    UpdateTemporaries(entity.collisions.list);
                    if (entity.collisions.list.Count == 0)
                    {
                        entity.RemoveCollisions();
                    }
                }
                if (entity.hasTriggers)
                {
                    UpdateTemporaries(entity.triggers.list);
                    if (entity.triggers.list.Count == 0)
                    {
                        entity.RemoveTriggers();
                    }
                }
            }
        }

        private void UpdateTemporaries<T>(List<Temporary<T>> list) 
        {
            var internalList = new LinkedList<Temporary<T>>(list);
            foreach (var temporary in internalList)
            {
                switch (temporary.Status)
                {
                    case TemporaryStatus.Started:
                        temporary.Status = TemporaryStatus.Stay;
                        break;
                    case TemporaryStatus.Ended:
                        list.Remove(temporary);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}