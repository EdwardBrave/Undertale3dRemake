using Entitas;
using UnityEngine;

namespace Systems.Game
{
    public class InitSceneSystem: IInitializeSystem
    {
        private readonly GameContext _gameContext;
        
        public InitSceneSystem(Contexts contexts)
        {
            _gameContext = contexts.game;
        }
        
        public void Initialize()
        {
            var playerEntity = _gameContext.CreateEntity();
            playerEntity.AddSceneObject("Player", true);
            playerEntity.isPlayerController = true;
            playerEntity.AddMotion(3, 0);
            playerEntity.AddAnimator(null);
            
            var camEntity = _gameContext.CreateEntity();
            camEntity.AddSceneObject("MainCamera", true);
            camEntity.AddFollow(playerEntity, new Vector3(0,8.5f,-12f));
        }
    }
}