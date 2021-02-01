using System;
using Entitas;
using UnityEngine.Events;

namespace Logic.Components.Game
{
    [Serializable][Game]
    public class ReactionComponent: IComponent
    {
        public UnityEvent onReaction;
    }
}