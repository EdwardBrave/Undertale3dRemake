//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UiEntity {

    public Logic.Components.UI.BindingsComponent bindings { get { return (Logic.Components.UI.BindingsComponent)GetComponent(UiComponentsLookup.Bindings); } }
    public bool hasBindings { get { return HasComponent(UiComponentsLookup.Bindings); } }

    public void AddBindings(UI.Binding.UIBinder newContext) {
        var index = UiComponentsLookup.Bindings;
        var component = (Logic.Components.UI.BindingsComponent)CreateComponent(index, typeof(Logic.Components.UI.BindingsComponent));
        component.context = newContext;
        AddComponent(index, component);
    }

    public void ReplaceBindings(UI.Binding.UIBinder newContext) {
        var index = UiComponentsLookup.Bindings;
        var component = (Logic.Components.UI.BindingsComponent)CreateComponent(index, typeof(Logic.Components.UI.BindingsComponent));
        component.context = newContext;
        ReplaceComponent(index, component);
    }

    public void RemoveBindings() {
        RemoveComponent(UiComponentsLookup.Bindings);
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

    static Entitas.IMatcher<UiEntity> _matcherBindings;

    public static Entitas.IMatcher<UiEntity> Bindings {
        get {
            if (_matcherBindings == null) {
                var matcher = (Entitas.Matcher<UiEntity>)Entitas.Matcher<UiEntity>.AllOf(UiComponentsLookup.Bindings);
                matcher.componentNames = UiComponentsLookup.componentNames;
                _matcherBindings = matcher;
            }

            return _matcherBindings;
        }
    }
}
