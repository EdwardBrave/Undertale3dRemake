using System.Collections.Generic;
using Entitas;

namespace Logic.Components.UI
{
    [Ui]
    public class CanvasComponent : IComponent
    {
        public string name;
        public List<UiEntity> windows;
    }
}
