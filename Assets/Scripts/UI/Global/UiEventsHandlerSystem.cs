using Entitas;

namespace UI.Global
{
    public class UiEventsHandlerSystem: IExecuteSystem, ITearDownSystem
    {
        private readonly UiContext _uiContext;
        private readonly ICollector<UiEntity> _closeAllEvents;
        private readonly ICollector<UiEntity> _openWindowEvents;
        private readonly ICollector<UiEntity> _closeWindowEvents;

        public UiEventsHandlerSystem(Contexts contexts)
        {
            _uiContext = contexts.ui;
            _closeAllEvents = _uiContext.CreateCollector(UiMatcher.CloseAll);
            _openWindowEvents = _uiContext.CreateCollector(UiMatcher.OpenWindow);
            _closeWindowEvents = _uiContext.CreateCollector(UiMatcher.CloseWindows);
        }
        
        public void Execute()
        {
            foreach(var entity in _openWindowEvents.collectedEntities)
            {
                if (entity.isGlobalEvents)
                {
                    
                }
                entity.RemoveOpenWindow();
            }
            _openWindowEvents.ClearCollectedEntities();

            TearDown();
        }

        public void TearDown()
        {
            foreach(var entity in _closeAllEvents.collectedEntities)
            {
                if (entity.isGlobalEvents)
                {
                    foreach (var viewEntity in _uiContext.GetEntities(UiMatcher.View))
                    {
                        if (viewEntity.view.parent != null)
                        {
                            viewEntity.ReplaceClose(entity.closeAll.isForce);
                        }
                    }
                }
                entity.RemoveCloseAll();
            } 
            _closeAllEvents.ClearCollectedEntities();
            
            foreach(var entity in _closeWindowEvents.collectedEntities)
            {
                if (entity.isGlobalEvents)
                {
                    foreach(var viewEntity in _uiContext.GetEntities(UiMatcher.View))
                    {
                        if (viewEntity.view.obj.name == entity.closeWindows.data.name)
                        {
                            viewEntity.ReplaceClose(entity.closeWindows.isForce);
                        }
                    }
                }
                entity.RemoveCloseWindows();
            }
            _closeWindowEvents.ClearCollectedEntities();
        }
    }
}