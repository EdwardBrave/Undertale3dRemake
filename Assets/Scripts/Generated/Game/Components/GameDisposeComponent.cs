//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Game.Binding.Components.DisposeComponent dispose { get { return (Game.Binding.Components.DisposeComponent)GetComponent(GameComponentsLookup.Dispose); } }
    public bool hasDispose { get { return HasComponent(GameComponentsLookup.Dispose); } }

    public void AddDispose(System.Collections.Generic.List<Game.Binding.Components.DisposeComponent.Process> newProcesses) {
        var index = GameComponentsLookup.Dispose;
        var component = (Game.Binding.Components.DisposeComponent)CreateComponent(index, typeof(Game.Binding.Components.DisposeComponent));
        component.processes = newProcesses;
        AddComponent(index, component);
    }

    public void ReplaceDispose(System.Collections.Generic.List<Game.Binding.Components.DisposeComponent.Process> newProcesses) {
        var index = GameComponentsLookup.Dispose;
        var component = (Game.Binding.Components.DisposeComponent)CreateComponent(index, typeof(Game.Binding.Components.DisposeComponent));
        component.processes = newProcesses;
        ReplaceComponent(index, component);
    }

    public void RemoveDispose() {
        RemoveComponent(GameComponentsLookup.Dispose);
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

    static Entitas.IMatcher<GameEntity> _matcherDispose;

    public static Entitas.IMatcher<GameEntity> Dispose {
        get {
            if (_matcherDispose == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Dispose);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherDispose = matcher;
            }

            return _matcherDispose;
        }
    }
}