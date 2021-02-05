using System;
using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Core.Data
{
    [Core, Unique, CreateAssetMenu(fileName = "NewGameSettings", menuName = "gameConfigs/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public List<CanvasInitData> canvasInitList;
    }
    
    [Serializable]
    public class CanvasInitData
    {
        public string name;
        public List<string> windows;

        public CanvasInitData()
        {
            windows = new List<string>();
        }

        public CanvasInitData(string name, List<string> windows)
        {
            this.name = name;
            this.windows = windows;
        }
    }
}
