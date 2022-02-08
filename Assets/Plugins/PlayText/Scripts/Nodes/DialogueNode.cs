using PlayTextSupport;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using XNode;

namespace GraphSpace
{
    [NodeWidth(320)]
    [CreateNodeMenu("Dialogue")]
    [NodeTint("#00CED1")]//深绿宝石
    public class DialogueNode : Node
    {
        [Input] public Empty Input;
        [Output] public Empty Output;
        public string TalkingPerson;
        public string Expression;
        public FacingDirection Facing;
        public string FacingPerson;
        public float Width;
        public bool ShowBubble = true;
        public bool CameraFollow = true;
        public AudioClip Audio;
        public bool PlayPerChar = false;
        public bool PlayTyping = false;

        [TextArea(5,5)]
        public List<string> Dialogue = new List<string>();

        public bool IsMax = true;
        public int curIndex = 0;

        private void Awake()
        {
#if UNITY_EDITOR
            CameraFollow = XNodeEditor.NodeEditorPreferences.GetSettings().CameraFollow;
#endif
        }

        // Use this for initialization
        protected override void Init()
        {
            base.Init();
            curIndex = 0;
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return Dialogue;
        }

        public string GetBriefDialog()
        {
            string temp = string.Empty;
            if (Dialogue.Count >= 1)
            {
                temp = Dialogue[0];
                if (temp.IndexOf('\n') > 0)
                    temp = Regex.Match(Dialogue[0], @".+(?=\n)").Value;
                if (temp.Length >= 15)
                {
                    temp = temp.Substring(0, 14) + "…";
                }
                temp = TalkingPerson.ToString() + ": " + temp;
            }
            else
            {
                temp = "Dialogue";
            }
            return temp;
        }

        public Node MoveNext()
        {
            if(curIndex + 1 < Dialogue.Count)
            {
                curIndex++;
                return this;
            }
            else
            {
                curIndex = 0;
                NodePort exitPort = GetOutputPort("Output");

                if (!exitPort.IsConnected)
                {
                    EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
                    return this;
                }

                Node node = exitPort.Connection.node;

                if (DialogueGraph.IsVaildNodeForMoveNext(node))
                    return node;
                else
                {
                    EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
                    return this;
                }
            }
        }
    }
}