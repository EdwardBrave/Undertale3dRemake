using UnityEngine;

namespace Entitas.Unity
{
    public static class EntitasExtensions
    {
        public static IEntity GetEntityByLink(this GameObject gameObject)
        {
            return gameObject.GetEntityLink()?.entity;
        }
        
        public static IEntity GetEntityByLink(this Component component)
        {
            return component.gameObject.GetEntityLink()?.entity;
        }
    }
}