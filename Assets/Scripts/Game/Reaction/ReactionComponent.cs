using Bolt;
using Entitas;

namespace Game.Reaction
{
    [Game]
    public class ReactionComponent: IComponent
    {
        public FlowMachine onReaction;
    }
}