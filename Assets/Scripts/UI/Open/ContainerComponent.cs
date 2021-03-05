using System.Collections.Generic;
using Entitas;

namespace UI.Open
{
    [Ui]
    public class ContainerComponent : IComponent
    {
        public List<UiEntity> windows;
    }
}
