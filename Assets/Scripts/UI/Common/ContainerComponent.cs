using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace UI.Common
{
    [Ui]
    public class ContainerComponent : UiComponent
    {
        [HideInInspector]
        public List<IEntity> windows = new List<IEntity>();
    }
}
