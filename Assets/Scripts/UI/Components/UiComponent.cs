using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UI.Components
{
    // Used for defining allowed entities for editing in the inspector
    [DontGenerate]
    public abstract class UiComponent : IComponent
    {
    }
}