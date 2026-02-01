using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_mid_project
{
    public class FriendsList
    {
        private FriendNode head;

        private class FriendNode
        {
            public int UserId { get; set; }
            public int FriendId { get; set; }
            public string FriendUsername { get; set; }
            public DateTime CreatedAt { get; set; }
            public FriendNode Next { get; set; }

            public FriendNode(int userId, int friendId,string friendUsername, DateTime createdAt)
            {
                UserId = userId;
                FriendId = friendId;
                FriendUsername = friendUsername;
                Next = null;
                CreatedAt = createdAt;
            }
        }

        public void AddFriend(int userId, int friendId,string friendUsername, DateTime created_at)
        {
            FriendNode newNode = new FriendNode(userId, friendId,friendUsername, created_at);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                FriendNode temp = head;
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = newNode;
            }
        }

        public Friends FindFriendById(int userId)
        {
            FriendNode temp = head;
            while (temp != null)
            {
                if (temp.UserId == userId || temp.FriendId == userId)
                {
                    return new Friends
                    {
                        user_id1 = temp.UserId,
                        user_id2 = temp.FriendId,
                        created_at = temp.CreatedAt,
                        friendUsername = GetUsernameById(temp.FriendId)
                    };
                }
                temp = temp.Next;
            }
            return null;
        }

        public string GetUsernameById(int userId2)
        {
            FriendNode temp = head;
            while (temp != null)
            {
                if (temp.FriendId == userId2)
                {
                    return temp.FriendUsername; 
                }
                temp = temp.Next;
            }
            return "Unknown User";
        }
        public List<int> GetFriends(int userId)
        {
            List<int> friends = new List<int>();
            FriendNode temp = head;

            while (temp != null)
            {
                if (temp.UserId == userId)
                {
                    friends.Add(temp.FriendId);
                }
                temp = temp.Next;
            }
            return friends;
        }
    }
}
