using Entitas;
using UnityEngine;
using UInput = UnityEngine.Input;

namespace Systems.Input
{
    public class KeyboardEventsSystem: IExecuteSystem, IInitializeSystem
    {
        private readonly InputContext _inputContext;
        
        public KeyboardEventsSystem(Contexts contexts)
        {
            _inputContext = contexts.input;
        }

        public void Initialize()
        {
            _inputContext.isKeyboardEvents = true;
        }

        public void Execute()
        {
            if (UInput.GetKey(KeyCode.F))
                Debug.Log("F is here");
            if (UInput.anyKeyDown && UInput.inputString !="")
                _inputContext.keyboardEventsEntity.ReplaceKeyPressed(UInput.inputString);
            else if (!UInput.anyKey && _inputContext.keyboardEventsEntity.hasKeyPressed)
                _inputContext.keyboardEventsEntity.RemoveKeyPressed();
            
        }
    }
}