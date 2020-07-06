using System;
using UnityEngine.UI;

namespace UI.BondedElements
{
    [Serializable]
    public class BondedField : Bonded<InputField>
    {
        public InputField Content
        {
            get => content;
            set => content = value;
        }
    
        public BondedField() : base() { }

        public BondedField(string name, int depth, int id) : base(name, depth, id) { }
    }
}