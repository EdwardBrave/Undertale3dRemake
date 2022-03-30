using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.UiListeners
{
    public static class ButtonExtension
    {
        public static void AddListener(this Button button, Action<Button> action)
        {
            var listener = button.GetComponent<ButtonListener>();
            if (!listener)
            {
                listener = button.gameObject.AddComponent<ButtonListener>();
            }
            listener.AddListener(action);
        }
        
        public static void RemoveListener(this Button button, Action<Button> action)
        {
            var listener = button.GetComponent<ButtonListener>();
            if (listener != null)
            {
                listener.RemoveListener(action);
            }
        }
    }
    
    public class ButtonListener : MonoBehaviour
    {

        private Button _button;
        private Action<Button> _actions;

        public void AddListener(Action<Button> action)
        {
            _actions += action;
        }
        
        public void RemoveListener(Action<Button> action)
        {
            _actions -= action;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnEvent);
        }

        private void OnEvent()
        {
            _actions?.Invoke(_button);
        }
    }
}