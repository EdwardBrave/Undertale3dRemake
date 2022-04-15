using Cinemachine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Core.Camera
{
    [Core, Unique]
    public class MainCameraComponent: IComponent
    {
        public CinemachineBrain brain;
    }
}