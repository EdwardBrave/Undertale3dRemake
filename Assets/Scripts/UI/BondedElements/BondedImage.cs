using System;
using UnityEngine.UI;

namespace UI.BondedElements
{
    [Serializable]
    public class BondedImage : Bonded<Image>
    {
        public Image Content
        {
            get => content;
            set => content = value;
        }
    
        public BondedImage() : base() { }

        public BondedImage(string name, int depth, int id) : base(name, depth, id) { }
    }
}

