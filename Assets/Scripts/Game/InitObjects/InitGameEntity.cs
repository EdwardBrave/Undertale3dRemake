using System.Collections.Generic;
using Entitas;
using Game.Motion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.InitObjects
{
    public class InitGameEntity: SerializedMonoBehaviour
    {
        [SerializeField]
        internal List<IGameComponent> components;

        private void OnValidate()
        {
            if (components == null)
            {
                components = new List<IGameComponent>();
            }
            if (components.Count > 0)
            {
                return;
            }
            components.Add(new ViewComponent{obj = gameObject});
        }
        
        private void OnEnable()
        {
            InitFromSceneSystem.AddToInitQueue(components);
            Destroy(this);
        }
    }
}
