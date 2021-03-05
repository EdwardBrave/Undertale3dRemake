using Entitas;
using UnityEngine;

namespace Game.Follow
{
    [Game]
    public class FollowComponent: IGameComponent
    {
        public GameObject target;
        public Vector3 relativePosition;
    }
}