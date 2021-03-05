using Entitas;
using UnityEngine;

namespace UI
{
    [Ui]
    public class ViewComponent : IComponent
    {
        public GameObject obj;

        public UiEntity parent;
    }
}
