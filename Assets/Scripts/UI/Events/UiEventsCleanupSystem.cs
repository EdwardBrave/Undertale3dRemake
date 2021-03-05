using Entitas;

namespace UI.Events
{
    public class UiEventsCleanupSystem: ICleanupSystem
    {
        private readonly IGroup<UiEntity> _uiEventGroup;

        public UiEventsCleanupSystem(Contexts contexts)
        {
            _uiEventGroup = contexts.ui.GetGroup(UiMatcher.AnyOf(UiMatcher.Confirm, UiMatcher.Reject,
                UiMatcher.Cancel, UiMatcher.Pressed, UiMatcher.Check));
        }
        
        public void Cleanup()
        {
            foreach (var entity in _uiEventGroup.GetEntities())
            {
                entity.isConfirm = false;
                entity.isReject = false;
                entity.isCancel = false;
                if (entity.hasPressed)
                {
                    entity.RemovePressed();
                }
                if (entity.hasCheck)
                {
                    entity.RemoveCheck();
                }
            }
        }
    }
}