using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Data
{
     [Core, Unique, CreateAssetMenu(fileName = "NewCoreConfig", menuName = "gameConfigs/CoreConfig")]
     public class CoreConfig : ScriptableObject
     {
          public string gamePrefabsPath;
          public string uiPrefabsPath;
          public string dataPath;
          public string canvasesPath;
     }
}
