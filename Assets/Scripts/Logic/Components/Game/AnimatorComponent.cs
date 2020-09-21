using Entitas;
using UnityEngine;

namespace Logic.Components.Game
{
    [Game]
    public class AnimatorComponent: IComponent
    {
        public Animator value;
    }
}