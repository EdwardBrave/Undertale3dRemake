using Entitas;
using UnityEngine;

namespace Game.Follow
{
    [Game]
    public class FollowComponent: IComponent
    {
        public GameObject target;
        public Vector3 relativePosition;
    }
}