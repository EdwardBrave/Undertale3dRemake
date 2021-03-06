using Logic.Components.Input;
using Entitas;
using UnityEngine;
using UInput = UnityEngine.Input;

namespace Logic.Systems.Input
{
    public class InputGameEventsSystem: IExecuteSystem, ICleanupSystem
    {
        private readonly InputContext _inputContext;
        private InputEntity _kEventsEntity;
        
        public InputGameEventsSystem(Contexts contexts)
        {
            _inputContext = contexts.input;
            _inputContext.isKeyboardEvents = true;
            _kEventsEntity = _inputContext.keyboardEventsEntity;
        }

        public void Execute()
        {
            if (!UInput.anyKey)
            {
                if (_kEventsEntity.hasKeyPressed)
                {
                    _kEventsEntity.AddKeyUp(_kEventsEntity.keyPressed.key);
                    _kEventsEntity.RemoveKeyPressed();
                }
                return;
            }

            GameKey prevKey = _kEventsEntity.hasKeyPressed ? _kEventsEntity.keyPressed.key : GameKey.None;
            GameKey key = GameKey.None;
            if (UInput.GetKey(KeyCode.W) || UInput.GetKey(KeyCode.UpArrow))
                key |= GameKey.Up;
            if (UInput.GetKey(KeyCode.S) || UInput.GetKey(KeyCode.DownArrow))
                key |= GameKey.Down;
            if (UInput.GetKey(KeyCode.A) || UInput.GetKey(KeyCode.LeftArrow))
                key |= GameKey.Left;
            if (UInput.GetKey(KeyCode.D) || UInput.GetKey(KeyCode.RightArrow))
                key |= GameKey.Right;
            if (UInput.GetKey(KeyCode.Space))
                key |= GameKey.Jump;
            if (UInput.GetKey(KeyCode.F))
                key |= GameKey.Use;
            
            if (key != GameKey.None)
                _kEventsEntity.ReplaceKeyPressed(key);
            else if (_kEventsEntity.hasKeyPressed)
                _kEventsEntity.RemoveKeyPressed();

            GameKey dif = key ^ prevKey;
            if (dif == GameKey.None) 
                return;
            if((dif & key) == GameKey.None)
                _kEventsEntity.AddKeyUp(dif);
            else
                _kEventsEntity.AddKeyDown(dif);
        }

        public void Cleanup()
        {
            if (_kEventsEntity.hasKeyUp)
                _kEventsEntity.RemoveKeyUp();
            if (_kEventsEntity.hasKeyDown)
                _kEventsEntity.RemoveKeyDown();
        }
    }
}