using Entitas;

namespace Components.Game
{
    [Game]
    public class MotionComponent: IComponent
    {
        public float maxSpeed;
        public float speed;
    }
}