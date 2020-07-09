using Systems.UI;

namespace Systems.Features
{
    public sealed class UISystems: Feature
    {
        public UISystems(Contexts contexts)
        {
            Add(new CreateCanvasSystem(contexts));
            
            Add(new OpenWindowSystem(contexts));
            Add(new LoadUIDataSystem(contexts));
            Add(new BindingsSystem(contexts));
            
            Add(new CloseWindowCleanupSystem(contexts));
        }
    }
}