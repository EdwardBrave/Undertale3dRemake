using Entitas;
using UnityEngine;

namespace Logic.Systems.Game
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
                GameMatcher.AllOf(GameMatcher.MoveInDirection, GameMatcher.Motion, GameMatcher.View));
            _onMoveEventEntities = contexts.game.CreateCollector(
                new TriggerOnEvent<GameEntity>(GameMatcher.MoveInDirection, GroupEvent.Removed));
        }

        public void Execute()
        {
            foreach (var entity in _onMoveEventEntities.collectedEntities)
            {
                if (!entity.hasMotion || entity.hasMoveInDirection)
                    continue;
                entity.ReplaceMotion(entity.motion.maxSpeed, 0);
            }
            _onMoveEventEntities.ClearCollectedEntities();
            
            foreach (var entity in _inMotionEntities)
            {
                var vector = entity.moveInDirection.vector;
                if (vector == Vector3.zero)
                {
                    entity.ReplaceMotion(entity.motion.maxSpeed, 0);
                    continue;
                }
                float deltaSpeed = Mathf.MoveTowards(
                    entity.motion.speed, entity.motion.maxSpeed, 10 * Time.deltaTime);
                entity.ReplaceMotion(entity.motion.maxSpeed, deltaSpeed);
                
                var transform = entity.view.obj.transform;
                transform.position += entity.motion.speed * Time.deltaTime * vector;
                float angle = Mathf.Atan2(vector.x, vector.z) * 57.2957795f;
                float speed = (Mathf.Abs(transform.eulerAngles.y - angle) + 360 )* Time.deltaTime;
                angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, angle, speed);
                transform.eulerAngles = new Vector3(entity.view.Rotation.x, angle, entity.view.Rotation.z);
            }
        }
    }
}