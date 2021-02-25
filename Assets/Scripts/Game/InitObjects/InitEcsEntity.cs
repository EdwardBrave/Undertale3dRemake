using System.Collections.Generic;
using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.InitObjects
{
    public class InitEcsEntity: SerializedMonoBehaviour
    {
        [SerializeField]
        internal List<IComponent> components = new List<IComponent>();

        private void OnEnable()
        {
            InitFromSceneSystem.AddToInitQueue(components);
            Destroy(this);
        }
    }
}
