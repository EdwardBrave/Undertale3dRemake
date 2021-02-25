using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Core.Save
{
    [Core, Unique]
    public class UserDataComponent : IComponent
    {
        public UserData data;
    }
    
    [Core]
    public class SaveUserProgressComponent: IComponent {}
}
