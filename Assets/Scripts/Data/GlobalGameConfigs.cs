using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Data
{
    [Core, Unique]
    [CreateAssetMenu(fileName = "new GlobalGameConfigs", menuName = "Configs/GlobalGameConfigs")]
    public class GlobalGameConfigs: ScriptableObject
    {
        public UiConfig uiConfig;
        
        
    }
}