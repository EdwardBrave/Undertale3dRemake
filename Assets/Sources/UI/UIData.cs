using System;
using System.Collections.Generic;
using UI.BondedElements;
using UI.TreeDataModel;
using UnityEngine;

namespace UI
{
   [Serializable]
   [CreateAssetMenu(fileName = "NewUIData", menuName = "dataFiles/UI Data")]
   public class UIData : ScriptableObject
   {
      [HideInInspector] public bool isShowConfigs = false;
      [HideInInspector] public bool isShowSprites = true;
      [HideInInspector] public bool isShowStrings = true;
      [HideInInspector] public bool isShowSettings = false;
      
      [SerializeField]
      public List<BondedUIData> bondedData = new List<BondedUIData>();
      
      [SerializeField]
      public List<BondedSprite> bondedSprites = new List<BondedSprite>();
      
      [SerializeField]
      public List<BondedString> bondedStrings = new List<BondedString>();
      
      [SerializeField]
      public List<BondedString> settings = new List<BondedString>();
      
      public BondedUIData GetBinder(string kayName) => bondedData.Get(kayName);
        
      public BondedSprite GetImage(string kayName) => bondedSprites.Get(kayName);
        
      public BondedString GetText(string kayName) => bondedStrings.Get(kayName);
      
   }
}
