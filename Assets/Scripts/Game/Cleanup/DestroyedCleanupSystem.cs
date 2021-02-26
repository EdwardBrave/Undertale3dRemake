using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;

namespace Game.Cleanup
{
    public class DestroyedCleanupSystem: ICleanupSystem
    {
        private readonly IGroup<GameEntity> _destroyedEntities;
        
        private readonly List<GameEntity> _buffer;
        private readonly GameContext _gameContext;

        public DestroyedCleanupSystem(Contexts contexts)
        {
            _gameContext = contexts.game;
            _destroyedEntities = contexts.game.GetGroup(GameMatcher.Destroyed);
            
            _buffer = new List<GameEntity>();
        }
        
        public void Cleanup()
        {
            foreach (var entity in _destroyedEntities.GetEntities(_buffer))
            {
                if (entity.hasView)
                {
                    entity.view.obj.Unlink();
                    entity.view.obj.DestroyGameObject();
                }
                entity.Destroy();
            }
        }
    }
}