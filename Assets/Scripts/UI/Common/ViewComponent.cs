using Entitas;
using UnityEngine;

namespace UI.Common
{
    [Ui]
    public class ViewComponent : IComponent
    {
        public GameObject obj;

        public UiEntity parent;
    }
}
