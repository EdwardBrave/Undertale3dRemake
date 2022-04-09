using System.Collections.Generic;

namespace Entitas.Unity
{
    public abstract class ExtendableReactiveSystem<TEntity> : IReactiveSystem, IExecuteSystem, ISystem
        where TEntity : class, IEntity
    {
        private readonly ICollector<TEntity> _collector;
        private readonly List<TEntity> _buffer;
        private string _toStringCache;
      
      protected ExtendableReactiveSystem(IContext<TEntity> context)
    {
      _collector = GetTrigger(context);
      _buffer = new List<TEntity>();
    }

    protected ExtendableReactiveSystem(ICollector<TEntity> collector)
    {
      _collector = collector;
      _buffer = new List<TEntity>();
    }

    /// <summary>
    /// Manually add some entities to the _collector
    /// </summary>
    protected void AddCollectedEntities(IEnumerable<TEntity> entities)
    {
      foreach (var entity in entities)
      {
        _collector.collectedEntities.Add(entity);
        entity.Retain((object) _collector);
      }
    }

    /// Specify the collector that will trigger the ExtendedReactiveSystem.
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.GetTrigger">`ReactiveSystem.GetTrigger` on google.com</a></footer>
    protected abstract ICollector<TEntity> GetTrigger(IContext<TEntity> context);

    /// This will exclude all entities which don't pass the filter.
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.Filter">`ReactiveSystem.Filter` on google.com</a></footer>
    protected abstract bool Filter(TEntity entity);

    protected abstract void Execute(List<TEntity> entities);

    /// Activates the ReactiveSystem and starts observing changes
    ///             based on the specified Collector.
    ///             ReactiveSystem are activated by default.
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.Activate">`ReactiveSystem.Activate` on google.com</a></footer>
    public virtual void Activate() => _collector.Activate();

    /// Deactivates the ReactiveSystem.
    ///             No changes will be tracked while deactivated.
    ///             This will also clear the ReactiveSystem.
    ///             ReactiveSystem are activated by default.
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.Deactivate">`ReactiveSystem.Deactivate` on google.com</a></footer>
    public virtual void Deactivate() => _collector.Deactivate();

    /// Clears all accumulated changes.
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.Clear">`ReactiveSystem.Clear` on google.com</a></footer>
    public virtual void Clear() => _collector.ClearCollectedEntities();

    /// Will call Execute(entities) with changed entities
    ///             if there are any. Otherwise it will not call Execute(entities).
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.Execute">`ReactiveSystem.Execute` on google.com</a></footer>
    public virtual void Execute()
    {
      if (_collector.count == 0)
        return;
      foreach (TEntity collectedEntity in _collector.collectedEntities)
      {
        if (Filter(collectedEntity))
        {
          collectedEntity.Retain((object) this);
          _buffer.Add(collectedEntity);
        }
      }
      _collector.ClearCollectedEntities();
      if (_buffer.Count == 0)
        return;
      try
      {
        Execute(_buffer);
      }
      finally
      {
        for (int index = 0; index < _buffer.Count; ++index)
          _buffer[index].Release((object) this);
        _buffer.Clear();
      }
    }

    public override string ToString()
    {
      if (_toStringCache == null)
        _toStringCache = "ReactiveSystem(" + GetType().Name + ")";
      return _toStringCache;
    }

    ~ExtendableReactiveSystem() => Deactivate();
    }
}
