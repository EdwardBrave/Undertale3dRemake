using Entitas;

namespace Components.Input
{
    [Input]
    public class KeyPressedComponent: IComponent
    {
        public string key;
    }
}