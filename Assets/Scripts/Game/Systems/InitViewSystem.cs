using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;
using JetBrains.Annotations;
using UnityEngine;

namespace Logic.Systems.Game
{
    public class InitViewSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _context;
        private readonly Transform _parent;
        public InitViewSystem(Contexts contexts): base(contexts.game)
        {
            _context = contexts.game;
            _parent = new GameObject("Views").transform;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Prefab);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasPrefab;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                var prefab = Resources.Load<GameObject>(e.prefab.path);
                if (!prefab)
                    continue;
                if (e.hasView)
                {
                    e.view.obj.Unlink();
                    e.view.obj.DestroyGameObject();
                }
                var view = Object.Instantiate(prefab, _parent);
                e.ReplaceView(view);
                if (e.hasAnimator)
                    e.ReplaceAnimator(view.GetComponent<Animator>());
                view.Link(e);
            }
        }
    }
}
