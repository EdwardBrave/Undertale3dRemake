using Entitas;
using Entitas.VisualDebugging.Unity;

namespace Main
{
    public abstract class GameState: Feature
    {
        ////////////////////////////////////////////////////////////////////
        #region Data and Properties

        private Contexts _contexts;
    
        private bool _isActive = false;

        public bool IsActive => _isActive;
    
        protected Contexts Contexts => _contexts;
    
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Interface

        public void InitSystems(Contexts contexts)
        {
            _contexts = contexts;
            var systems = GetSystems(contexts);
            foreach (var system in systems)
            {
                Add(system);
            }
            Initialize();
        }
    
        protected abstract ISystem[] GetSystems(Contexts contexts);
    
        public sealed override Systems Add(ISystem system) => base.Add(system);

        public sealed override void Execute() => base.Execute();
    
        public sealed override void Cleanup() => base.Cleanup();

        public virtual void Activate()
        {
            ActivateReactiveSystems();
            _isActive = true;
        }

        public virtual void Deactivate()
        {
            DeactivateReactiveSystems();
            _isActive = false;
        }
    
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
        public override void Initialize()
        {
            base.Initialize();
            var components = UnityEngine.Object.FindObjectsOfType<DebugSystemsBehaviour>();
            foreach(var component in components)
            {
                if (component.systems == this)
                {
                    UnityEngine.Object.DontDestroyOnLoad(component.gameObject);
                }
            }
        }
    
        public override void TearDown()
        {
            base.TearDown();
            var components = UnityEngine.Object.FindObjectsOfType<DebugSystemsBehaviour>();
            foreach(var component in components)
            {
                if (component.systems == this)
                {
                    UnityEngine.Object.Destroy(component.gameObject);
                }
            }
        }
#endif
    
        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}