using System.Collections.Generic;
using Entitas;
using Game.Common;
using UnityEngine;

namespace Game.Reaction
{
    [Game]
    public class ColliderComponent : GameComponent
    {
        [HideInInspector]
        public List<GameEntity> list = new List<GameEntity>();
    }
}