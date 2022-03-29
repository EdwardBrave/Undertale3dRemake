using Data;
using Entitas;

namespace UI
{
    public class UiInitSystem: IInitializeSystem
    {
        private readonly UiContext _uiContext;
        private readonly UiConfig _uiConfig;

        public UiInitSystem(Contexts contexts)
        {
            _uiContext = contexts.ui;
            _uiConfig = contexts.core.globalGameConfigs.value.uiConfig;
        }

        public void Initialize()
        {
            _uiContext.isMainScreen = true;
            _uiContext.mainScreenEntity.AddCreateWindow(_uiConfig.mainScreen, null);
            var entity = _uiContext.CreateEntity();
            entity.AddCreateWindow(_uiConfig.mainMenu, _uiContext.mainScreenEntity);
        }
    }
}