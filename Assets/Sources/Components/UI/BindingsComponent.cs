using System.Collections.Generic;
using Entitas;
using UI;
using UnityEngine;
using Utils;

namespace Components.UI
{
    [Ui] 
    public class BindingsComponent: IComponent
    {
        public UIBinder context;
        public Dictionary<string, Changeable<Sprite>> sprites;
        public Dictionary<string, Changeable<string>> texts;
        public Dictionary<string, Changeable<string>> fields;
    }
}