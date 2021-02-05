using Entitas;
using UnityEngine;

namespace Logic.Components.Game
{
    [Game]
    public class ViewComponent : IComponent
    {
        public GameObject obj;

        public Vector3 Position => obj.transform.position;

        public Vector3 Rotation => obj.transform.eulerAngles;

        public Vector3 Scale => obj.transform.localScale;
        
    }
}
