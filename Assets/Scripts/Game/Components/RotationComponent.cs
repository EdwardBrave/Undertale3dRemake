using Entitas;
using UnityEngine;

namespace Logic.Components.Game
{
    [Game]
    public class RotationComponent: IComponent
    {
        public Vector3 euler;
        public bool isRelative;
    }
}