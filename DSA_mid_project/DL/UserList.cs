using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA_mid_project
{
    public class UserList
    {
        private UserNode head;
        private int count;

    
        private class UserNode
        {
            public Users data;
            public UserNode next;

            public UserNode(Users x)
            {
                data = x;
                next = null;
            }
        }

        
        public UserList()
        {
            head = null;
            count = 0;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public int Count => count;

        // Insert at head
        public void InsertAtHead(Users x)
        {
            UserNode newNode = new UserNode(x);
            newNode.next = head;
            head = newNode;
            count++;
        }

        // Insert at end
        public void InsertAtEnd(Users x)
        {
            UserNode newNode = new UserNode(x);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                UserNode temp = head;
                while (temp.next != null)
                {
                    temp = temp.next;
                }
                temp.next = newNode;
            }
            count++;
        }

        public Users FindUserByUsername(string username)
        {

            UserNode temp = head;

            while (temp != null)
            {

                if (temp.data.Username == username)
                    return temp.data;
                temp = temp.next;
            }
            return null;
        }

        public Users FindUserById(int userId)
        {
            UserNode temp = head;
            while (temp != null)
            {
                if (temp.data.UserId == userId)
                    return temp.data;
                temp = temp.next;
            }
            return null;
        }

        public bool UpdateUserPassword(string username, string newPassword)
        {
            UserNode temp = head;
            while (temp != null)
            {
                if (temp.data.Username == username)
                {
                    temp.data.Password = newPassword; 
                    return true;
                }
                temp = temp.next;
            }
            return false;
        }

        // Display list
        public List<Users> DisplayList()
        {
            List<Users> users = new List<Users>();
            UserNode temp = head;

            while (temp != null)
            {
                users.Add(temp.data);
                temp = temp.next;
            }

            return users;
        }
    }
}