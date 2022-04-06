using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Game.Common
{
    // Used for defining allowed entities for editing  in the inspector
    [DontGenerate]
    public abstract class GameComponent : IComponent
    {
        
    }
}