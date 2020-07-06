using Entitas;
using UnityEngine;

namespace Components.UI
{
    [Ui]
    public class ViewComponent : IComponent
    {
        public GameObject obj;

        public UiEntity canvas;
    }
}
