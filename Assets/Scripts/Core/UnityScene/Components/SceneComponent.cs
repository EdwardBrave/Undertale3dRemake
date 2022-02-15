using Entitas;
using UnityEngine.SceneManagement;

namespace Core.UnityScene.Components
{
    [Core]
    public class SceneComponent : IComponent
    {
        public Scene value;
    }
}