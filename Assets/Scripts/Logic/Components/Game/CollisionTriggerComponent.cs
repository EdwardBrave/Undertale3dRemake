using System;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Logic.Components.Game
{
    [Serializable][Game]
    public class CollisionTriggerComponent: MonoBehaviour, IComponent
    {
        [HideInInspector] public bool isTriggered = false;

        [SerializeField][TagSelector]
        private string[] tagFilters;
        

        private void OnCollisionEnter(Collision other)
        {
            if (tagFilters.Contains(other.collider.tag))
            {
                isTriggered = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (tagFilters.Contains(other.collider.tag))
            {
                isTriggered = false;
            }
        }
    }
}