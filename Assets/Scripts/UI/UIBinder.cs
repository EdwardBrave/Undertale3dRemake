using System;
using System.Collections.Generic;
using UI.BondedElements;
using UI.TreeDataModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class UIBinder : MonoBehaviour
    {
        [HideInInspector]
        public bool isShowBinders = false;
        [HideInInspector]
        public bool isShowImages = false;
        [HideInInspector]
        public bool isShowTexts = false;
        [HideInInspector]
        public bool isShowFields = false;
        
        [SerializeField]
        public bool isAutoSearch = true;

        [SerializeField]
        public UIData loadedUIData;

        [SerializeField]
        public List<BondedUIBinder> bondedBinders = new List<BondedUIBinder>();
    
        [SerializeField]
        public List<BondedImage> bondedImages = new List<BondedImage>();

        [SerializeField]
        public List<BondedText> bondedTexts = new List<BondedText>();
        
        [SerializeField]
        public List<BondedField> bondedFields = new List<BondedField>();

        public BondedUIBinder GetBinder(string kayName) => bondedBinders.Get(kayName);
        
        public BondedImage GetImage(string kayName)
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedImages.Get(kayName);
            return GetBinder(args[0]).Content.GetImage(args[1]);
        }
        
        public BondedText GetText(string kayName) 
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedTexts.Get(kayName);
            return GetBinder(args[0]).Content.GetText(args[1]);
        }
        
        public BondedField GetField(string kayName)
        {
            var args = kayName.Split(new char[]{'/','\\'}, 2);
            if (args.Length == 1)            
                return bondedFields.Get(kayName);
            return GetBinder(args[0]).Content.GetField(args[1]);
        }

        private void Start()
        {
            if (isAutoSearch)
                FindBindings();
        }

        public void FindBindings(Transform parent = null)
        {
            if (!parent)
            {
                parent = transform;
                bondedBinders.Clear();
                bondedBinders.Add(new BondedUIBinder("ROOT", -1, 0) 
                    {children = new List<TreeElement>()});
                
                bondedImages.Clear();
                bondedImages.Add(new BondedImage("ROOT", -1, 0) 
                    {children = new List<TreeElement>()});
                
                bondedTexts.Clear();
                bondedTexts.Add(new BondedText("ROOT", -1, 0) 
                    {children = new List<TreeElement>()});
                
                bondedFields.Clear();
                bondedFields.Add(new BondedField("ROOT", -1, 0) 
                    {children = new List<TreeElement>()});
            }
            
            foreach (Transform child in parent)
            {
                var binder = child.GetComponent<UIBinder>();
                if (binder && binder != this)
                {
                    var item = new BondedUIBinder(binder.name, 0, bondedBinders.Count) 
                        {parent = bondedBinders[0], Content = binder};
                    item.parent.children.Add(item);
                    bondedBinders.Add(item);
                    continue;
                }
                else if (child.GetComponent<Image>() is Image image && 
                         image.name != "Image" && !image.name.Contains("("))
                {
                    var item = new BondedImage(image.name, 0, bondedImages.Count) 
                        {parent = bondedImages[0], Content = image};
                    item.parent.children.Add(item);
                    bondedImages.Add(item);
                }
                else if (child.GetComponent<Text>() is Text text && 
                         text.name != "Text" && text.name != "Placeholder" && !text.name.Contains("("))
                {
                    var item = new BondedText(text.name, 0, bondedTexts.Count) 
                        {parent = bondedTexts[0], Content = text};
                    item.parent.children.Add(item);
                    bondedTexts.Add(item);
                }
                if (child.GetComponent<InputField>() is InputField field &&
                         field.name != "InputField" && !field.name.Contains("("))
                {
                    var item = new BondedField(field.name, 0, bondedFields.Count) 
                        {parent = bondedFields[0], Content = field};
                    item.parent.children.Add(item);
                    bondedFields.Add(item);
                }
                if (child.childCount > 0)
                    FindBindings(child);
            }
        }
        
        public void LoadUIData(UIData uiData)
        {
            if (isAutoSearch)
                FindBindings();
            foreach (var binder in bondedBinders)
            {
                if (binder.name == "ROOT")
                    continue;
                var item = uiData.bondedData.Find(o => o.name == binder.name);
                if (item != null)
                    binder.Content.LoadUIData(item.Content);
            }
            foreach (var image in bondedImages)
            {
                if (image.name == "ROOT")
                    continue;
                var item = uiData.bondedSprites.Find(o => o.name == image.name);
                if (item != null && image.Content != null)
                    image.Content.sprite = item.Content;
            }
            foreach (var text in bondedTexts)
            {
                if (text.name == "ROOT")
                    continue;
                var item = uiData.bondedStrings.Find(o => o.name == text.name);
                if (item != null && text.Content != null)
                    text.Content.text = item.Content;
            }
            loadedUIData = uiData;
            enabled = false;
            enabled = true;
        }
    }
}
