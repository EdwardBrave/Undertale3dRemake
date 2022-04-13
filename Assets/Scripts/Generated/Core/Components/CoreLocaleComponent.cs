//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class CoreContext {

    public CoreEntity localeEntity { get { return GetGroup(CoreMatcher.Locale).GetSingleEntity(); } }
    public Core.Localization.LocaleComponent locale { get { return localeEntity.locale; } }
    public bool hasLocale { get { return localeEntity != null; } }

    public CoreEntity SetLocale(UnityEngine.Localization.LocaleIdentifier newIdentifier) {
        if (hasLocale) {
            throw new Entitas.EntitasException("Could not set Locale!\n" + this + " already has an entity with Core.Localization.LocaleComponent!",
                "You should check if the context already has a localeEntity before setting it or use context.ReplaceLocale().");
        }
        var entity = CreateEntity();
        entity.AddLocale(newIdentifier);
        return entity;
    }

    public void ReplaceLocale(UnityEngine.Localization.LocaleIdentifier newIdentifier) {
        var entity = localeEntity;
        if (entity == null) {
            entity = SetLocale(newIdentifier);
        } else {
            entity.ReplaceLocale(newIdentifier);
        }
    }

    public void RemoveLocale() {
        localeEntity.Destroy();
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

    public Core.Localization.LocaleComponent locale { get { return (Core.Localization.LocaleComponent)GetComponent(CoreComponentsLookup.Locale); } }
    public bool hasLocale { get { return HasComponent(CoreComponentsLookup.Locale); } }

    public void AddLocale(UnityEngine.Localization.LocaleIdentifier newIdentifier) {
        var index = CoreComponentsLookup.Locale;
        var component = (Core.Localization.LocaleComponent)CreateComponent(index, typeof(Core.Localization.LocaleComponent));
        component.identifier = newIdentifier;
        AddComponent(index, component);
    }

    public void ReplaceLocale(UnityEngine.Localization.LocaleIdentifier newIdentifier) {
        var index = CoreComponentsLookup.Locale;
        var component = (Core.Localization.LocaleComponent)CreateComponent(index, typeof(Core.Localization.LocaleComponent));
        component.identifier = newIdentifier;
        ReplaceComponent(index, component);
    }

    public void RemoveLocale() {
        RemoveComponent(CoreComponentsLookup.Locale);
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

    static Entitas.IMatcher<CoreEntity> _matcherLocale;

    public static Entitas.IMatcher<CoreEntity> Locale {
        get {
            if (_matcherLocale == null) {
                var matcher = (Entitas.Matcher<CoreEntity>)Entitas.Matcher<CoreEntity>.AllOf(CoreComponentsLookup.Locale);
                matcher.componentNames = CoreComponentsLookup.componentNames;
                _matcherLocale = matcher;
            }

            return _matcherLocale;
        }
    }
}