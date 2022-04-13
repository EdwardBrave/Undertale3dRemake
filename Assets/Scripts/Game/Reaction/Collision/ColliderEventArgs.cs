using System;
using Tools;
using UnityEngine;

namespace Game.Reaction.Collision
{
    public class CollisionEventArgs: EventArgs
    {
        public readonly UnityEngine.Collision Other;

        public readonly TemporaryStatus Status;

        public CollisionEventArgs(UnityEngine.Collision other, TemporaryStatus status)
        {
            Other = other;
            Status = status;
        }
    }
    
    public class TriggerEventArgs: EventArgs
    {
        public readonly Collider Other;

        public readonly TemporaryStatus Status;

        public TriggerEventArgs(Collider other, TemporaryStatus status)
        {
            Other = other;
            Status = status;
        }
    }
}