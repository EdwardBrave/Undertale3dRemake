using Bolt;
using Entitas;

namespace UI.Animation
{
    public class WindowAnimationSystem: IExecuteSystem, ICleanupSystem
    {
        private const string OpenWindowEvent = "OnOpenWindow";
        private const string CloseWindowEvent = "OnCloseWindow";

        private readonly ICollector<UiEntity> _openedWindows;
        private readonly ICollector<UiEntity> _closedWindows;

        public WindowAnimationSystem(Contexts contexts)
        {
            var uiContext = contexts.ui;
            _openedWindows = uiContext.CreateCollector(UiMatcher.View);
            _closedWindows = uiContext.CreateCollector(UiMatcher.Close);
        }


        public void Execute()
        {
            foreach (var entity in _openedWindows.collectedEntities)
            {
                if (entity.isAnimation && entity.hasView)
                {
                    CustomEvent.Trigger(entity.view.obj, OpenWindowEvent);
                }
            }
            _openedWindows.ClearCollectedEntities();
        }

        public void Cleanup()
        {
            foreach (var entity in _closedWindows.collectedEntities)
            {
                if (entity.isAnimation && entity.hasView)
                {
                    CustomEvent.Trigger(entity.view.obj, CloseWindowEvent);
                }
            }
            _closedWindows.ClearCollectedEntities();
        }
    }
}