using Entitas;
using UnityEngine.UI;

namespace UI.Events
{
    [Ui]
    public class UiEventComponent: IComponent
    {
        public Button sender;

        public string Name => sender.name;
    }
}