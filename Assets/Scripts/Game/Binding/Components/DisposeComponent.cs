using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Game.Binding.Components
{
    [Game]
    public class DisposeComponent: IComponent
    {
        public class Process
        {
            public const byte Started = 0;
            public const byte Done = 100;
            
            private readonly ISystem _handler;

            //from 0 to 100
            private int _status;

            public int Status
            {
                get => _status;
                set => _status = Mathf.Clamp(Started, value, Done);
            }
            
            public Process(ISystem handler)
            {
                _handler = handler;
                _status = 0;
            }

            public bool IsDone() => _status == Done;
        }

        public List<Process> processes = new List<Process>();
        
        public bool IsAllProcessesDone() => processes.Count == 0 || processes.All(process => process.IsDone());
    }
}