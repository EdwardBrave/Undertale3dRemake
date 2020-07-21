using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;
using UnityEngine;

namespace Systems.Game
{
    public class FindViewSystem: ReactiveSystem<GameEntity>
    {
        private readonly GameContext _context;
        public FindViewSystem(Contexts contexts): base(contexts.game)
        {
            _context = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.SceneObject);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasSceneObject;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                GameObject view = e.sceneObject.isTag
                    ? GameObject.FindWithTag(e.sceneObject.name)
                    : GameObject.Find(e.sceneObject.name);
                if (!view)
                    continue;
                if (e.hasView)
                    e.view.obj.Unlink();
                e.ReplaceView(view);
                if (e.hasAnimator)
                    e.ReplaceAnimator(view.GetComponent<Animator>());
                view.Link(e);
            }
        }
    }
}