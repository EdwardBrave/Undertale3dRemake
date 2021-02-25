//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Game.Reaction.ReactionComponent reaction { get { return (Game.Reaction.ReactionComponent)GetComponent(GameComponentsLookup.Reaction); } }
    public bool hasReaction { get { return HasComponent(GameComponentsLookup.Reaction); } }

    public void AddReaction(Bolt.FlowMachine newOnReaction) {
        var index = GameComponentsLookup.Reaction;
        var component = (Game.Reaction.ReactionComponent)CreateComponent(index, typeof(Game.Reaction.ReactionComponent));
        component.onReaction = newOnReaction;
        AddComponent(index, component);
    }

    public void ReplaceReaction(Bolt.FlowMachine newOnReaction) {
        var index = GameComponentsLookup.Reaction;
        var component = (Game.Reaction.ReactionComponent)CreateComponent(index, typeof(Game.Reaction.ReactionComponent));
        component.onReaction = newOnReaction;
        ReplaceComponent(index, component);
    }

    public void RemoveReaction() {
        RemoveComponent(GameComponentsLookup.Reaction);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherReaction;

    public static Entitas.IMatcher<GameEntity> Reaction {
        get {
            if (_matcherReaction == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Reaction);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherReaction = matcher;
            }

            return _matcherReaction;
        }
    }
}
