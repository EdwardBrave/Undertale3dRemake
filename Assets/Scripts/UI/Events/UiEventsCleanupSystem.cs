using Entitas;

namespace UI.Events
{
    public class UiEventsCleanupSystem: ICleanupSystem
    {
        private readonly IGroup<UiEntity> _uiEventGroup;

        public UiEventsCleanupSystem(Contexts contexts)
        {
            _uiEventGroup = contexts.ui.GetGroup(UiMatcher.AnyOf(UiMatcher.UiEvent));
        }
        
        public void Cleanup()
        {
            foreach (var entity in _uiEventGroup.GetEntities())
            {
                if (entity.hasUiEvent)
                {
                    entity.RemoveUiEvent();
                }
            }
        }
    }
}