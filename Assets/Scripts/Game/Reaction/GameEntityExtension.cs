using System;

namespace Game.Reaction
{
    public static class GameEntityExtension
    {
        public static void SendReactionEvent(this GameEntity entity, EventArgs args)
        {
            if (!entity.hasReaction)
            {
                return;
            }
            entity.reaction.SendEvent(entity, args);
        }
    }
}