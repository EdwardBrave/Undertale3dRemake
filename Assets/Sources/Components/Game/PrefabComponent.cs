using System;
using Entitas;

namespace Components.Game
{
    [Serializable][Game]
    public class PrefabComponent : IComponent
    {
        public string path;
    }
}
