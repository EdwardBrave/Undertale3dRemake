using System;
using System.Collections.Generic;
using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic
{
    public class EcsSetuper: SerializedMonoBehaviour
    {
        
        [SerializeField]
        private List<IComponent> components = new List<IComponent>();

        private void OnEnable()
        {
            
        }
    }
}
