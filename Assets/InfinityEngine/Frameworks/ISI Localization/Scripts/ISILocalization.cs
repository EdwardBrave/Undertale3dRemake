﻿/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *
*************************************************************************************************************************************/

using UnityEngine;
using System;
using System.Collections.Generic;
using InfinityEngine.DesignPatterns;

using InfinityEngine.Utils;
using InfinityEngine.Attributes;
using System.Linq;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]
//// <summary>
//// This namespace provide access to localization tools.
//// </summary>
namespace InfinityEngine.Localization
{

    /// <summary>
    ///  Main component of the localization engine of InfinityEngine API.
    /// </summary>
    [AddComponentMenu("InfinityEngine/Localization/ISI Localization")]
    public class ISILocalization : Singleton<ISILocalization>
    {
        #region Fields

        private static ISILocalization InstanceReference;

        private const string ApplicationLanguagePreferenceKey = "___APPLICATION_LANGUAGE___";

        /// <summary>
        /// Name of the PlayerPrefs which contains the default language used when a language is not specified
        /// </summary>
        public const string DefaultLanguagePreferenceKey = "___DEFAULT_LANGUAGE_PREF___";

        /// <summary>
        /// The name of the prefab generated by the editor class of this component.
        /// </summary>
        public const string PrefabName = "ISILocalizationPrefab";
        /// <summary>
        ///  The path of the prefab generated by the editor class of this component.
        /// </summary>

        public const string PrefabPath = "Assets/InfinityEngine/Gen/Resources/ISILocalizationPrefab.prefab";

        /// <summary>
        /// Path for all languages. '{0}' is replaced by the name of the language.
        /// </summary>
        public const string LanguagePaths = "InfinityEngine/Gen/Xml/ISI Localization/{0}/strings.xml";

        internal LocalizedLanguage m_currentLanguage;
        internal Language m_defaultLanguage;
        internal bool m_isInitialized;

        /// <summary>
        /// Invoked when the language changes.
        /// </summary>
        public static Action onLanguageChanged;

        #region Used By The Editor Script (DO NOT MODIFY THE NAME OF THESE FIELDS)

        [SerializeField]
        internal Scene m_nextScene;

        [SerializeField]
        internal int m_loadSceneDelay = 3;

        [SerializeField]
        internal List<LocalizedLanguage> m_languages;


        [SerializeField]
        internal List<string> m_stringKeys;

        [SerializeField]
        internal List<string> m_audiosKeys;

        [SerializeField]
        internal List<string> m_spriteKeys;

        [SerializeField]
        [MinMaxRange(1, "GetStringMaxRange")]
        internal MinMax m_stringKeysRange;

        [MinMaxRange(1, "GetAudioMaxRange")]
        [SerializeField]
        internal MinMax m_spriteKeysRange;

        [MinMaxRange(1, "GetSpriteMaxRange")]
        [SerializeField]
        internal MinMax m_audioKeysRange;

        #endregion Used By The Editor Script 

        #endregion Fields

        #region Properties

        private Language DefaultLanguage
        {
            get
            {
                var pref = PlayerPrefs.GetString(DefaultLanguagePreferenceKey, Language.English.ToString());
                m_defaultLanguage = (Language)Enum.Parse(typeof(Language), pref);
                return m_defaultLanguage;
            }
        }

        /// <summary>
        /// The list of all languages
        /// </summary>
        public List<LocalizedLanguage> LocalizedLanguages
        {
            get => m_languages ?? (m_languages = new List<LocalizedLanguage>());
            set => m_languages = value;
        }

        /// <summary>
        /// List of all string keys
        /// </summary>
        public List<string> StringKeys
        {
            get => m_stringKeys ?? (m_stringKeys = new List<string>());
            set => m_stringKeys = value;
        }

        /// <summary>
        /// List of all audio keys
        /// </summary>
        public List<string> AudioKeys
        {
            get => m_audiosKeys ?? (m_audiosKeys = new List<string>());
            set => m_audiosKeys = value;
        }

        /// <summary>
        /// List of all Sprite keys
        /// </summary>
        public List<string> SpriteKeys
        {
            get => m_spriteKeys ?? (m_spriteKeys = new List<string>());
            set => m_spriteKeys = value;
        }

        /// <summary>
        /// Gets the number of used languages
        /// </summary>
        public int LanguageCount => LocalizedLanguages.Count;

        /// <summary>
        /// Gets a value indicating whether an instance of the class is initialized with a language and presents in the current scene
        /// </summary>
        public static bool IsInitialized => Instance.m_isInitialized;

        /// <summary>
        /// Gets the current language of the plugin
        /// </summary>
        public static Language CurrentLanguage
        {
            get
            {
                if (LoadIfNotInScene())
                    return Instance.m_currentLanguage.Language;
                return Language.English;
            }
        }

        #endregion Properties

        #region Methods

        private int GetStringMaxRange() => Math.Max(0, StringKeys.Count);
        private int GetAudioMaxRange() => Math.Max(0, AudioKeys.Count);
        private int GetSpriteMaxRange() => Math.Max(0, SpriteKeys.Count);

        /// <summary>
        /// Gets a dictionary of all localized string, sprite and audio clip keys
        /// </summary>
        public Dictionary<string, List<string>> Keys
        {
            get
            {
                return new Dictionary<string, List<string>>()
                {
                    { "strings", StringKeys },
                    { "audios",  AudioKeys },
                    { "sprites", SpriteKeys }
                };
            }
        }

        /// <summary>
        /// Adds the given localized language informations to the list of available languages.
        /// </summary>
        /// <param name="language">The informations of the language</param>
        public void AddLanguage(LocalizedLanguage language)
        {
            LocalizedLanguages.Add(language);
        }

        /// <summary>
        /// This function adds the language if it does not exist otherwise it replaces the one that currently exists with the new one.
        /// </summary>
        /// <param name="language">The language to replaces</param>
        public void ReplaceLocalizedLanguage(LocalizedLanguage language)
        {

            var exists = false;
            LocalizedLanguages.ForEach(model =>
            {
                if (model.Language == language.Language)
                {
                    model.LocalizedStrings = language.LocalizedStrings;
                    exists = true;
                }
            });

            if (!exists)
                LocalizedLanguages.Add(language);
        }

        /// <summary>
        /// Checks if the given language is added to the list of available languages
        /// </summary>
        /// <param name="language">The Language</param>
        /// <returns><c>true</c> if the language exists <c>false</c> otherwise</returns>
        public bool HasLanguage(Language language)
        {
            return LocalizedLanguages.Any(token => token.Language == language);
        }

        /// <summary> 
        /// Removes all unused key. 
        /// </summary>
        public void Validate()
        {
            foreach (var l in m_languages)
            {
                l.LocalizedAudio = l.LocalizedAudio.Where(elem => m_audiosKeys.Contains(elem.Key)).ToList();
                l.LocalizedStrings = l.LocalizedStrings.Where(elem => m_stringKeys.Contains(elem.Key)).ToList();
                l.LocalizedSprites = l.LocalizedSprites.Where(elem => m_spriteKeys.Contains(elem.Key)).ToList();
            }
        }

        #region Static

        /// <summary>
        ///  Gets the value the given <paramref name="name"/> in current <see cref="CurrentLanguage"/>
        /// </summary>
        /// <remarks>
        ///    There is a shortcut way to use this function.<br/>
        ///     
        ///     When you request a string from <see cref="R3.strings"/>, the class return an object of type <see cref="ISIString"/>. <br/>
        ///     You can use the returned value as a string object.   
        /// </remarks>
        /// <example>
        ///     Exemple : <br/>
        ///     
        ///     Suppose you have created a "TEST" key in the Editor with the value : "Ceci est un test" in French Language. <br/>
        ///     If you want to get the value of "TEST" in current language you need to call this function like :
        ///     <code>
        ///     Debugger.Log(ISILocalization.GetValueof("TEST"));
        ///     </code>
        ///     
        ///     This code log the message "Ceci est un test".
        ///     
        ///     You can also access directly to the value of the key by using the class <see cref="R3.strings"/>.
        /// </example>
        /// <param name="name">The key </param>
        /// <returns>The value of the <see cref="LocalizedString"/> with the given name or <c>null</c> if it doesn't exists</returns>
        public static string GetValueOf(string name)
        {
            if (LoadIfNotInScene())
                return Instance.m_currentLanguage.GetString(name);

            Debugger.LogError("ISI Localization is not initialized !", Instance.gameObject);

            return null;
        }

        /// <summary>
        ///  Gets the value of the key 'key' in current <see cref="LocalizedLanguage"/> and replaces all strings with the syntax {i} by the value of <paramref name="data"/>[i] <br/>
        /// </summary>
        /// <remarks>
        ///    There is a shortcut way to use this function.<br/>
        ///     
        ///     When you request a string from <see cref="R3.strings"/>, the class return an object of type <see cref="ISIString"/>. <br/>
        ///     You can use the function <see cref="ISIString.Format(object[])"/> of the returned object.    
        /// </remarks>
        /// 
        /// <example> 
        ///     <br/>
        ///     
        ///     Suppose you have created a "TEST" key in ISILocalization Editor with the value : <br/>
        ///     "{0} is an integer, {1} is a string, {true} is a boolean. I reuse the integer {1}" in English Language
        ///     
        ///     and the editor generated a class <see cref="R3.strings"/> that contains all keys.
        ///     
        ///      There is two way to gets the value of "TEST" : <br/>
        ///     
        ///     - First way 
        ///     <code>
        ///         Debugger.Log(ISILocalization.GetFormatedValueof(R.strings.TEST.key, 1, "test", true));
        ///     </code> <br/>
        ///     
        ///     - Second way 
        ///     <code>
        ///         Debugger.Log(R.strings.TEST.Format(1, "test", true));
        ///     </code> <br/>
        ///     
        ///     Theses codes log the message "1 is an integer, test is a string, true is a boolean. I reuse the integer 1"   
        /// </example>
        /// <param name="key">the key of the sentence to get </param>
        /// <param name="data">list of objects</param>
        /// <returns>The value of the <see cref="LocalizedString"/> with the given name or <c>null</c> if it doesn't exists</returns>
        public static string GetFormatedValueOf(string key, params object[] data)
        {
            LoadIfNotInScene();

            string message = GetValueOf(key);
            if (string.IsNullOrEmpty(message))
                return null;
            return string.Format(message, data);
        }

        /// <summary>
        /// Gets a localized audio clip
        /// </summary>
        /// <param name="name">AudioClip name</param>
        /// <returns>The localized audio clip if it exist null otherwise</returns>
        public static AudioClip GetAudio(string name)
        {
            if (LoadIfNotInScene())
                return Instance.m_currentLanguage.GetAudio(name);
            Debugger.LogError("ISILocalization is not initialized !", Instance.gameObject);
            return null;
        }

        /// <summary>
        /// Gets a localized Sprite
        /// </summary>
        /// <param name="name">Sprite name</param>
        /// <returns>The localized Sprite if it exist null otherwise</returns>
        public static Sprite GetSprite(string name)
        {
            if (LoadIfNotInScene())
                return Instance.m_currentLanguage.GetSprite(name);
            Debugger.LogError("ISILocalization is not initialized !", Instance.gameObject);
            return null;
        }

        /// <summary>
        /// Changes the current language of the plugin
        /// </summary>
        /// <param name="language">The new language</param>
        /// <returns><c>true</c> if the language has changed</returns>
        public static bool ChangeLanguage(Language language)
        {
            if (Instance.LocalizedLanguages == null && Instance.LocalizedLanguages.Count == 0)
                return false;
            Instance.m_currentLanguage = Instance.LocalizedLanguages.Find(model => model.Language == language);
            if (Instance.m_currentLanguage != null)
            {
                Instance.m_isInitialized = true;
                PlayerPrefs.SetString(ApplicationLanguagePreferenceKey, language.ToString());
                LocalizedString.OnLanguageChanged();
                LocalizedSprite.OnLanguageChanged();
                if (onLanguageChanged != null)
                    onLanguageChanged.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads the scene scene if it exists
        /// </summary>
        public static void LoadNextLevel()
        {
            if (Instance.m_nextScene == null)
            {
                Debugger.LogError("There is no specifed scene to load", Instance);
            }
            else
            {
                Infinity.LoadLevelAfterDelay(Instance.m_nextScene.SceneName, Instance.m_loadSceneDelay);
            }

        }

        /// <summary>
        /// Loads the ISILocalization prefab placed at <see cref="PrefabPath"/> if there is not ISILocalization GameObject in the scene.
        /// </summary>
        /// <remarks>
        /// This function return <c>false</c> if there is not prefab in Resources folder<c>true</c> otherwise and sets <see cref="DefaultLanguage"/> as the current language.
        /// </remarks>
        /// <returns><c>true</c> if this is setup <c>false</c> otherwise</returns>
        public static bool LoadIfNotInScene()
        {
            if (InstanceReference == null)
            {
                InstanceReference = FindObjectOfType<ISILocalization>();
            }

            if (InstanceReference == null)
            {
                var prefab = Resources.Load<GameObject>(PrefabName);
                if (prefab == null)
                {
                    Debugger.LogError("There is not ISILocalization prefab in resources folders ! ", Instance.gameObject);
                    return false;
                }
                Instantiate(prefab);

                InstanceReference = Instance;

                if (Instance.LocalizedLanguages == null || Instance.LocalizedLanguages.Count == 0)
                {
                    Debugger.LogError("ISILocalization is not Setup ! ", Instance.gameObject);
                    return false;
                }
                if (ChangeLanguage(Instance.DefaultLanguage))
                {
                    return true;
                }
                Debugger.LogError($"{Instance.m_defaultLanguage} language is not register in ISILocalization !", Instance.gameObject);
                return false;
            }

            if (Instance.m_currentLanguage == null)
            {
                if (ChangeLanguage(Instance.DefaultLanguage))
                {
                    return true;
                }
            }

            return true;
        }

        #endregion Static  

        #endregion Methods

    }
}