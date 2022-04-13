using System;
using Sirenix.Utilities;
using Tools;
using Tools.TagSelector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Reaction.Collision
{
    [Serializable]
    public class CollisionEventWrapper
    {
        [Serializable]
        public class CollisionUnityEvent : UnityEvent2<GameEntity, CollisionEventArgs> {}
        
        [TagSelector]
        [SerializeField]
        private string _tag;

        [SerializeField]
        private TemporaryStatus _status;

        [SerializeField]
        private CollisionUnityEvent Event;

        public bool IsTagAllowed(string tag) => _tag.IsNullOrWhitespace() || _tag == tag;
            
        public bool IsStatusEqual(TemporaryStatus status) => _status == status;

        public void Invoke(GameEntity sender, CollisionEventArgs args) => Event?.Invoke(sender, args);
    }
}