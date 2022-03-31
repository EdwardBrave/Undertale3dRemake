using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine.Localization;

namespace Core.Localization
{
    [Core, Unique]
    public class LocaleComponent: IComponent
    {
        public LocaleIdentifier identifier;
    }
}