//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class CoreEntity {

    public Core.UnityScene.Components.SceneLoadingComponent sceneLoading { get { return (Core.UnityScene.Components.SceneLoadingComponent)GetComponent(CoreComponentsLookup.SceneLoading); } }
    public bool hasSceneLoading { get { return HasComponent(CoreComponentsLookup.SceneLoading); } }

    public void AddSceneLoading(UnityEngine.AsyncOperation newOperation) {
        var index = CoreComponentsLookup.SceneLoading;
        var component = (Core.UnityScene.Components.SceneLoadingComponent)CreateComponent(index, typeof(Core.UnityScene.Components.SceneLoadingComponent));
        component.operation = newOperation;
        AddComponent(index, component);
    }

    public void ReplaceSceneLoading(UnityEngine.AsyncOperation newOperation) {
        var index = CoreComponentsLookup.SceneLoading;
        var component = (Core.UnityScene.Components.SceneLoadingComponent)CreateComponent(index, typeof(Core.UnityScene.Components.SceneLoadingComponent));
        component.operation = newOperation;
        ReplaceComponent(index, component);
    }

    public void RemoveSceneLoading() {
        RemoveComponent(CoreComponentsLookup.SceneLoading);
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
public sealed partial class CoreMatcher {

    static Entitas.IMatcher<CoreEntity> _matcherSceneLoading;

    public static Entitas.IMatcher<CoreEntity> SceneLoading {
        get {
            if (_matcherSceneLoading == null) {
                var matcher = (Entitas.Matcher<CoreEntity>)Entitas.Matcher<CoreEntity>.AllOf(CoreComponentsLookup.SceneLoading);
                matcher.componentNames = CoreComponentsLookup.componentNames;
                _matcherSceneLoading = matcher;
            }

            return _matcherSceneLoading;
        }
    }
}