using System.Collections.Generic;
using Entitas;
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
using Entitas.VisualDebugging.Unity;
#endif
using UnityEngine;

namespace UI.Common
{
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
    [DontDrawComponent]
#endif
    [Ui]
    public class ContainerComponent : UiComponent
    {
        [HideInInspector]
        public List<IEntity> windows = new List<IEntity>();
    }
}
