using System;
using Entitas;
using Entitas.Unity;

namespace Game.Binding
{
    public class BindGameEntitySystem: IExecuteSystem
    {
        private readonly IGroup<GameEntity> _bindersGroup;

        public BindGameEntitySystem(Contexts contexts)
        {
            _bindersGroup = contexts.game.GetGroup(GameMatcher.BindEntity);
        }
        
        public void Execute()
        {
            foreach (var entity in _bindersGroup.GetEntities())
            {
                BindEntity(entity, entity.bindEntity.binding);
            }
        }

        private void BindEntity(GameEntity gameEntity, BindGameEntity binding)
        {
            binding.gameObject.Link(gameEntity);
            gameEntity.AddView(binding.gameObject);

            foreach (var component in binding.components)
            {
                int index = Array.IndexOf(GameComponentsLookup.componentTypes, component.GetType());
                if (!gameEntity.HasComponent(index))
                {
                    gameEntity.AddComponent(index, component);
                }
            }
            binding.DestroySelf();
            gameEntity.RemoveBindEntity();
        }
    }
}