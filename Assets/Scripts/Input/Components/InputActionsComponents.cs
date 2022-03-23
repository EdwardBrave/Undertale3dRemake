using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Input.Components
{
    [Input, Unique]
    public class InputControlsComponent: IComponent
    {
        public GameControls value;
    }
    
    [Input, Unique]
    public class MotionInputComponent: IComponent
    {
        public GameControls.MotionActions actions;
    }
    
    [Input, Unique]
    public class UiInputComponent: IComponent
    {
        public GameControls.UIActions actions;
    }
    
    [Input, Unique]
    public class BattleInputComponent: IComponent
    {
        public GameControls.BattleActions actions;
    }
}