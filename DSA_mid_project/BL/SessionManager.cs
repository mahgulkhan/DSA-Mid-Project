using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_mid_project
{
    public static class SessionManager
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string Email { get; set; }

        public static bool IsLoggedIn()
        {
            return UserID > 0;
        }

        public static void Login(Users user)
        {
            UserID = user.UserId;
            Username = user.Username;
            Email = user.Email;
        }

        public static void Logout()
        {
            UserID = 0;
            Username = "";
            Email = "";
        }
    }
}

