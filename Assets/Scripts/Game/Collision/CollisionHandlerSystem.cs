using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using Tools;
using UnityEngine;

namespace Game.Collision
{
    public class CollisionHandlerSystem: ExtendableReactiveSystem<GameEntity>
    {
        private readonly IGroup<GameEntity> _collidersGroup;
        
        ////////////////////////////////////////////////////////////////////
        #region ECS Interface

        public CollisionHandlerSystem(Contexts contexts) : base(contexts.game)
        {
            _collidersGroup = contexts.game.GetGroup(GameMatcher.Collider);
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Collider);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCollider;
        }
        
        public override void Activate()
        {
            base.Activate();
            AddCollectedEntities(_collidersGroup.AsEnumerable());
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                Subscribe(entity, entity.collider.listener);
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            foreach (var entity in _collidersGroup.AsEnumerable())
            {
                Unsubscribe(entity, entity.collider.listener);
            }
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Subscriptions
        
        private void Subscribe(IEntity entity, CollisionListener listener)
        {
            listener.CollisionEnter += OnCollisionEnter;
            listener.CollisionExit += OnCollisionExit;
            listener.TriggerEnter += OnTriggerEnter;
            listener.TriggerExit += OnTriggerExit;
            entity.OnComponentReplaced += OnComponentReplaced;
            entity.OnComponentRemoved += OnComponentRemoved;
        }

        private void Unsubscribe(IEntity entity, CollisionListener listener)
        {
            listener.CollisionEnter -= OnCollisionEnter;
            listener.CollisionExit -= OnCollisionExit;
            listener.TriggerEnter -= OnTriggerEnter;
            listener.TriggerExit -= OnTriggerExit;
            entity.OnComponentReplaced -= OnComponentReplaced;
            entity.OnComponentRemoved -= OnComponentRemoved;

            if (!(entity is GameEntity gameEntity))
            {
                return;
            }
            
            if (gameEntity.hasCollisions)
            {
                bool isChanged = false;
                foreach (var temporaryCollision in gameEntity.collisions.list)
                {
                    if (temporaryCollision.Status != TemporaryStatus.Ended)
                    {
                        isChanged = true;
                        temporaryCollision.Status = TemporaryStatus.Ended;
                    }
                }

                if (isChanged)
                {
                    gameEntity.ReplaceCollisions(gameEntity.collisions.list);
                }
            }

            if (gameEntity.hasTriggers)
            {
                bool isChanged = false;
                foreach (var temporaryTrigger in gameEntity.triggers.list)
                {
                    if (temporaryTrigger.Status != TemporaryStatus.Ended)
                    {
                        isChanged = true;
                        temporaryTrigger.Status = TemporaryStatus.Ended;
                    }
                }

                if (isChanged)
                {
                    gameEntity.ReplaceTriggers(gameEntity.triggers.list);
                }
            }
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Entity event handlers
        
        private void OnComponentReplaced(IEntity entity, int index, IComponent previous, IComponent next)
        {
            if (previous is ColliderComponent collider)
            {
                Unsubscribe(entity, collider.listener);
            }
        }

        private void OnComponentRemoved(IEntity entity, int index, IComponent component)
        {
            if (component is ColliderComponent collider)
            {
                Unsubscribe(entity, collider.listener);
            }
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
        #region Collision event handlers
        
        private void OnCollisionEnter(CollisionListener listener, UnityEngine.Collision collision)
        {
            if (!(listener.GetEntityByLink() is GameEntity gameEntity))
            {
                return;
            }
            if (gameEntity.hasCollisions)
            {
                gameEntity.collisions.list.Add(new Temporary<Collider>(collision.collider));
                gameEntity.ReplaceCollisions(gameEntity.collisions.list);
            }
            else
            {
                gameEntity.AddCollisions(new List<Temporary<Collider>>
                {
                    new Temporary<Collider>(collision.collider)
                });
            }
        }

        private void OnCollisionExit(CollisionListener listener, UnityEngine.Collision collision)
        {
            if (!(listener.GetEntityByLink() is GameEntity {hasCollisions: true} gameEntity))
            {
                return;
            }
            
            bool isChanged = false;
            foreach (var temporaryCollision in gameEntity.collisions.list)
            {
                if (temporaryCollision.Data == collision.collider)
                {
                    isChanged = true;
                    temporaryCollision.Status = TemporaryStatus.Ended;
                }
            }
            if (isChanged)
            {
                gameEntity.ReplaceCollisions(gameEntity.collisions.list);
            }
        }

        private void OnTriggerEnter(CollisionListener listener, Collider trigger)
        {
            if (!(listener.GetEntityByLink() is GameEntity gameEntity))
            {
                return;
            }
            if (gameEntity.hasTriggers)
            {
                gameEntity.triggers.list.Add(new Temporary<Collider>(trigger));
                gameEntity.ReplaceTriggers(gameEntity.triggers.list);
            }
            else
            {
                gameEntity.AddTriggers(new List<Temporary<Collider>>
                {
                    new Temporary<Collider>(trigger)
                });
            }
        }

        private void OnTriggerExit(CollisionListener listener, Collider trigger)
        {
            if (!(listener.GetEntityByLink() is GameEntity {hasTriggers: true} gameEntity))
            {
                return;
            }
            bool isChanged = false;
            foreach (var temporaryTrigger in gameEntity.triggers.list)
            {
                if (temporaryTrigger.Data == trigger)
                {
                    isChanged = true;
                    temporaryTrigger.Status = TemporaryStatus.Ended;
                }
            }
            if (isChanged)
            {
                gameEntity.ReplaceTriggers(gameEntity.triggers.list);
            }
        }
        
        #endregion
        ////////////////////////////////////////////////////////////////////
    }
}