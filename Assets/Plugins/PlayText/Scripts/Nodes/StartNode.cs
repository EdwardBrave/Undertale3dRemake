using PlayTextSupport;
using UnityEngine;
using XNode;

namespace GraphSpace
{
    [CreateNodeMenu("Start")]
    [NodeTint("#6495ED")]//矢车菊的蓝色
    public class StartNode : Node
    {
        private DialogueGraph dialogueGraph;
        [Output] public Empty Ouput;

        public string Language;

        // Use this for initialization
        protected override void Init()
        {
            name = "Start";
            base.Init();
            if (dialogueGraph == null) dialogueGraph = graph as DialogueGraph;
            dialogueGraph.current = this;
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }

        public Node MoveNext()
        {
            NodePort exitPort = GetOutputPort("Ouput");

            if (!exitPort.IsConnected)
            {
                EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
                Debug.LogWarning("Start Node isn't connected");
                return this;
            }

            Node node = exitPort.Connection.node;
            if (DialogueGraph.IsVaildNodeForMoveNext(node))
                return node;
            else
            {
                EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
                Debug.LogWarning("Start Node isn't connected");
                return this;
            }
        }
    }
}