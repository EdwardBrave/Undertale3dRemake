using System.Collections.Generic;
using Entitas;

namespace Logic.Systems.Game
{
    public class EcsSetupSystem: IExecuteSystem
    {
        public static List<EcsSetuper> setupQueue;
        
        private readonly GameContext _context;
        public EcsSetupSystem(Contexts contexts)
        {
            _context = contexts.game;
        }
        
        public void Execute()
        {
            foreach(var setuper in setupQueue)
            {
                var entity = _context.CreateEntity();
            }
        }
    }
}