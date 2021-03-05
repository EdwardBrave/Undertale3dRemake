using Logic.Systems.Input;
using Logic.Systems.UI;
using UI;
using UI.Close;
using UI.Open;

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