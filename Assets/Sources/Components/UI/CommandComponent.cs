using Entitas;

namespace Components.UI
{
    [Ui, Input]
    public class CommandComponent: IComponent
    {
        public string[] args;
    }
}