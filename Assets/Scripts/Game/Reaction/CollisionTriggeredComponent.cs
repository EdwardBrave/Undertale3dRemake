using System.Collections.Generic;
using Entitas;

namespace Game.Reaction
{
    [Game]
    public class CollisionTriggeredComponent : IComponent
    {
        public List<GameEntity> list;
    }
}