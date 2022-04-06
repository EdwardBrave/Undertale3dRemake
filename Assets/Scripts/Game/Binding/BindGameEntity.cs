using System.Collections.Generic;
using Entitas.Unity;
using Game.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Binding
{
    public class BindGameEntity: SerializedMonoBehaviour
    {
        [SerializeField]
        internal List<GameComponent> components = new List<GameComponent>();

        private void Start()
        {
            if (!gameObject.GetEntityLink())
            {
                var entity = Contexts.sharedInstance.game.CreateEntity();
                entity.AddBindEntity(this);
            }
        }
        
        internal void DestroySelf()
        {
            Destroy(this);
        }
    }
}
