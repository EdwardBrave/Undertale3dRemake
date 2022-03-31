using Entitas;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Core.Localization
{
    public class LocalizationSystem: IReactiveSystem
    {
        private readonly CoreContext _coreContext;
        private readonly ICollector<CoreEntity> _localeCollector;

        public LocalizationSystem(Contexts contexts)
        {
            _coreContext = contexts.core;
            _localeCollector = _coreContext.CreateCollector(CoreMatcher.Locale);
            _localeCollector.Deactivate();
        }

        public void Activate()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
            _localeCollector.Activate();
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

        public void Deactivate()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
            _localeCollector.Deactivate();
        }

        public void Clear()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
            _localeCollector.ClearCollectedEntities();
        }

        private void OnLocaleChanged(Locale newLocale)
        {
            _coreContext.ReplaceLocale(newLocale.Identifier);
        }
    }
}