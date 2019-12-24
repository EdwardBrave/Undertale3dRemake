using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    Feature _systems;

    // Initializes specific systems list on the scene loading 
    private void Awake()
    {
        var contexts = Contexts.sharedInstance;
        contexts.game.DestroyAllEntities();
        _systems = GetNewSystemsList(contexts);
        var e = contexts.game.CreateEntity();
    }
    
    Feature GetNewSystemsList(Contexts contexts) => new MainMenuSystems(contexts);

    // Initializes all systems in the list before the first frame update
    void Start()
    {
        _systems.Initialize();
    }

    // Updates every system in the list once per frame
    void Update()
    {
        _systems.Execute();
    }
    
}