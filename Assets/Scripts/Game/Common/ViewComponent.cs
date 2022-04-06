using Entitas;
using UnityEngine;

namespace Game.Common
{
    [Game]
    public class ViewComponent : IComponent
    {
        public GameObject obj;

        public Transform transform => obj.transform;
    }
}
