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
            Add(new LoadUIDataSystem(contexts));
            Add(new UIEventHandlerSystem(contexts));
            Add(new UIHandlerSystem(contexts));

            Add(new UIRequestsCleanupSystem(contexts));
            Add(new CloseWindowCleanupSystem(contexts));
        }
    }
}