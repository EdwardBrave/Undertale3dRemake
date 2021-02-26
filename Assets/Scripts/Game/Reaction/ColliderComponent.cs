using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Game.Reaction
{
    [Game]
    public class ColliderComponent : IComponent
    {
        [HideInInspector]
        public List<GameEntity> list = new List<GameEntity>();
    }
}