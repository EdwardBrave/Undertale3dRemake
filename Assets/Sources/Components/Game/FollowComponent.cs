using Entitas;
using UnityEngine;

namespace Components.Game
{
    [Game]
    public class FollowComponent: IComponent
    {
        public GameEntity target;
        public Vector3 relativePosition;
    }
}