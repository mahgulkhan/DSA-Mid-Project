using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA_mid_project
{
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            WelcomePage welcomePage = new WelcomePage();
            welcomePage.StartPosition = FormStartPosition.Manual;
            welcomePage.Location = this.Location;
            this.Hide();
            welcomePage.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            UserCrud userCrud = LaunchPage.userCrud;

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            if (!userCrud.ValidateUsername(textBox1.Text))
            {
                MessageBox.Show("Invalid Username. Username should not contains symbols, numeral or spaces.");
                return;
            }

            if(!userCrud.ValidateEmail(textBox3.Text))
            {
                MessageBox.Show("Invalid Email format.");
                return;
            }

            if (!userCrud.ValidatePassword(textBox2.Text))
            {
                MessageBox.Show("Invalid Password. Password must be at least 6 characters long and should be numeric.");
                return;
            }

            string username = textBox1.Text;
            string tempPassword = userCrud.HashPassword(textBox2.Text);
            string mail = textBox3.Text;

            bool result = userCrud.CreateAccount(username,tempPassword,mail);
            if (result)
            {
                MessageBox.Show("Registration Successful!");
                Login loginPage = new Login();
                loginPage.StartPosition = FormStartPosition.Manual;
                loginPage.Location = this.Location;
                this.Hide();
                loginPage.Show();
            }
            else
            {
                MessageBox.Show("Registration Failed. Please try again.");
            }
        }
    }
}
