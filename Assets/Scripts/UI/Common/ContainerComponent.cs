using System.Collections.Generic;
using UnityEngine;

namespace UI.Common
{
    [Ui]
    public class ContainerComponent : UiComponent
    {
        [HideInInspector]
        public List<UiEntity> windows = new List<UiEntity>();
    }
}
