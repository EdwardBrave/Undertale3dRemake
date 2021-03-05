using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Game.Reaction
{
    [Game]
    public class ColliderComponent : IGameComponent
    {
        [HideInInspector]
        public List<GameEntity> list = new List<GameEntity>();
    }
}