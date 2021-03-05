using Entitas;

namespace Game.Motion
{
    [Game]
    public class MotionComponent: IGameComponent
    {
        public float maxSpeed;
        public float speed;
    }
}