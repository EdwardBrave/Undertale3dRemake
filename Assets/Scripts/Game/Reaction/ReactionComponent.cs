using Bolt;
using Entitas;

namespace Game.Reaction
{
    [Game]
    public class ReactionComponent: IGameComponent
    {
        public FlowMachine onReaction;
    }
}