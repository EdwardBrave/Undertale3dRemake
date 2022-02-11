using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Main
{
    [Core, Unique]
    public class GameStateComponent: IComponent
    {
        public RegisteredGameState value;
    }
    
    [Core]
    public class ChangeGameStateComponent: IComponent
    {
        public enum StateMode{Main, Additional, Previous}

        public RegisteredGameState value;
        
        public StateMode mode;
    }
}