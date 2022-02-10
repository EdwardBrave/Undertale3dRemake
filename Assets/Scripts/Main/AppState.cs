using Entitas;

public abstract class AppState: Feature
{
    ////////////////////////////////////////////////////////////////////
    #region Data

    protected readonly Contexts contexts;
    
    #endregion
    ////////////////////////////////////////////////////////////////////
    #region Interface
    
    public AppState(Contexts newContexts)
    {
        contexts = newContexts;
        var systems = GetSystems(contexts);
        foreach (var system in systems)
        {
            Add(system);
        }
    }
    
    protected abstract ISystem[] GetSystems(Contexts contexts);
    
    protected virtual void Init() { }
    protected virtual void Activate() { }
    protected virtual void Deactivate() { }
    protected virtual void Destroy() { }

    #endregion
    ////////////////////////////////////////////////////////////////////
    #region Feature implementation
    
    public sealed override Systems Add(ISystem system) => base.Add(system);

    public sealed override void Initialize()
    {
        Init();
        base.Initialize();
    }

    public sealed override void Execute() => base.Execute();
    
    public sealed override void Cleanup() => base.Cleanup();
    
    public sealed override void TearDown()
    {
        base.TearDown();
        Destroy();
    }

    #endregion
    ////////////////////////////////////////////////////////////////////
}