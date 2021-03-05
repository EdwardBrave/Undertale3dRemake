using System;
using System.Collections.Generic;
using Entitas.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Common
{
    public class ExtendUiOnInit: SerializedMonoBehaviour
    {
        [SerializeField]
        internal List<IUiComponent> components;

        private void Update()
        {
            var link = gameObject.GetEntityLink();
            if (!link || !(link.entity is UiEntity entity))
            {
                return;
            }
            foreach (var component in components)
            {
                int index = Array.IndexOf(UiComponentsLookup.componentTypes, component.GetType());
                entity.AddComponent(index, component);
            }
            Destroy(this);
        }
    }
}
