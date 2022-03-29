
using UI;

namespace Main.GameStates
{
    public sealed class MainMenuSystems : Feature
    {
        public MainMenuSystems(Contexts contexts)
        {
            Add(new UISystems(contexts));
        }
    }
}
