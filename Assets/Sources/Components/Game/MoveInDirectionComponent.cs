using Entitas;
using UnityEngine;

namespace Components.Game
{
    [Game]
    public class MoveInDirectionComponent: IComponent
    {
        public Vector3 vector;
    }
}