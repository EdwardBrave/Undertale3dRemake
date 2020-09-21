using Entitas;
using UI;
using UnityEngine.UI;

namespace Logic.Components.UI
{
    [Ui] 
    public class BindingsComponent: IComponent
    {
        public UIBinder context;

        public Image GetImage(string name) => context.GetImage(name);
        public Text GetText(string name) => context.GetText(name);
        public InputField GetField(string name) => context.GetField(name);
        
        public void LoadUIData(UIData data) => context.LoadUIData(data);
        
    }
}