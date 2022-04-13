//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Game.Collision.CollisionsComponent collisions { get { return (Game.Collision.CollisionsComponent)GetComponent(GameComponentsLookup.Collisions); } }
    public bool hasCollisions { get { return HasComponent(GameComponentsLookup.Collisions); } }

    public void AddCollisions(System.Collections.Generic.List<Tools.Temporary<UnityEngine.Collider>> newList) {
        var index = GameComponentsLookup.Collisions;
        var component = (Game.Collision.CollisionsComponent)CreateComponent(index, typeof(Game.Collision.CollisionsComponent));
        component.list = newList;
        AddComponent(index, component);
    }

    public void ReplaceCollisions(System.Collections.Generic.List<Tools.Temporary<UnityEngine.Collider>> newList) {
        var index = GameComponentsLookup.Collisions;
        var component = (Game.Collision.CollisionsComponent)CreateComponent(index, typeof(Game.Collision.CollisionsComponent));
        component.list = newList;
        ReplaceComponent(index, component);
    }

    public void RemoveCollisions() {
        RemoveComponent(GameComponentsLookup.Collisions);
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

    static Entitas.IMatcher<GameEntity> _matcherCollisions;

    public static Entitas.IMatcher<GameEntity> Collisions {
        get {
            if (_matcherCollisions == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Collisions);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCollisions = matcher;
            }

            return _matcherCollisions;
        }
    }
}
