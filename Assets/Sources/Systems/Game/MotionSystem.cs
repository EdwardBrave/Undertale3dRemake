using Entitas;
using UnityEngine;

namespace Systems.Game
{
    public class MotionSystem : IExecuteSystem
    {
        private readonly GameContext _context;
        private readonly IGroup<GameEntity> _inMotionEntities;
        private readonly ICollector<GameEntity> _onMoveEventEntities;
        public MotionSystem(Contexts contexts)
        {
            _context = contexts.game;
            _inMotionEntities = contexts.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.MoveInDirection, GameMatcher.Motion)
                    .AnyOf(GameMatcher.Position, GameMatcher.Rotation));
            _onMoveEventEntities = contexts.game.CreateCollector(
                new TriggerOnEvent<GameEntity>(GameMatcher.MoveInDirection, GroupEvent.AddedOrRemoved));
        }

        public void Execute()
        {
            foreach (var entity in _onMoveEventEntities.collectedEntities)
            {
                if (!entity.hasMotion)
                    continue;
                entity.ReplaceMotion(entity.motion.maxSpeed, 
                    entity.hasMoveInDirection ? entity.motion.maxSpeed : 0);
            }
            _onMoveEventEntities.ClearCollectedEntities();
            
            foreach (var entity in _inMotionEntities)
            {
                var vector = entity.moveInDirection.vector;
                if(entity.hasPosition)
                    entity.ReplacePosition(entity.position.value + 
                                           entity.motion.speed * Time.deltaTime * vector);
                if (entity.hasRotation)
                {
                    float angle = Mathf.Atan2(vector.x, vector.z) * 57.2957795f;
                    entity.ReplaceRotation(new Vector3(entity.rotation.euler.x,angle, entity.rotation.euler.z));
                }
            }
        }
    }
}