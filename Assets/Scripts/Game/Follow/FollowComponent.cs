using Game.Common;
using UnityEngine;

namespace Game.Follow
{
    [Game]
    public class FollowComponent: GameComponent
    {
        public GameObject target;
        public Vector3 relativePosition;
    }
}