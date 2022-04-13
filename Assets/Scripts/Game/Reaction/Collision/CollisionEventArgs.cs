using System;
using Tools;
using UnityEngine;

namespace Game.Reaction.Collision
{
    [Serializable]
    public class CollisionEventArgs: EventArgs
    {
        public readonly Collider Other;

        public readonly TemporaryStatus Status;

        public readonly bool IsTrigger;

        public CollisionEventArgs(Collider other, TemporaryStatus status, bool isTrigger = false)
        {
            Other = other;
            Status = status;
            IsTrigger = isTrigger;
        }
    }
}