using Entitas;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Core.Localization
{
    public class LocalizationSystem: IInitializeSystem, IExecuteSystem, ITearDownSystem
    {
        private readonly CoreContext _coreContext;
        private ICollector<CoreEntity> _localeCollector;

        public LocalizationSystem(Contexts contexts)
        {
            _coreContext = contexts.core;
        }
        
        public void Initialize()
        {
            _coreContext.ecsRootEntity.AddLocale(LocalizationSettings.SelectedLocale.Identifier);
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
            _localeCollector = _coreContext.CreateCollector(CoreMatcher.Locale);
        }

        public void Execute()
        {
            foreach (var entity in _localeCollector.collectedEntities)
            {
                if (!entity.hasLocale || entity.locale.identifier == LocalizationSettings.SelectedLocale.Identifier)
                {
                    continue;
                }
                var newLocale = LocalizationSettings.AvailableLocales.GetLocale(entity.locale.identifier);
                if (newLocale)
                {
                    LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
                    LocalizationSettings.SelectedLocale = newLocale;
                    LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
                }
                else
                {
                    Debug.LogError($"Localization \"{entity.locale.identifier}\" is undefined!");
                }
            } 
            _localeCollector.ClearCollectedEntities();
        }

        public void TearDown()
        {
            _localeCollector.Deactivate();
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
            _localeCollector.ClearCollectedEntities();
        }

        private void OnLocaleChanged(Locale newLocale)
        {
            _coreContext.ReplaceLocale(newLocale.Identifier);
        }
    }
}