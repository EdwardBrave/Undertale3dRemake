using UI.Animation;
using UI.Close;
using UI.Events;
using UI.Global;
using UI.Open;

namespace Main.Features
{
    public sealed class UISystems: Feature
    {
        public UISystems(Contexts contexts)
        {
            Add(new InitWindowsSystem(contexts));

            Add(new UiEventsHandlerSystem(contexts));
            Add(new CreateWindowSystem(contexts));

            Add(new WindowAnimationSystem(contexts));
            Add(new UiEventsCleanupSystem(contexts));
            Add(new CloseWindowCleanupSystem(contexts));
        }
    }
}