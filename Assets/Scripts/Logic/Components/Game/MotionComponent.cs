using Entitas;

namespace Logic.Components.Game
{
    [Game]
    public class MotionComponent: IComponent
    {
        public float maxSpeed;
        public float speed;
    }
}