using Entitas;
using UnityEngine.SceneManagement;

namespace Core.UnityScene.Components
{
    [Core]
    public class LoadSceneComponent : IComponent
    {
        public string name;

        public LoadSceneMode mode;
    }
}