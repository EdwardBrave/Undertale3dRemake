using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Open
{
    public class InitUiEntity: SerializedMonoBehaviour
    {
        [SerializeField] internal InitUiEntity containerPrefab;
        
        [SerializeField] internal List<IUiComponent> components = new List<IUiComponent>();

        internal void DestroySelf()
        {
            Destroy(this);
        }
    }
}
