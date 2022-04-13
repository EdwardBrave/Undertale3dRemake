using System;
using Sirenix.Utilities;
using Tools;
using Tools.TagSelector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Reaction.Collision
{
    [Serializable]
    public abstract class CollisionUnityEvent : EventArgs
    {
        [TagSelector]
        [SerializeField]
        private string _tag;

        [SerializeField]
        private TemporaryStatus _status;

        [SerializeField]
        private UnityEvent<CollisionEventArgs> Event;

        public bool IsTagAllowed(string tag) => _tag.IsNullOrWhitespace() || _tag == tag;
            
        public bool IsStatusEqual(TemporaryStatus status) => _status == status;

        public void Invoke(CollisionEventArgs args) => Event.Invoke(args);
    }
}