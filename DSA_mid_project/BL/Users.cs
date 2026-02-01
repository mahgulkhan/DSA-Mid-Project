using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_mid_project
{
    public class Users
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public Users() { }

        public Users(string username, string password, string email) 
        {
            this.Username = username;
            this.Password = password;
            this.Email = email;
        }

        public Users(int userid, string username, string password, string email)
        {
            this.UserId = userid;
            this.Username = username;
            this.Password = password;
            this.Email = email;
        }
    }
}
