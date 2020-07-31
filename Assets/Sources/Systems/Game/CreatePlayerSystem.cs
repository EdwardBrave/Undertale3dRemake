using Entitas;
using UnityEngine;

namespace Systems.Game
{
    public class CreatePlayerSystem: IInitializeSystem
    {
        private readonly GameContext _gameContext;
        private readonly string _playerPrefabPath;
        
        public CreatePlayerSystem(Contexts contexts)
        {
            _gameContext = contexts.game;
            _playerPrefabPath = contexts.core.coreConfig.value.gamePrefabsPath + "Frisk";
        }
        
        public void Initialize()
        {
            /*var entity = _gameContext.CreateEntity();
            entity.AddPosition(new Vector3(-0.15f, 0,-5.86f));
            entity.AddPrefab(_playerPrefabPath);
            entity.AddRotation(Vector3.zero);
            entity.isPlayerController = true;
            entity.AddMotion(3, 0);
            entity.AddAnimator(null);*/
        }
    }
}