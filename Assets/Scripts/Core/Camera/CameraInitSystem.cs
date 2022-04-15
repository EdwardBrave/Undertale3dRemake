using Entitas;
using UnityEngine;

namespace Core.Camera
{
    public class CameraInitSystem: IInitializeSystem
    {
        private readonly CoreContext _coreContext;

        public CameraInitSystem(Contexts contexts)
        {
            _coreContext = contexts.core;
        }
        
        public void Initialize()
        {
            var cameraObject = Object.Instantiate(
                _coreContext.globalGameConfigs.value.mainCameraPrefab,
                _coreContext.ecsRoot.value.transform);
            _coreContext.SetMainCamera(cameraObject);
        }
    }
}