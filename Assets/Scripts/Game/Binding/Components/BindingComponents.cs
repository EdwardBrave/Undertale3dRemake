using Entitas;

namespace Game.Binding.Components
{
    [Game]
    public class CreateEntityComponent: IComponent
    {
        public BindGameEntity prefab;
    }
    
    [Game]
    public class BindEntityComponent: IComponent
    {
        public BindGameEntity binding;
    }
    
    
}