using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class UIBinder : SerializedMonoBehaviour
    {
        public bool isAutoSearch = true;

        public string identifier;

        [TableList]
        [HideIf("isAutoSearch")]
        public List<Bonded<UIBinder>> bondedBinders = new List<Bonded<UIBinder>>();
        
        [TableList]
        [HideIf("isAutoSearch")]
        public List<Bonded<ListLayout>> bondedLists = new List<Bonded<ListLayout>>();
        
        [TableList]
        [HideIf("isAutoSearch")]
        public List<Bonded<Image>> bondedImages = new List<Bonded<Image>>();

        [TableList]
        [HideIf("isAutoSearch")]
        public List<Bonded<TMP_Text>> bondedTmpTexts = new List<Bonded<TMP_Text>>();
        
        [TableList]
        [HideIf("isAutoSearch")]
        public List<Bonded<TMP_InputField>> bondedFields = new List<Bonded<TMP_InputField>>();
        
        [TableList]
        [HideIf("isAutoSearch")]
        public List<Bonded<Toggle>> bondedToggle = new List<Bonded<Toggle>>();

        public UIBinder GetBinder(string kayName) => bondedBinders.Get(kayName);
        
        public ListLayout GetListLayout(string kayName)
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)
                return bondedLists.Get(kayName);
            return GetBinder(args[0])?.GetListLayout(args[1]);
        }
        
        public Image GetImage(string kayName)
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedImages.Get(kayName);
            return GetBinder(args[0])?.GetImage(args[1]);
        }

        public TMP_Text GetText(string kayName) 
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedTmpTexts.Get(kayName);
            return GetBinder(args[0])?.GetText(args[1]);
        }
        
        public TMP_InputField GetField(string kayName)
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedFields.Get(kayName);
            return GetBinder(args[0])?.GetField(args[1]);
        }
        
        public Toggle GetToggle(string kayName)
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedToggle.Get(kayName);
            return GetBinder(args[0])?.GetToggle(args[1]);
        }

        private void Start()
        {
            if (isAutoSearch)
                FindBindings();
        }
        
        [Button]
        [PropertyOrder(-1)]
        public void FindBindings(Transform parent = null)
        {
            if (!parent)
            {
                parent = transform;
                bondedBinders.Clear();
                bondedImages.Clear();
                bondedTmpTexts.Clear();
                bondedToggle.Clear();
                bondedFields.Clear();
            }
            
            foreach (Transform child in parent)
            {
                var binder = child.GetComponent<UIBinder>();
                if (binder && binder != this)
                {
                    bondedBinders.Add(new Bonded<UIBinder>(binder.name, binder));
                    continue;
                }
                if (child.GetComponent<ListLayout>() is ListLayout listLayout && 
                         listLayout.name != "ListLayout" && !listLayout.name.Contains("("))
                {
                    bondedLists.Add(new Bonded<ListLayout>(listLayout.name, listLayout));
                    continue;
                }
                if (child.GetComponent<Image>() is Image image && 
                         image.name != "Image" && !image.name.Contains("("))
                {
                    bondedImages.Add(new Bonded<Image>(image.name, image));
                }
                else if (child.GetComponent<TMP_Text>() is TMP_Text tmpText && 
                         tmpText.name != "Text" && tmpText.name != "Placeholder" && !tmpText.name.Contains("("))
                {
                    bondedTmpTexts.Add(new Bonded<TMP_Text>(tmpText.name, tmpText));
                }
                if (child.GetComponent<TMP_InputField>() is TMP_InputField field &&
                    field.name != "InputField" && !field.name.Contains("("))
                {
                    bondedFields.Add(new Bonded<TMP_InputField>(field.name, field));
                }
                if (child.GetComponent<Toggle>() is Toggle toggle &&
                    toggle.name != "Toggle" && !toggle.name.Contains("("))
                {
                    bondedToggle.Add(new Bonded<Toggle>(toggle.name, toggle));
                }
                if (child.childCount > 0)
                    FindBindings(child);
            }
        }
        
        [Button]
        [PropertyOrder(-1)]
        public void LoadUIData(UIData uiData)
        {
            if (isAutoSearch)
                FindBindings();
            foreach (var binder in bondedBinders)
            {
                var item = uiData.bondedData.Get(binder.name);
                if (item)
                    binder.content.LoadUIData(item);
            }
            foreach (var list in bondedLists)
            {
                var item = uiData.bondedList.Get(list.name);
                if (item != null)
                    list.content.Reload(item);
            }
            foreach (var image in bondedImages)
            {
                var item = uiData.bondedSprites.Get(image.name);
                if (item)
                    image.content.sprite = item;
                var color = uiData.bondedColors.Get(image.name);
                if (color != Color.clear)
                    image.content.color = color;
            }
            foreach (var text in bondedTmpTexts)
            {
                var item = uiData.bondedStrings.Get(text.name);
                if (item != null)
                    text.content.text = item;
                var color = uiData.bondedColors.Get(text.name);
                if (color != Color.clear)
                    text.content.color = color;
            }
            foreach (var toggle in bondedToggle)
            {
                var item = uiData.bondedChecks.Get(toggle.name);
                toggle.content.isOn = item;
            }
            enabled = false;
            enabled = true;
        }
    }
    
    
}
