using Entitas;

namespace Logic.Components.Game
{
    [Game]
    public class SceneObjectComponent : IComponent
    {
        public string name;
        public bool isTag = false;
    }
}
