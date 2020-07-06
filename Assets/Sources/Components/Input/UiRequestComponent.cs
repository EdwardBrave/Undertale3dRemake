using Entitas;
using UI;

namespace Components.Input
{
    [Input]
    public class UiRequestComponent : IComponent
    {
        public UIHandler sender;
        public UIEventArgs data;
    }
}

