using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Entitas.Unity;
using Tools.TagSelector;
using UnityEngine;

namespace Game.Reaction
{
    [Serializable][Game]
    public class CollisionComponent: MonoBehaviour, IComponent
    {
        internal GameEntity dependedEntity = null;

        [SerializeField][TagSelector]
        private string[] tagFilters;
        
        private void OnCollisionEnter(Collision other)
        {
            if (dependedEntity == null || !tagFilters.Contains(other.collider.tag))
            {
                return;
            }
            
            if (!(other.gameObject.GetEntityLink()?.entity is GameEntity gameEntity))
            {
                return;
            }

            if (!dependedEntity.hasCollisionTriggered)
            {
                dependedEntity.AddCollisionTriggered(new List<GameEntity>{gameEntity});
            }
            else
            {
                dependedEntity.collisionTriggered.list.Add(gameEntity);
                dependedEntity.ReplaceCollisionTriggered(dependedEntity.collisionTriggered.list);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (dependedEntity == null || !tagFilters.Contains(other.collider.tag) || !dependedEntity.hasCollisionTriggered)
            {
                return;
            }
            
            if (!(other.gameObject.GetEntityLink()?.entity is GameEntity gameEntity))
            {
                return;
            }

            dependedEntity.collisionTriggered.list.Remove(gameEntity);
            
            if (dependedEntity.collisionTriggered.list.Count > 0)
            {
                dependedEntity.ReplaceCollisionTriggered(dependedEntity.collisionTriggered.list);
            }
            else
            {
                dependedEntity.RemoveCollisionTriggered();
            }
        }
    }
}