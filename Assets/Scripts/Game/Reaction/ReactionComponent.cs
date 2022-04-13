using System;
using Game.Common;

namespace Game.Reaction
{
    [Game]
    public class ReactionComponent : GameComponent
    {
        public Reactions obj;

        public void SendEvent(GameEntity sender, EventArgs args)
        {
            obj.OnAnyEntityEvent(sender, args);
        }
    }
}