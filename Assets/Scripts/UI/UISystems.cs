using UI.Close;
using UI.Open;

namespace UI
{
    public sealed class UISystems: Feature
    {
        public UISystems(Contexts contexts)
        {
            Add(new CreateWindowSystem(contexts));
            Add(new CloseWindowCleanupSystem(contexts));
        }
    }
}