using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_mid_project
{
    public class PostQueue
    {
        private PostNode front;
        private PostNode rear;

        private class PostNode
        {
            public Post data;
            public PostNode next;

            public PostNode(Post value)
            {
                data = value;
                next = null;
            }
        }

        public PostQueue()
        {
            front = rear = null;
        }

        public bool IsEmpty()
        {
            return front == null;
        }

        public void EnqueuePost(Post item)
        {
            PostNode newNode = new PostNode(item);

            if (IsEmpty())
            {
                front = rear = newNode;
            }
            else
            {
                rear.next = newNode;
                rear = newNode;
            }
        }

        public Post DequeuePost()
        {
            if (IsEmpty())
            {
                return null;
            }

            Post item = front.data;
            PostNode temp = front;
            front = front.next;

            if (front == null)
            {
                rear = null;
            }

            return item;
        }

        public Post PeekPost()
        {
            if (IsEmpty())
            {
                return null;
            }
            return front.data;
        }

        public List<Post> GetAllPosts()
        {
            List<Post> posts = new List<Post>();
            PostNode temp = front;

            while (temp != null)
            {
                posts.Add(temp.data);
                temp = temp.next;
            }

            return posts;
        }

        public Post FindPost(int postId)
        {
            PostNode temp = front;

            while (temp != null)
            {
                if (temp.data.PostId == postId)
                    return temp.data;
                temp = temp.next;
            }

            return null;
        }

        public bool DeletePost(int postId)
        {
            if (IsEmpty())
                return false;

            if (front.data.PostId == postId)
            {
                PostNode temp = front;
                front = front.next;
                if (front == null)
                    rear = null;
                return true;
            }

            PostNode current = front;
            while (current.next != null)
            {
                if (current.next.data.PostId == postId)
                {
                    PostNode temp = current.next;
                    current.next = current.next.next;

                    if (temp == rear)
                        rear = current;

                    return true;
                }
                current = current.next;
            }
            return false;
        }

        public bool EditPost(int postId, string newContent)
        {
            PostNode temp = front;

            while (temp != null)
            {
                if (temp.data.PostId == postId)
                {
                    temp.data.Content = newContent;
                    return true;
                }
                temp = temp.next;
            }
            return false;
        }
    }
}
