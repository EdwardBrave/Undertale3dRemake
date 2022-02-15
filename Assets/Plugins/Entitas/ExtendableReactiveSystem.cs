using System.Collections.Generic;

namespace Entitas
{
    public abstract class ExtendableReactiveSystem<TEntity> : IReactiveSystem, IExecuteSystem, ISystem
        where TEntity : class, IEntity
    {
        private readonly ICollector<TEntity> _collector;
        private readonly List<TEntity> _buffer;
        private string _toStringCache;
      
      protected ExtendableReactiveSystem(IContext<TEntity> context)
    {
      this._collector = this.GetTrigger(context);
      this._buffer = new List<TEntity>();
    }

    protected ExtendableReactiveSystem(ICollector<TEntity> collector)
    {
      this._collector = collector;
      this._buffer = new List<TEntity>();
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
    public virtual void Activate() => this._collector.Activate();

    /// Deactivates the ReactiveSystem.
    ///             No changes will be tracked while deactivated.
    ///             This will also clear the ReactiveSystem.
    ///             ReactiveSystem are activated by default.
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.Deactivate">`ReactiveSystem.Deactivate` on google.com</a></footer>
    public virtual void Deactivate() => this._collector.Deactivate();

    /// Clears all accumulated changes.
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.Clear">`ReactiveSystem.Clear` on google.com</a></footer>
    public virtual void Clear() => this._collector.ClearCollectedEntities();

    /// Will call Execute(entities) with changed entities
    ///             if there are any. Otherwise it will not call Execute(entities).
    ///         <footer><a href="https://www.google.com/search?q=Entitas.ReactiveSystem%601.Execute">`ReactiveSystem.Execute` on google.com</a></footer>
    public virtual void Execute()
    {
      if (this._collector.count == 0)
        return;
      foreach (TEntity collectedEntity in this._collector.collectedEntities)
      {
        if (this.Filter(collectedEntity))
        {
          collectedEntity.Retain((object) this);
          this._buffer.Add(collectedEntity);
        }
      }
      this._collector.ClearCollectedEntities();
      if (this._buffer.Count == 0)
        return;
      try
      {
        this.Execute(this._buffer);
      }
      finally
      {
        for (int index = 0; index < this._buffer.Count; ++index)
          this._buffer[index].Release((object) this);
        this._buffer.Clear();
      }
    }

    public override string ToString()
    {
      if (this._toStringCache == null)
        this._toStringCache = "ReactiveSystem(" + this.GetType().Name + ")";
      return this._toStringCache;
    }

    ~ExtendableReactiveSystem() => this.Deactivate();
    }
}
