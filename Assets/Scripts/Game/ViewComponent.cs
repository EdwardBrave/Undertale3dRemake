using UnityEngine;

namespace Game
{
    [Game]
    public class ViewComponent : IGameComponent
    {
        public GameObject obj;

        public Vector3 Position
        {
            get => obj.transform.position;
            set => obj.transform.position = value;
        }
        
        public Vector3 Rotation
        {
            get => obj.transform.eulerAngles;
            set => obj.transform.eulerAngles = value;
        }

        public Vector3 Scale
        {
            get => obj.transform.localScale;
            set => obj.transform.localScale = value;
        }
    }
}
