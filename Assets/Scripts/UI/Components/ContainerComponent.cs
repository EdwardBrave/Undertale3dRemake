using System.Collections.Generic;
using UnityEngine;

namespace UI.Components
{
    [Ui]
    public class ContainerComponent : UiComponent
    {
        [HideInInspector]
        public List<UiEntity> windows = new List<UiEntity>();
    }
}
