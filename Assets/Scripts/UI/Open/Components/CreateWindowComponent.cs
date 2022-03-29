using Entitas;

namespace UI.Open.Components
{
    [Ui]
    public class CreateWindowComponent : IComponent
    {
        public InitUiEntity prefab;
        public UiEntity container;
    }
}
