using System;
using Entitas;
using Entitas.Unity;
using Object = UnityEngine.Object;

namespace Game.Binding
{
    public class CreateGameEntitySystem: IExecuteSystem
    {
        private readonly IGroup<GameEntity> _creatorsGroup;

        public CreateGameEntitySystem(Contexts contexts)
        {
            _creatorsGroup = contexts.game.GetGroup(GameMatcher.CreateEntity);
        }
        
        public void Execute()
        {
            foreach (var entity in _creatorsGroup.GetEntities())
            {
                InitEntity(entity, entity.createEntity.prefab);
            }
        }

        private void InitEntity(GameEntity gameEntity, BindGameEntity basePrefab)
        {
            var newViewObject = Object.Instantiate(basePrefab.gameObject);
            newViewObject.name = basePrefab.gameObject.name;
            newViewObject.Link(gameEntity);
            gameEntity.AddView(newViewObject);
            var loadGameEntity = newViewObject.GetComponent<BindGameEntity>();

            foreach (var component in loadGameEntity.components)
            {
                int index = Array.IndexOf(GameComponentsLookup.componentTypes, component.GetType());
                if (!gameEntity.HasComponent(index))
                {
                    gameEntity.AddComponent(index, component);
                }
            }
            loadGameEntity.DestroySelf();
            gameEntity.RemoveCreateEntity();
        }
    }
}