using UI.Close;
using UI.Events;
using UI.Global;
using UI.Open;

namespace Main.GameStates
{
    public sealed class UISystems: Feature
    {
        public UISystems(Contexts contexts)
        {
            Add(new InitWindowsSystem(contexts));

            Add(new UiEventsHandlerSystem(contexts));
            Add(new CreateWindowSystem(contexts));
            Add(new UiEventsCleanupSystem(contexts));
            Add(new CloseWindowCleanupSystem(contexts));
        }
    }
}