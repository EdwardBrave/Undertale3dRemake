//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Game.Reaction.CollisionTriggeredComponent collisionTriggered { get { return (Game.Reaction.CollisionTriggeredComponent)GetComponent(GameComponentsLookup.CollisionTriggered); } }
    public bool hasCollisionTriggered { get { return HasComponent(GameComponentsLookup.CollisionTriggered); } }

    public void AddCollisionTriggered(System.Collections.Generic.List<GameEntity> newEntity) {
        var index = GameComponentsLookup.CollisionTriggered;
        var component = (Game.Reaction.CollisionTriggeredComponent)CreateComponent(index, typeof(Game.Reaction.CollisionTriggeredComponent));
        component.list = newEntity;
        AddComponent(index, component);
    }

    public void ReplaceCollisionTriggered(System.Collections.Generic.List<GameEntity> newEntity) {
        var index = GameComponentsLookup.CollisionTriggered;
        var component = (Game.Reaction.CollisionTriggeredComponent)CreateComponent(index, typeof(Game.Reaction.CollisionTriggeredComponent));
        component.list = newEntity;
        ReplaceComponent(index, component);
    }

    public void RemoveCollisionTriggered() {
        RemoveComponent(GameComponentsLookup.CollisionTriggered);
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

    static Entitas.IMatcher<GameEntity> _matcherCollisionTriggered;

    public static Entitas.IMatcher<GameEntity> CollisionTriggered {
        get {
            if (_matcherCollisionTriggered == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CollisionTriggered);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCollisionTriggered = matcher;
            }

            return _matcherCollisionTriggered;
        }
    }
}
