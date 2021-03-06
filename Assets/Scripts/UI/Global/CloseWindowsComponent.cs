using Entitas;
using UI.Open;

namespace UI.Global
{
    [Ui]
    public class CloseWindowsComponent: IComponent
    {
        public InitUiEntity data;
        public bool isForce = false;
    }
}