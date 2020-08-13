using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class UIBinder : SerializedMonoBehaviour
    {
        public bool isAutoSearch = true;

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
        public List<Bonded<Text>> bondedTexts = new List<Bonded<Text>>();
        
        [TableList]
        [HideIf("isAutoSearch")]
        public List<Bonded<InputField>> bondedFields = new List<Bonded<InputField>>();

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
        
        public Text GetText(string kayName) 
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedTexts.Get(kayName);
            return GetBinder(args[0])?.GetText(args[1]);
        }
        
        public InputField GetField(string kayName)
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedFields.Get(kayName);
            return GetBinder(args[0])?.GetField(args[1]);
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
                bondedTexts.Clear();
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
                else if (child.GetComponent<Text>() is Text text && 
                         text.name != "Text" && text.name != "Placeholder" && !text.name.Contains("("))
                {
                    bondedTexts.Add(new Bonded<Text>(text.name, text));
                }
                if (child.GetComponent<InputField>() is InputField field &&
                         field.name != "InputField" && !field.name.Contains("("))
                {
                    bondedFields.Add(new Bonded<InputField>(field.name, field));
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
            foreach (var text in bondedTexts)
            {
                var item = uiData.bondedStrings.Get(text.name);
                if (item != null)
                    text.content.text = item;
                var color = uiData.bondedColors.Get(text.name);
                if (color != Color.clear)
                    text.content.color = color;
            }
            enabled = false;
            enabled = true;
        }
    }
    
    
}
