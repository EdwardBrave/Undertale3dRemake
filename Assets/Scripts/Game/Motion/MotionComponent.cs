using Entitas;

namespace Game.Motion
{
    [Game]
    public class MotionComponent: IComponent
    {
        public float maxSpeed;
        public float speed;
    }
}