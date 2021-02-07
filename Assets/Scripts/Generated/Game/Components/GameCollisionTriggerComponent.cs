//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Logic.Components.Game.CollisionTriggerComponent collisionTrigger { get { return (Logic.Components.Game.CollisionTriggerComponent)GetComponent(GameComponentsLookup.CollisionTrigger); } }
    public bool hasCollisionTrigger { get { return HasComponent(GameComponentsLookup.CollisionTrigger); } }

    public void AddCollisionTrigger(bool newIsTriggered, bool newUseGUILayout, bool newRunInEditMode, bool newEnabled, string newTag, string newName, UnityEngine.HideFlags newHideFlags) {
        var index = GameComponentsLookup.CollisionTrigger;
        var component = (Logic.Components.Game.CollisionTriggerComponent)CreateComponent(index, typeof(Logic.Components.Game.CollisionTriggerComponent));
        component.isTriggered = newIsTriggered;
        component.useGUILayout = newUseGUILayout;
        component.runInEditMode = newRunInEditMode;
        component.enabled = newEnabled;
        component.tag = newTag;
        component.name = newName;
        component.hideFlags = newHideFlags;
        AddComponent(index, component);
    }

    public void ReplaceCollisionTrigger(bool newIsTriggered, bool newUseGUILayout, bool newRunInEditMode, bool newEnabled, string newTag, string newName, UnityEngine.HideFlags newHideFlags) {
        var index = GameComponentsLookup.CollisionTrigger;
        var component = (Logic.Components.Game.CollisionTriggerComponent)CreateComponent(index, typeof(Logic.Components.Game.CollisionTriggerComponent));
        component.isTriggered = newIsTriggered;
        component.useGUILayout = newUseGUILayout;
        component.runInEditMode = newRunInEditMode;
        component.enabled = newEnabled;
        component.tag = newTag;
        component.name = newName;
        component.hideFlags = newHideFlags;
        ReplaceComponent(index, component);
    }

    public void RemoveCollisionTrigger() {
        RemoveComponent(GameComponentsLookup.CollisionTrigger);
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

    static Entitas.IMatcher<GameEntity> _matcherCollisionTrigger;

    public static Entitas.IMatcher<GameEntity> CollisionTrigger {
        get {
            if (_matcherCollisionTrigger == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CollisionTrigger);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCollisionTrigger = matcher;
            }

            return _matcherCollisionTrigger;
        }
    }
}