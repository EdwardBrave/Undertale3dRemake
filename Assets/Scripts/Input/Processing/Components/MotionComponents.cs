using Entitas;
using UnityEngine;

namespace Input.Processing.Components
{
    [Input]
    public class MoveComponent: IComponent
    {
        public Vector2 axes;
    }
    
    [Input]
    public class LookComponent: IComponent
    {
        public Vector2 axes;
    }
}