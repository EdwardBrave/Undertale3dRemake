namespace Logic.Systems.Features
{
    public sealed class MainMenuSystems : Feature
    {
        public MainMenuSystems(Contexts contexts)
        {
            Add(new UISystems(contexts));
        }
    }
}
