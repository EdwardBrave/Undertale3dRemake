using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    Feature _systems;

    // Initializes specific systems list on the scene loading 
    private void Awake()
    {
        var contexts = Contexts.sharedInstance;
        _systems = GetNewSystemsList(contexts);
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