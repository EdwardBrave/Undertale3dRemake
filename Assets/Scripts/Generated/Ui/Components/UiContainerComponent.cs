//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class UiEntity {

    public UI.Common.ContainerComponent container { get { return (UI.Common.ContainerComponent)GetComponent(UiComponentsLookup.Container); } }
    public bool hasContainer { get { return HasComponent(UiComponentsLookup.Container); } }

    public void AddContainer(System.Collections.Generic.List<Entitas.IEntity> newWindows) {
        var index = UiComponentsLookup.Container;
        var component = (UI.Common.ContainerComponent)CreateComponent(index, typeof(UI.Common.ContainerComponent));
        component.windows = newWindows;
        AddComponent(index, component);
    }

    public void ReplaceContainer(System.Collections.Generic.List<Entitas.IEntity> newWindows) {
        var index = UiComponentsLookup.Container;
        var component = (UI.Common.ContainerComponent)CreateComponent(index, typeof(UI.Common.ContainerComponent));
        component.windows = newWindows;
        ReplaceComponent(index, component);
    }

    public void RemoveContainer() {
        RemoveComponent(UiComponentsLookup.Container);
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

    static Entitas.IMatcher<UiEntity> _matcherContainer;

    public static Entitas.IMatcher<UiEntity> Container {
        get {
            if (_matcherContainer == null) {
                var matcher = (Entitas.Matcher<UiEntity>)Entitas.Matcher<UiEntity>.AllOf(UiComponentsLookup.Container);
                matcher.componentNames = UiComponentsLookup.componentNames;
                _matcherContainer = matcher;
            }

            return _matcherContainer;
        }
    }
}
