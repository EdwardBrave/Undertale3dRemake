using Sirenix.OdinInspector;
using UI.Open;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "new UiConfig", menuName = "Configs/UiConfig")]
    public class UiConfig : ScriptableObject
    {
        [AssetSelector(Paths = "Assets/Prefabs/GUI/Canvases")]
        public InitUiEntity mainScreen;
        
        [TitleGroup("Main menu")]
        [AssetSelector(Paths = "Assets/Prefabs/GUI/Windows")]
        public InitUiEntity mainMenu;
    }
}