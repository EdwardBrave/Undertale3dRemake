using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;

namespace Game.InitLogic
{
    public class InitFromSceneSystem : IExecuteSystem
    {
        private static readonly Queue<List<IGameComponent>> InitQueue = new Queue<List<IGameComponent>>();
        
        private readonly GameContext _gameContext;

        public static void AddToInitQueue(List<IGameComponent> entityInitList)
        {
            InitQueue.Enqueue(entityInitList);
        }

        public InitFromSceneSystem(Contexts contexts)
        {
            _gameContext = contexts.game;
        }

        public void Execute()
        {
            while (InitQueue.Count > 0)
            {
                var initList = InitQueue.Dequeue();
                var entity = _gameContext.CreateEntity();
                foreach (var component in initList)
                {
                    if (component is ViewComponent view)
                    {
                        view.obj.Link(entity);
                    }
                    int index = Array.IndexOf(GameComponentsLookup.componentTypes, component.GetType());
                    entity.AddComponent(index, component);
                }
            }
        }
    }
}