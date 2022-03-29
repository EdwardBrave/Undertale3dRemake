using Entitas;
using UnityEngine;

namespace UI.Components
{
    [Ui]
    public class ViewComponent : IComponent
    {
        public GameObject obj;

        public UiEntity parent;
    }
}
