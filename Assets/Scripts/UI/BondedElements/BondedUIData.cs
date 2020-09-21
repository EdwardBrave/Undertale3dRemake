using System;

namespace UI.BondedElements
{
    [Serializable]
    public class BondedUIData : Bonded<UIData>
    {
        public UIData Content
        {
            get => content;
            set => content = value;
        }
    
        public BondedUIData() : base() { }

        public BondedUIData(string name, int depth, int id) : base(name, depth, id) { }
    }
}