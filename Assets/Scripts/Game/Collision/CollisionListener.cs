using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tools.TagSelector;
using UnityEngine;

namespace Game.Collision
{
    public class CollisionListener: MonoBehaviour
    {
        [SerializeField]
        private bool isInverted = true;
        
        [Title("@isInverted ? \"Black List\" : \"White List\"")]
        [TagSelector, ListDrawerSettings(Expanded = true)]
        [SerializeField]
        private List<string> tagsList = new List<string>();

        public event Action<CollisionListener, Collider> TriggerEnter;
        public event Action<CollisionListener, Collider> TriggerExit;
        public event Action<CollisionListener, UnityEngine.Collision> CollisionEnter;
        public event Action<CollisionListener, UnityEngine.Collision> CollisionExit;

        private bool IsAllowed(GameObject other)
        {
            return isInverted != tagsList.Contains(other.tag);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsAllowed(other.gameObject))
            {
                TriggerEnter?.Invoke(this, other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsAllowed(other.gameObject))
            {
                TriggerExit?.Invoke(this, other);
            }
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (IsAllowed(other.gameObject))
            {
                CollisionEnter?.Invoke(this, other);
            }
        }

        private void OnCollisionExit(UnityEngine.Collision other)
        {
            if (IsAllowed(other.gameObject))
            {
                CollisionExit?.Invoke(this, other);
            }
        }
    }
}