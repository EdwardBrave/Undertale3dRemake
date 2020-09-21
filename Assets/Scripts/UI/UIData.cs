using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
   [Serializable]
   [CreateAssetMenu(fileName = "NewUIData", menuName = "dataFiles/UI Data")]
   public class UIData : SerializedScriptableObject
   {
      [TableList]
      public List<Bonded<UIData>> bondedData = new List<Bonded<UIData>>();
      
      [TableList]
      public List<Bonded<List<UIData>>> bondedList = new List<Bonded<List<UIData>>>();
      
      [TableList]
      public List<Bonded<Sprite>> bondedSprites = new List<Bonded<Sprite>>();
      
      [TableList]
      public List<Bonded<Color>> bondedColors = new List<Bonded<Color>>();
      
      [TableList]
      public List<Bonded<String>> bondedStrings = new List<Bonded<String>>();
      
      [TableList]
      public List<Bonded<String>> settings = new List<Bonded<String>>();
      
      public UIData GetBinder(string kayName) => bondedData.Get(kayName);
      
      public List<UIData> GetBondedList(string kayName) => bondedList.Get(kayName);
        
      public Color GetColor(string kayName) => bondedColors.Get(kayName);
      
      public Sprite GetSprite(string kayName) => bondedSprites.Get(kayName);
        
      public String GetStrings(string kayName) => bondedStrings.Get(kayName);
      
   }
}
