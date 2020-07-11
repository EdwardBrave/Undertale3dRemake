using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        public static event Action<UIHandler, UIEventArgs> UIEvent;
        
        public string identifier;

        public void Confirm() => OnInterfaceEvent(UIEventArgs.Confirm);

        public void Reject() => OnInterfaceEvent(UIEventArgs.Reject);
        
        public void Pressed() => OnInterfaceEvent(UIEventArgs.Pressed);
        
        public void Released() => OnInterfaceEvent(UIEventArgs.Released);

        public void OpenWindow(string windowName) => OnInterfaceEvent(UIEventArgs.Open + " " + windowName);
        
        public void Close(string windows) => OnInterfaceEvent(UIEventArgs.Close + " " + windows);
        public void CloseSelf() => OnInterfaceEvent(UIEventArgs.CloseSelf);
        
        public void CloseAllWindows() => OnInterfaceEvent(UIEventArgs.CloseAll);
        
        public void OnInterfaceEvent(string eventRequest)
        {
            if (string.IsNullOrWhiteSpace(eventRequest)) return;
            string[] args = eventRequest.Split(' ');
            UIEvent?.Invoke(this, new UIEventArgs(identifier, args[0], args.Skip(1).ToArray()));
        }

        public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
        
        public void SendCommand(string command) => OnInterfaceEvent(UIEventArgs.Command + " " + command);
        
        public void SendGlobalCommand(string command) => OnInterfaceEvent(UIEventArgs.GlobalCommand + " " + command);
    }
    
    public class UIEventArgs: EventArgs
    {
        public const string Confirm = "confirm";
        public const string Reject = "reject";
        public const string Pressed = "pressed";
        public const string Released = "released";
        public const string Open = "open";
        public const string Close = "close";
        public const string CloseSelf = "closeSelf";
        public const string CloseAll = "closeAll";
        public const string Command = "aditionalCommand";
        public const string GlobalCommand = "globalAditionalCommand";

        public readonly string identifier;

        public readonly string type;

        public readonly string[] args;

        public UIEventArgs(string identifier, string type, string[] args = null)
        {
            this.identifier = identifier;
            this.type = type;
            this.args = args;
        }
    }
}
