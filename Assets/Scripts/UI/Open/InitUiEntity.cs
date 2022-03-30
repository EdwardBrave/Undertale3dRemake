using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI.Common;
using UnityEngine;

namespace UI.Open
{
    public class InitUiEntity: SerializedMonoBehaviour
    {
        [SerializeField] internal List<UiComponent> components = new List<UiComponent>();

        internal void DestroySelf()
        {
            Destroy(this);
        }
    }
}
