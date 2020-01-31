using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIBinder : MonoBehaviour
{
    [SerializeField]
    public List<BondedImage> bondedImages = new List<BondedImage>();

    [SerializeField]
    public List<BondedText> bondedTexts = new List<BondedText>();
    
}
