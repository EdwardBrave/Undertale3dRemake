using Core.Data;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Logic.Components.Core
{
    [Core, Unique]
    public class UserDataComponent : IComponent
    {
        public UserData data;
    }
    
    [Core]
    public class UpdateUserProgressComponent: IComponent {}
}
