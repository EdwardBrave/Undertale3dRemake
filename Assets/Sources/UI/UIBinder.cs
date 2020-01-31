using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class UIBinder : MonoBehaviour
{
    [SerializeField]
    public UIConfig testUIConfig;
    
    [SerializeField]
    public List<BondedUIBinder> bondedBinders = new List<BondedUIBinder>();
    
    [SerializeField]
    public List<BondedImage> bondedImages = new List<BondedImage>();

    [SerializeField]
    public List<BondedText> bondedTexts = new List<BondedText>();

    public void LoadTextUIConfig(UIConfig uiConfig)
    {
        foreach (var binder in bondedBinders)
        {
            if (binder.name == "ROOT")
                continue;
            var item = uiConfig.bondedConfigs.Find(o => o.name == binder.name);
            if (item != null)
                binder?.component.LoadTextUIConfig(item.config);
        }
        foreach (var image in bondedImages)
        {
            if (image.name == "ROOT")
                continue;
            var item = uiConfig.bondedSprites.Find(o => o.name == image.name);
            if (item != null && image.component != null)
                image.component.sprite = item.sprite;
        }
        foreach (var text in bondedTexts)
        {
            if (text.name == "ROOT")
                continue;
            var item = uiConfig.bondedStrings.Find(o => o.name == text.name);
            if (item != null && text.component != null)
                text.component.text = item.str;
        }
        enabled = false;
        enabled = true;
    }
}
