using System;
using UnityEngine;

namespace UI.BondedElements
{
    [Serializable]
    public class BondedSprite : Bonded<Sprite>
    {
        public Sprite Content
        {
            get => content;
            set => content = value;
        }
    
        public BondedSprite() : base() { }

        public BondedSprite(string name, int depth, int id) : base(name, depth, id) { }
    }
}

