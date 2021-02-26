using Entitas;
using UnityEngine;

namespace Game.Motion
{
    public class MotionSystem : IExecuteSystem, ITearDownSystem
    {
        private readonly IGroup<GameEntity> _inMotionEntities;
        private readonly ICollector<GameEntity> _endMoveEntities;
        
        public MotionSystem(Contexts contexts)
        {
            _inMotionEntities = contexts.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.MoveInDirection, GameMatcher.Motion, GameMatcher.View));
            _endMoveEntities = contexts.game.CreateCollector(
                new TriggerOnEvent<GameEntity>(GameMatcher.MoveInDirection, GroupEvent.Removed));
        }

        public void Execute()
        {
            foreach (var entity in _endMoveEntities.collectedEntities)
            {
                if (!entity.hasMotion || entity.hasMoveInDirection)
                    continue;
                entity.ReplaceMotion(entity.motion.maxSpeed, 0);
            }
            _endMoveEntities.ClearCollectedEntities();
            
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
                
                entity.view.Position += entity.motion.speed * Time.deltaTime * vector;
                float angle = Mathf.Atan2(vector.x, vector.z) * 57.2957795f;
                float speed = (Mathf.Abs(entity.view.Rotation.y - angle) + 360 ) * Time.deltaTime;
                angle = Mathf.MoveTowardsAngle(entity.view.Rotation.y, angle, speed);
                entity.view.Rotation = new Vector3(entity.view.Rotation.x, angle, entity.view.Rotation.z);
            }
        }

        public void TearDown()
        {
            foreach (var entity in _inMotionEntities.GetEntities())
            {
                entity.RemoveMoveInDirection();
                entity.ReplaceMotion(entity.motion.maxSpeed, 0);
            }
        }
    }
}