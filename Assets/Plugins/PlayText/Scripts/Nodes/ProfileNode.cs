using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GraphSpace
{
    [CreateNodeMenu("ProfileNode")]
    [NodeTint("#D8BFD8")]//水绿色
    public class ProfileNode : Node
    {
        public DialogueProfile dialogueProfile;
    }
}