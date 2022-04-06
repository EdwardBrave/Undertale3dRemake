using Game.Binding;

namespace Game
{
    public class GameBindingSystems : Feature
    {
        public GameBindingSystems(Contexts contexts)
        {
            Add(new CreateGameEntitySystem(contexts));
            Add(new BindGameEntitySystem(contexts));
            Add(new DisposeCleanupSystem(contexts));
        }
    }
}