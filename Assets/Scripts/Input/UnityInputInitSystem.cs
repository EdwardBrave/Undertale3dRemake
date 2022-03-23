using Entitas;

namespace Input
{
    public class UnityInputInitSystem: IInitializeSystem
    {
        private readonly InputContext _context;

        public UnityInputInitSystem(Contexts contexts)
        {
            _context = contexts.input;
        }

        public void Initialize()
        {
            var controls = new GameControls();
            _context.SetInputControls(controls);
            _context.SetMotionInput(controls.Motion);
            _context.SetUiInput(controls.UI);
            _context.SetBattleInput(controls.Battle);
        }
    }
}