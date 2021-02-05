using Entitas;
using TMPro;
using UI;
using UI.Binding;
using UnityEngine.UI;

namespace Logic.Components.UI
{
    [Ui] 
    public class BindingsComponent: IComponent
    {
        public UIBinder context;

        public Image GetImage(string name) => context.GetImage(name);
        public TMP_Text GetText(string name) => context.GetText(name);
        public TMP_InputField GetField(string name) => context.GetField(name);
        public ListLayout GetListLayout(string name) => context.GetListLayout(name);
        
        public Toggle GetToggle(string name) => context.GetToggle(name);
        
        public void LoadUIData(UIData data) => context.LoadUIData(data);
        
    }
}