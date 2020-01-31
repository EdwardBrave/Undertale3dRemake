using System.Collections.Generic;
using UnityEngine;

namespace UI
{
   [CreateAssetMenu(fileName = "newUIConfig", menuName = "Config files/UIConfig")]
   public class UIConfig : ScriptableObject
   {
      [SerializeField]
      public List<BondedUIConfig> bondedConfigs = new List<BondedUIConfig>();
      
      [SerializeField]
      public List<BondedSprite> bondedSprites = new List<BondedSprite>();
      
      [SerializeField]
      public List<BondedString> bondedStrings = new List<BondedString>();
   }
}
