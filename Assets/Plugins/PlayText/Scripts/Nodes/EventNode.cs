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
        public bool WaitCallback = false;
        public List<EventValueClass> EventValue = new List<EventValueClass>();
        List<EventValueClass> SentEventValue = new List<EventValueClass>();
        public bool IsMax = true;
        public bool IsWaiting = false;
        [Input] public Empty Input;
        [Output] public Empty Output;

        string guid = string.Empty;

        // Use this for initialization
        protected override void Init()
        {
            base.Init();
            name = "Event";
            IsWaiting = false;
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }

        public void Trigger()
        {
            IsWaiting = false;
            SentEventValue.Clear();
            SentEventValue.AddRange(EventValue);
            if (WaitCallback)
            {
                IsWaiting = true;
                if (guid == string.Empty)
                    guid = Guid.NewGuid().ToString();
                EventCenter.GetInstance().AddEventListener("PlayText.FinishWait." + guid, FinishWaiting);

                if (SentEventValue.Count == 0)
                {
                    EventValueClass nValue = new EventValueClass();
                    nValue.guid = guid;
                    SentEventValue.Add(nValue);
                }
                else if (SentEventValue[0].guid != guid)
                    SentEventValue[0].guid = guid;
            }
            EventCenter.GetInstance().EventTriggered(EventName, SentEventValue);
        }

        public static void FinishThisNode(List<EventValueClass> Value)
        {
            string uid = Value[0].guid;
            EventCenter.GetInstance().EventTriggered("PlayText.FinishWait." + uid);
        }

        public void FinishWaiting()
        {
            EventCenter.GetInstance().RemoveEventListener("PlayText.FinishWait." + guid, FinishWaiting);
            IsWaiting = false;
        }

        public Node MoveNext()
        {
            if (IsWaiting)
            {
                return this;
            }
            else
            {
                NodePort exitPort = GetOutputPort("Output");
                if (!exitPort.IsConnected)
                {
                    EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
                    return null;
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
    public string guid = string.Empty;
}