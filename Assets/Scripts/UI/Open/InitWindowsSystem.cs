using Entitas;
using UI.Data;

namespace UI.Open
{
    public class InitWindowsSystem: IInitializeSystem
    {

        private readonly UiContext _context;
        private readonly UiConfig _uiConfig;

        public InitWindowsSystem(Contexts contexts)
        {
            _context = contexts.ui;
            _uiConfig = _context.uiConfig.value;
        }

        public void Initialize()
        {
            foreach (var window in _uiConfig.startWindows)
            {
                var entity = _context.CreateEntity();
                entity.AddCreateWindow(window);
            }
        }
    }
}