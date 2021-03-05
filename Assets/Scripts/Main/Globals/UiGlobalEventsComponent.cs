using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Main.Globals
{
    [Game, Ui, Input, Core, Unique]
    public class GlobalEventsComponent: IComponent { }
}