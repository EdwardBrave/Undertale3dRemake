using System;

namespace UI.BondedElements
{
    [Serializable]
    public class BondedString : Bonded<string>
    {
        public string Content
        {
            get => content;
            set => content = value;
        }
    
        public BondedString() : base() { }

        public BondedString(string name, int depth, int id) : base(name, depth, id) { }
    }
}