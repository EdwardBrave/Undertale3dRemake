using Entitas;
using UnityEngine;

namespace Logic.Components.Game
{
    [Game]
    public class FollowComponent: IComponent
    {
        public GameEntity target;
        public Vector3 relativePosition;
    }
}