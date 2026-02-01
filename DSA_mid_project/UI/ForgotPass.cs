using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA_mid_project
{
    public partial class ForgotPass : Form
    {
        public ForgotPass()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.StartPosition = FormStartPosition.Manual;
            login.Location = this.Location;
            this.Hide();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Users users = new Users();

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            if (!LaunchPage.userCrud.ValidateUsername(textBox1.Text))
            {
                MessageBox.Show("Invalid Username. Username should not contains symbols, numerals or spaces.");
                return;
            }

            if (!LaunchPage.userCrud.ValidateEmail(textBox3.Text))
            {
                MessageBox.Show("Invalid Email. Please enter a valid email address.");
                return;
            }

            if (!LaunchPage.userCrud.ValidatePassword(textBox2.Text))
            {
                MessageBox.Show("Invalid Password. Password must be at least 6 characters long and should be numeric.");
                return;
            }

            string username = textBox1.Text;
            string tempPassword = textBox2.Text; 
            string email = textBox3.Text;

            bool result = LaunchPage.userCrud.ForgotPassword(username, tempPassword, email);
            if (result)
            {
                MessageBox.Show("Password Updated Successful!");
                Login loginPage = new Login();
                loginPage.StartPosition = FormStartPosition.Manual;
                loginPage.Location = this.Location;
                this.Hide();
                loginPage.Show();
            }
            else
            {
                MessageBox.Show("Updation of password Failed. Please try again.");
                return;
            }
        }
        private void ForgotPass_Load(object sender, EventArgs e)
        {

        }
    }
}
