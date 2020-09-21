using Entitas;

namespace Logic.Systems.UI
{
    public class UiEventsCleanupSystem: ICleanupSystem
    {
        private readonly UiContext _uiContext;
        private readonly IGroup<UiEntity> _uiEventGroup;

        public UiEventsCleanupSystem(Contexts contexts)
        {
            _uiContext = contexts.ui;
            _uiEventGroup = _uiContext.GetGroup(UiMatcher.AnyOf(UiMatcher.Command));
        }
        
        public void Cleanup()
        {
            foreach (var entity in _uiEventGroup.GetEntities())
            {
                if (entity.hasCommand)
                    entity.RemoveCommand();
            }
        }
    }
}