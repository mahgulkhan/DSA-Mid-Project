using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DSA_mid_project
{
    public class UserCrud 
    {
        private UserList userList = new UserList();
        private FriendsList friendsList = new FriendsList();

        bool isDataLoaded = false;

        public UserCrud()
        {
            LoadUsersFromDatabase();
            LoadFriendsFromDatabase();
        }

        public void LoadUsersFromDatabase()
        {
            if (isDataLoaded) return;

            string query = "SELECT * FROM users";
            var reader = DSA_mid_project.DatabaseHelper.Instance.getData(query);

            while (reader.Read())
            {
                Users user = new Users
                {
                    UserId = Convert.ToInt32(reader["user_id"]),
                    Username = reader["username"].ToString(),
                    Password = reader["password"].ToString(),
                    Email = reader["email"].ToString(),
                };
                userList.InsertAtEnd(user);
            }

            reader.Close();
            isDataLoaded = true;

        }
        private void LoadFriendsFromDatabase()
        {
            string query = "SELECT f.*, u.username as friend_username FROM friends f JOIN users u ON f.user_id2 = u.user_id";

            var reader = DatabaseHelper.Instance.getData(query);

            while (reader.Read())
            {
                int userId = Convert.ToInt32(reader["user_id1"]);
                int friendId = Convert.ToInt32(reader["user_id2"]);
                DateTime created_at = Convert.ToDateTime(reader["created_at"]);
                string friendUsername = reader["friend_username"].ToString();

                friendsList.AddFriend(userId, friendId, friendUsername, created_at);
            }
            reader.Close();
        }

        public bool Login(string username, string password)
        {

            Users user = userList.FindUserByUsername(username);

            if (user == null)
            {
                MessageBox.Show("User not found!");
                return false;
            }

            if (user.Password != password)
            {
                MessageBox.Show("Incorrect password!");
                return false;
            }
            return true;
        }


        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public bool ValidateUsername(string username)
        {
            return username.All(char.IsLetter);
        }

        public bool ValidateEmail(string email)
        {
            return email.Contains('@');
        }

        public bool ValidatePassword(string password)
        {
            return password.Length >= 6 && password.All(char.IsDigit);
        }

        private string GetUsernameFromDatabase(int userId)
        {
            string query = $"SELECT username FROM users WHERE user_id = {userId}";
            var reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                return reader["username"].ToString();
            }
            return $"User_{userId}";
        }

        public bool AddFriend(int friendId)
        {
            int currentUserId = SessionManager.UserID;

            DateTime createdAt = DateTime.Now;
            string friendUsername = GetUsernameFromDatabase(friendId);
            friendsList.AddFriend(currentUserId, friendId,friendUsername,createdAt); // to list

            // to database
            string query = $"INSERT INTO friends (user_id1, user_id2) VALUES ({currentUserId}, {friendId})";
            return DatabaseHelper.Instance.Update(query) > 0;
        }


        public bool CreateAccount(string username, string password, string email)
        {
            if (userList.FindUserByUsername(username) != null)
            {
                MessageBox.Show("Username already exists!");
                return false;
            }

            string query = $"INSERT INTO users (username, password, email) VALUES ('{username}', '{password}', '{email}')";
            var reader1 = DatabaseHelper.Instance.Update(query);

            if (reader1 > 0)
            {
                string getUserIdQuery = $"SELECT user_id FROM users WHERE username = '{username}'";
                var reader = DatabaseHelper.Instance.getData(getUserIdQuery);

                if (reader.Read())
                {
                    int newUserId = Convert.ToInt32(reader["user_id"]);

                    Users newUser = new Users(newUserId, username, password, email);
                    userList.InsertAtEnd(newUser);
                    reader.Close();

                    return true;
                }
                reader.Close();
            }
            return false;
        }

        public Users ViewProfile(int userId)
        {
            return userList.FindUserById(userId);
        }

      
        public Users FindProfile(string username)
        {
            return userList.FindUserByUsername(username);
        }

        
        public bool EditEmail(string username, string oldEmail, string newEmail)
        {
            Users user = userList.FindUserByUsername(username);

            if (user == null)
            {
                MessageBox.Show("User not found!");
                return false;
            }

           
            string tempEmail = user.Email;

            {
                if (tempEmail != oldEmail)
                {
                    MessageBox.Show("Old email does not match!");
                    return false;
                }
            }

            user.Email = newEmail;

            string query = $"UPDATE users SET email = '{newEmail}' WHERE username = '{username}' AND user_id ={SessionManager.UserID} ";
            var reader = DatabaseHelper.Instance.Update(query);

            if (reader > 0)
            {
                return true;
            }
            else
            {
                user.Email = oldEmail;
                return false;
            }
        }

        public bool EditPassword(string username, string newPassword, string oldPassword)
        {

            Users user = userList.FindUserByUsername(username);
            if (user == null)
            {
                MessageBox.Show("User not found!");
                return false;
            }

            string tempoldPass = user.Password;
            {
                if (tempoldPass != oldPassword)
                {
                    MessageBox.Show("Old password does not match!");
                    return false;
                }
            }

            user.Password = newPassword;

            string query = $"UPDATE users SET password = '{newPassword}' WHERE username = '{username}' AND user_id = {SessionManager.UserID} ";
            var reader = DatabaseHelper.Instance.Update(query);

            if (reader > 0)
            {
                return true;
            }
            else
            {
                user.Password = oldPassword;
                return false;
            }
        }

        public bool ForgotPassword(string username, string newPassword, string email)
        {
            Users user = userList.FindUserByUsername(username);
            if (user == null)
            {
                MessageBox.Show("User not found!");
                return false;
            }

            if (user.Email != email)
            {
                MessageBox.Show("Email does not match!");
                return false;
            }

            string hashedPassword = HashPassword(newPassword);

            if (user.Password == hashedPassword)
            {
                MessageBox.Show("New password cannot be the same as the old password!");
                return false;
            }

            bool listUpdated = userList.UpdateUserPassword(username, hashedPassword);

            string query = $"UPDATE users SET password = '{hashedPassword}' WHERE username = '{username}'";
            bool dbUpdated = DatabaseHelper.Instance.Update(query) > 0;

            return listUpdated && dbUpdated;
        }

        public DataTable GetFriendsList()
        {
           
            DataTable dt = new DataTable();
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("created_at", typeof(DateTime));

            List<int> friendIds = friendsList.GetFriends(SessionManager.UserID);
            foreach (int friendId in friendIds)
            {
                Friends friend = friendsList.FindFriendById(friendId);
                if (friend != null)
                {
                    dt.Rows.Add(friend.friendUsername, friend.created_at);
                }
            }
            return dt;
        }


        public DataTable GetFriendsListAlphabetical()
        {
            DataTable dt = GetFriendsList();

            //Bubble Sort
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dt.Rows.Count - i - 1; j++)
                {
                    DataRow row1 = dt.Rows[j];
                    DataRow row2 = dt.Rows[j + 1];

                    if (string.Compare(row1["Username"].ToString(), row2["Username"].ToString()) > 0)
                    {
                        DataTable tempTable = dt.Clone();
                        tempTable.ImportRow(row1);
                        tempTable.ImportRow(row2);

                        dt.Rows[j].ItemArray = tempTable.Rows[1].ItemArray;
                        dt.Rows[j + 1].ItemArray = tempTable.Rows[0].ItemArray;
                    }
                }
            }

            return dt;
        }

        
        public DataTable FriendsListOldest()
        {
            DataTable dt = GetFriendsList();

            // Insertion Sort 
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                DataRow currentRow = dt.Rows[i];
                DateTime currentDate = Convert.ToDateTime(currentRow["created_at"]);
                int j = i - 1;

                while (j >= 0 && Convert.ToDateTime(dt.Rows[j]["created_at"]) > currentDate)
                {
                    dt.Rows[j + 1].ItemArray = dt.Rows[j].ItemArray;
                    j--;
                }

                dt.Rows[j + 1].ItemArray = currentRow.ItemArray;
            }

            return dt;
        }

        
    }
}
