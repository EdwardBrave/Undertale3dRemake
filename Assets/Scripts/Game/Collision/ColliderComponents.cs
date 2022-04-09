using System.Collections.Generic;
using Entitas;
using Game.Common;
using Tools;
using UnityEngine;

namespace Game.Collision
{
    [Game]
    public class ColliderComponent : GameComponent
    {
        public CollisionListener listener;
    }
    
    [Game]
    public class CollisionsComponent : IComponent
    {
        public List<Temporary<UnityEngine.Collision>> list;
    }
    
    [Game]
    public class TriggersComponent : IComponent
    {
        public List<Temporary<Collider>> list;
    }
}