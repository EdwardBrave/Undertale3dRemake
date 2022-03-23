using Input.Processing;

namespace Input
{
    public class InputProcessingSystems: Feature
    {
        public InputProcessingSystems(Contexts contexts)
        {
            Add(new MotionInputProcessingSystem(contexts));
            Add(new UIInputProcessingSystem(contexts));
            Add(new BattleInputProcessingSystem(contexts));
        }
    }
}