using Entitas;

namespace UI.Events
{
    [Ui]
    public class ConfirmComponent: IComponent { }
    
    [Ui]
    public class RejectComponent : IComponent { }
    
    [Ui]
    public class CancelComponent : IComponent { }

    [Ui]
    public class PressedComponent : IComponent
    {
        public string name;
    }

    [Ui]
    public class CheckComponent : IComponent
    {
        public bool state;
    }

}