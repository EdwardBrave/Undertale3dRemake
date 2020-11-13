using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GraphSpace
{

    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right,
        FacingPerson
    }

    [CreateAssetMenu]
    public class DialogueGraph : NodeGraph
    {
        public StartNode startNode;
        public DialogueNode dialogueNode;
        public OptionNode optionNode;
        public EventNode eventNode;
        public CommentNode commentNode;
        public ProfileNode profileNode;

        public Node current;

        public void Continue(int index = 0)
        {
            OptionNode opt = current as OptionNode;
            DialogueNode dia = current as DialogueNode;
            StartNode sta = current as StartNode;
            if (opt != null)
            {
                current = opt.MoveNext(index);
            }
            else if(dia != null)
            {
                current = dia.MoveNext();
            }
            else if (sta != null)
            {
                current = sta.MoveNext();
            }
            EventNode evt = current as EventNode;
            while (evt != null)
            {
                current = evt.MoveNext();
                evt = current as EventNode;
            }
        }

        public DialogueProfile GetProfile()
        {
            if (profileNode != null)
            {
                if (profileNode.dialogueProfile != null)
                    return profileNode.dialogueProfile;
                else
                {
                    Debug.LogError("Didn't assign profile to Profile Node");
                    return null;
                } 
            }
            else
            {
                Debug.LogError("Didn't create a Profile Node yet");
                return null;
            }
        }

        public int ProfileUpdate()
        {
            List<Node> ListNode = nodes.FindAll(node =>
            {
                return node.GetType() == typeof(ProfileNode);
            });
            if(ListNode.Count == 1)
            {
                profileNode = ListNode[0] as ProfileNode;
                return 1;
            }
            else if(ListNode.Count == 0)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public void Open()
        {
            List<Node> temp =  nodes.FindAll(node =>
            {
                return node.GetType() == typeof(StartNode);
            });
            for (int i = 0; i < temp.Count; i++)
            {
                StartNode tStart = temp[i] as StartNode;
                if(tStart != null)
                {
                    if(tStart.Language == GetProfile().Language[0])
                    {
                        current = tStart;
                    }
                }
            }
        }

        public void SetStartPoint(string lan)
        {
            List<Node> temp = nodes.FindAll(node =>
            {
                return node.GetType() == typeof(StartNode);
            });
            for (int i = 0; i < temp.Count; i++)
            {
                StartNode tStart = temp[i] as StartNode;
                if (tStart != null)
                {
                    if (tStart.Language == lan)
                    {
                        current = tStart;
                    }
                }
            }
            if (current == null)
            {
                Debug.LogError("There is not a starting point for current language");
                Debug.Break();
            }
        }
    }

    [Serializable]
    public class Empty { }
}