using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;
using UI.Open;
using UnityEngine;

namespace UI.Data
{
    [Ui, Unique]
    [CreateAssetMenu(fileName = "NewUiConfig", menuName = "Configs/UiConfig")]
    public class UiConfig: ScriptableObject
    {
        public List<InitUiEntity> startWindows = new List<InitUiEntity>();
    }
}