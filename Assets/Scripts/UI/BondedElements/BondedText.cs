using System;
using UnityEngine.UI;

namespace UI.BondedElements
{
    [Serializable]
    public class BondedText : Bonded<Text>
    {
        public Text Content
        {
            get => content;
            set => content = value;
        }

        public BondedText() { }

        public BondedText(string name, int depth, int id) : base(name, depth, id) { }
    }
}