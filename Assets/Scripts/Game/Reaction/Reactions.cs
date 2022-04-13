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
        private List<TriggerUnityEvent> onTriggers = new List<TriggerUnityEvent>();

        public void OnAnyEntityEvent(GameEntity entity, EventArgs args)
        {
            switch (args)
            {
                case CollisionEventArgs collisionArgs:
                    foreach (var collisionEvent in onCollisions)
                    {
                        if (collisionEvent.IsTagAllowed(collisionArgs.Other.gameObject.tag) && 
                            collisionEvent.IsStatusEqual(collisionArgs.Status))
                        {
                            collisionEvent.Invoke(collisionArgs);
                        }
                    }
                    break;
                case TriggerEventArgs triggerArgs:
                    foreach (var triggerEvent in onTriggers)
                    {
                        if (triggerEvent.IsTagAllowed(triggerArgs.Other.tag) && 
                            triggerEvent.IsStatusEqual(triggerArgs.Status))
                        {
                            triggerEvent.Invoke(triggerArgs);
                        }
                    }
                    break;
                default:
                    throw new ArgumentException("EventArgs is empty event! Event call undefined.", nameof(args));
            }
        }
    }
}