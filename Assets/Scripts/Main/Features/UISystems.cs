using Logic.Systems.Input;
using Logic.Systems.UI;
using UI;

namespace Main.Features
{
    public sealed class UISystems: Feature
    {
        public UISystems(Contexts contexts)
        {
            Add(new CreateCanvasSystem(contexts));
            
            Add(new OpenWindowSystem(contexts));

            Add(new CloseWindowCleanupSystem(contexts));
        }
    }
}