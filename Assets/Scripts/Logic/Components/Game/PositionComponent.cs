using System;
using Entitas;
using UnityEngine;

namespace Logic.Components.Game
{
    [Serializable][Game]
    public class PositionComponent: IComponent
    {
        public Vector3 value;
        public bool isRelative;
    }
}