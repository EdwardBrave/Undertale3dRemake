using System.Linq;
using Entitas.Unity;
using Tools.TagSelector;
using UnityEngine;

namespace Game.Reaction
{
    public class CollisionListener: MonoBehaviour
    {

        [SerializeField][TagSelector]
        private string[] tagFilters;

        private void OnTriggerEnter(Collider other)
        {
            OnEnterEvent(other.gameObject);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            OnEnterEvent(other.gameObject);
        }
        
        private void OnTriggerExit(Collider other)
        {
            OnExitEvent(other.gameObject);
        }
        
        private void OnCollisionExit(Collision other)
        {
            OnExitEvent(other.gameObject);
        }

        private void OnEnterEvent(GameObject other)
        {
            var currentEntity = gameObject.GetEntityLink()?.entity as GameEntity;
            if (currentEntity == null || !currentEntity.hasCollider || !tagFilters.Contains(other.tag))
            {
                return;
            }
            
            if (!(other.GetEntityLink()?.entity is GameEntity gameEntity))
            {
                return;
            }
            
            currentEntity.collider.list.Add(gameEntity);
            currentEntity.ReplaceCollider(currentEntity.collider.list);
        }

        private void OnExitEvent(GameObject other)
        {
            var currentEntity = gameObject.GetEntityLink()?.entity as GameEntity;
            if (currentEntity == null || !currentEntity.hasCollider || !tagFilters.Contains(other.tag))
            {
                return;
            }
            
            if (!(other.GetEntityLink()?.entity is GameEntity gameEntity))
            {
                return;
            }

            currentEntity.collider.list.Remove(gameEntity);
            currentEntity.ReplaceCollider(currentEntity.collider.list);
        }
    }
}