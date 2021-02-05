using Entitas;

namespace Logic.Components.UI
{
    [Ui, Input]
    public class CommandComponent: IComponent
    {
        public string[] args;
    }
}