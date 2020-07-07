using System;
using Entitas;
using UnityEngine;

namespace Components.Game
{
    [Serializable][Game]
    public class PositionComponent: IComponent
    {
        public Vector3 value;
    }
}