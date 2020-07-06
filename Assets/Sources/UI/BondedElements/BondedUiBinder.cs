using System;

namespace UI.BondedElements
{
    [Serializable]
    public class BondedUIBinder : Bonded<UIBinder>
    {
        public UIBinder Content
        {
            get => content;
            set => content = value;
        }
    
        public BondedUIBinder() : base() { }

        public BondedUIBinder(string name, int depth, int id) : base(name, depth, id) { }
    }
}

