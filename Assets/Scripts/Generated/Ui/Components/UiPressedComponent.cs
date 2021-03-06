//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UiEntity {

    public UI.Events.PressedComponent pressed { get { return (UI.Events.PressedComponent)GetComponent(UiComponentsLookup.Pressed); } }
    public bool hasPressed { get { return HasComponent(UiComponentsLookup.Pressed); } }

    public void AddPressed(string newName) {
        var index = UiComponentsLookup.Pressed;
        var component = (UI.Events.PressedComponent)CreateComponent(index, typeof(UI.Events.PressedComponent));
        component.name = newName;
        AddComponent(index, component);
    }

    public void ReplacePressed(string newName) {
        var index = UiComponentsLookup.Pressed;
        var component = (UI.Events.PressedComponent)CreateComponent(index, typeof(UI.Events.PressedComponent));
        component.name = newName;
        ReplaceComponent(index, component);
    }

    public void RemovePressed() {
        RemoveComponent(UiComponentsLookup.Pressed);
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
public sealed partial class UiMatcher {

    static Entitas.IMatcher<UiEntity> _matcherPressed;

    public static Entitas.IMatcher<UiEntity> Pressed {
        get {
            if (_matcherPressed == null) {
                var matcher = (Entitas.Matcher<UiEntity>)Entitas.Matcher<UiEntity>.AllOf(UiComponentsLookup.Pressed);
                matcher.componentNames = UiComponentsLookup.componentNames;
                _matcherPressed = matcher;
            }

            return _matcherPressed;
        }
    }
}