using Entitas;
using UnityEngine;

namespace Components.Game
{
    [Game]
    public class AnimatorComponent: IComponent
    {
        public Animator value;
    }
}