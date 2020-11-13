using PlayTextSupport;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using XNode;

namespace GraphSpace
{
    [NodeWidth(300)]
    [CreateNodeMenu("Event")]
    [NodeTint("#FF6347")]//番茄色
    public class EventNode : Node
    {
        [Tooltip("Event that will sent to EventCenter")]
        public string EventName = string.Empty;
        public List<EventValueClass> EventValue = new List<EventValueClass>();
        public bool IsMax = true;
        [Input] public Empty Input;
        [Output] public Empty Output;

        // Use this for initialization
        protected override void Init()
        {
            base.Init();
            name = "Event";
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }

        public Node MoveNext()
        {
            NodePort exitPort = GetOutputPort("Output");
            EventCenter.GetInstance().EventTriggered(EventName, EventValue);
            if (!exitPort.IsConnected)
            {
                EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
                return null;
            }

            Node node = exitPort.Connection.node;
            DialogueNode dia = node as DialogueNode;
            if (dia != null)
            {
                return dia as Node;
            }

            OptionNode opt = node as OptionNode;
            if (opt != null)
            {
                return opt as Node;
            }

            EventNode evt = node as EventNode;
            if (evt != null)
            {
                return evt as Node;
            }

            EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
            return null;
        }

        public string GetBriefEvent()
        {
            string temp = string.Empty;
            if (EventName != string.Empty)
            {
                temp = EventName;
                if (temp.IndexOf('\n') > 0)
                    temp = Regex.Match(EventName, @".+(?=\n)").Value;
                if (temp.Length >= 15)
                {
                    temp = temp.Substring(0, 14) + "…";
                }
            }
            return temp;
        }
    }
}
[Serializable]
public class EventValueClass
{
    public int ValueType = 0;
    public int intValue = 0;
    public float floatValue = 0;
    public string stringValue = string.Empty;
    public bool boolValue = false;
}