//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class CoreContext {

    public CoreEntity globalGameConfigsEntity { get { return GetGroup(CoreMatcher.GlobalGameConfigs).GetSingleEntity(); } }
    public GlobalGameConfigsComponent globalGameConfigs { get { return globalGameConfigsEntity.globalGameConfigs; } }
    public bool hasGlobalGameConfigs { get { return globalGameConfigsEntity != null; } }

    public CoreEntity SetGlobalGameConfigs(Data.GlobalGameConfigs newValue) {
        if (hasGlobalGameConfigs) {
            throw new Entitas.EntitasException("Could not set GlobalGameConfigs!\n" + this + " already has an entity with GlobalGameConfigsComponent!",
                "You should check if the context already has a globalGameConfigsEntity before setting it or use context.ReplaceGlobalGameConfigs().");
        }
        var entity = CreateEntity();
        entity.AddGlobalGameConfigs(newValue);
        return entity;
    }

    public void ReplaceGlobalGameConfigs(Data.GlobalGameConfigs newValue) {
        var entity = globalGameConfigsEntity;
        if (entity == null) {
            entity = SetGlobalGameConfigs(newValue);
        } else {
            entity.ReplaceGlobalGameConfigs(newValue);
        }
    }

    public void RemoveGlobalGameConfigs() {
        globalGameConfigsEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class CoreEntity {

    public GlobalGameConfigsComponent globalGameConfigs { get { return (GlobalGameConfigsComponent)GetComponent(CoreComponentsLookup.GlobalGameConfigs); } }
    public bool hasGlobalGameConfigs { get { return HasComponent(CoreComponentsLookup.GlobalGameConfigs); } }

    public void AddGlobalGameConfigs(Data.GlobalGameConfigs newValue) {
        var index = CoreComponentsLookup.GlobalGameConfigs;
        var component = (GlobalGameConfigsComponent)CreateComponent(index, typeof(GlobalGameConfigsComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceGlobalGameConfigs(Data.GlobalGameConfigs newValue) {
        var index = CoreComponentsLookup.GlobalGameConfigs;
        var component = (GlobalGameConfigsComponent)CreateComponent(index, typeof(GlobalGameConfigsComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveGlobalGameConfigs() {
        RemoveComponent(CoreComponentsLookup.GlobalGameConfigs);
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

    static Entitas.IMatcher<CoreEntity> _matcherGlobalGameConfigs;

    public static Entitas.IMatcher<CoreEntity> GlobalGameConfigs {
        get {
            if (_matcherGlobalGameConfigs == null) {
                var matcher = (Entitas.Matcher<CoreEntity>)Entitas.Matcher<CoreEntity>.AllOf(CoreComponentsLookup.GlobalGameConfigs);
                matcher.componentNames = CoreComponentsLookup.componentNames;
                _matcherGlobalGameConfigs = matcher;
            }

            return _matcherGlobalGameConfigs;
        }
    }
}
