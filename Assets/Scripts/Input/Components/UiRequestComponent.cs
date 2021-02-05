using Entitas;
using UI;
using UI.Binding;

namespace Logic.Components.Input
{
    [Input]
    public class UiRequestComponent : IComponent
    {
        public UIHandler sender;
        public UIEventArgs data;
    }
}

