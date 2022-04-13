using System;
using System.Collections.Generic;
using Game.Reaction.Collision;
using UnityEngine;

namespace Game.Reaction
{
    public class Reactions: MonoBehaviour
    {
        
        [SerializeField]
        private List<CollisionUnityEvent> onCollisions = new List<CollisionUnityEvent>();
        
        [SerializeField]
        private List<CollisionUnityEvent> onTriggers = new List<CollisionUnityEvent>();

        public void OnAnyEntityEvent(GameEntity entity, EventArgs args)
        {
            switch (args)
            {
                case CollisionEventArgs collisionArgs:
                    var eventsList = collisionArgs.IsTrigger ? onTriggers : onCollisions;
                    
                    foreach (var collisionEvent in eventsList)
                    {
                        if (collisionEvent.IsTagAllowed(collisionArgs.Other.gameObject.tag) && 
                            collisionEvent.IsStatusEqual(collisionArgs.Status))
                        {
                            collisionEvent.Invoke(collisionArgs);
                        }
                    }
                    break;
                default:
                    throw new ArgumentException("EventArgs is empty event! Event call undefined.", nameof(args));
            }
        }
    }
}