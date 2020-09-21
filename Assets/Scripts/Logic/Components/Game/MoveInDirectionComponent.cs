using Entitas;
using UnityEngine;

namespace Logic.Components.Game
{
    [Game]
    public class MoveInDirectionComponent: IComponent
    {
        public Vector3 vector;
    }
}