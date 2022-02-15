using Entitas;
using UnityEngine;

namespace Core.UnityScene.Components
{
    [Core]
    public class SceneLoadingComponent : IComponent
    {
        public AsyncOperation operation;
    }
}