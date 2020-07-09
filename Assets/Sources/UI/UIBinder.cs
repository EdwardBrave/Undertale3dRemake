using System;
using System.Collections.Generic;
using UI.BondedElements;
using UI.TreeDataModel;
using UnityEngine;

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
        
        public BondedImage GetImage(string kayName) => bondedImages.Get(kayName);
        
        public BondedText GetText(string kayName) => bondedTexts.Get(kayName);
        
        public BondedField GetField(string kayName) => bondedFields.Get(kayName);
        

        public void LoadUIData(UIData uiData)
        {
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
