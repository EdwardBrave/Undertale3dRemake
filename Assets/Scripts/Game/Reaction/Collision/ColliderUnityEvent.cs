using System;
using Sirenix.Utilities;
using Tools;
using Tools.TagSelector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Reaction.Collision
{
    [Serializable]
    public abstract class ColliderUnityEvent<T> where T : EventArgs
    {
        [TagSelector]
        [SerializeField]
        private string _tag;

        [SerializeField]
        private TemporaryStatus _status;

        [SerializeField]
        private UnityEvent<T> Event;

        public bool IsTagAllowed(string tag) => _tag.IsNullOrWhitespace() || _tag == tag;
            
        public bool IsStatusEqual(TemporaryStatus status) => _status == status;

        public void Invoke(T args) => Event.Invoke(args);
    }
    
    [Serializable]
    public class CollisionUnityEvent : ColliderUnityEvent<CollisionEventArgs>{}
    
    [Serializable]
    public class TriggerUnityEvent : ColliderUnityEvent<TriggerEventArgs>{}
}