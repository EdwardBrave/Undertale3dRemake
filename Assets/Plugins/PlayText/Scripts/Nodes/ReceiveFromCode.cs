using PlayTextSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using XNode;

namespace GraphSpace
{
    [NodeWidth(300)]
    [CreateNodeMenu("Receive From Code")]
    [NodeTint("#89CFF0")]//Baby Blue
    public class ReceiveFromCode : Node
    {
        [Tooltip("Event that will sent to EventCenter to get result back")]
        public string EventName = string.Empty;
        [Input] public Empty Input;
        [Tooltip("Values that get back from event")]
        [Output(dynamicPortList = true)] public List<ReceiveValueClass> ValueForCheck = new List<ReceiveValueClass>();
        List<ReceiveValueClass> SentReceiveValues = new List<ReceiveValueClass>();

        [HideInInspector]
        public bool IsWaiting = false;
        string guid = string.Empty;
        int index = 0;

        // Use this for initialization
        protected override void Init()
        {
            base.Init();
            name = "Receive From";
            IsWaiting = false;
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }

        public void Trigger()
        {
            Debug.Log("Trigger");
            IsWaiting = true;
            SentReceiveValues.Clear();
            SentReceiveValues.AddRange(ValueForCheck);
            if (guid == string.Empty)
                guid = Guid.NewGuid().ToString();
            if (SentReceiveValues.Count == 0)
            {
                ReceiveValueClass nValue = new ReceiveValueClass();
                nValue.guid = guid;
                SentReceiveValues.Add(nValue);
            }
            else if (SentReceiveValues[0].guid != guid)
                SentReceiveValues[0].guid = guid;
            Debug.Log(guid);
            Debug.Log(SentReceiveValues[0].guid);
            EventCenter.GetInstance().AddEventListener<int>("PlayText.FinishWait." + guid, FinishWaiting);
            EventCenter.GetInstance().EventTriggered(EventName, SentReceiveValues);
        }

        public void FinishWaiting(int i)
        {
            Debug.Log("FinishWaiting");
            EventCenter.GetInstance().RemoveEventListener<int>("PlayText.FinishWait." + guid, FinishWaiting);
            index = i;
            IsWaiting = false;
        }

        public static void FinishThisNode(List<ReceiveValueClass> Value, int index)
        {
            Debug.Log("FinishThisNode");
            string uid = Value[0].guid;
            EventCenter.GetInstance().EventTriggered("PlayText.FinishWait." + uid, index);
        }

        public Node MoveNext()
        {
            if (IsWaiting)
                return this;

            Node temp = this;
            List<NodePort> Port = DynamicOutputs.ToList();

            if (!Port[index].IsConnected) return temp;
            if(DialogueGraph.IsVaildNodeForMoveNext(Port[index].Connection.node))
                temp = Port[index].Connection.node;

            if (temp == this)
            {
                EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
                return temp;
            }
            else
            {
                return temp;
            }

            //NodePort exitPort = GetOutputPort("Output");
            //if (!exitPort.IsConnected)
            //{
            //    EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
            //    return null;
            //}

            //Node node = exitPort.Connection.node;
            //DialogueNode dia = node as DialogueNode;
            //if (dia != null)
            //{
            //    return dia;
            //}

            //OptionNode opt = node as OptionNode;
            //if (opt != null)
            //{
            //    return opt;
            //}

            //EventNode evt = node as EventNode;
            //if (evt != null)
            //{
            //    return evt;
            //}

            //EventCenter.GetInstance().EventTriggered("PlayText.TalkingFinished");
            //return null;
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
public class ReceiveValueClass
{
    public string Value = string.Empty;
    [HideInInspector]
    public string guid;
}