using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DSA_mid_project
{
    internal class CommentQueue
    {
        private CommentNode front;
        private CommentNode rear;

        private class CommentNode
        {
            public Comments data;
            public CommentNode next;

            public CommentNode(Comments value)
            {
                data = value;
                next = null;
            }
        }

        public void EnqueueComment(Comments comment)
        {
            CommentNode newNode = new CommentNode(comment);
            if (front == null)
            {
                front = rear = newNode;
            }
            else
            {
                rear.next = newNode;
                rear = newNode;
            }
        }

    }
}
