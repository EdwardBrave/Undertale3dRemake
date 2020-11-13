using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GraphSpace
{
    [CreateNodeMenu("Comment")]
    public class CommentNode : Node
    {
        [TextArea(3,5)]
        public string Comment;
    }
}

