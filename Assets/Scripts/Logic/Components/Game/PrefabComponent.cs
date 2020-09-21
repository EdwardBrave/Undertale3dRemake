using System;
using Entitas;

namespace Logic.Components.Game
{
    [Serializable][Game]
    public class PrefabComponent : IComponent
    {
        public string path;
    }
}
