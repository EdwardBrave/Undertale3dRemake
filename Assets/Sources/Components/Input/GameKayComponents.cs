using System;
using Entitas;

namespace Components.Input
{
    [Input]
    public class KeyUpComponent: IComponent
    {
        public GameKey key;
    }
    
    [Input]
    public class KeyPressedComponent: IComponent
    {
        public GameKey key;
    }
    
    [Input]
    public class KeyDownComponent: IComponent
    {
        public GameKey key;
    }
    
    [Flags]
    public enum GameKey
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        Jump = 16,
        Use = 32
    }
}