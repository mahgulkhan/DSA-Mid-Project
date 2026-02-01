using DSA_mid_project.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DSA_mid_project
{
    public partial class Login : Form
    {
        public Login()
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            WelcomePage welcomePage = new WelcomePage();
            welcomePage.StartPosition = FormStartPosition.Manual;
            welcomePage.Location = this.Location;
            this.Hide();
            welcomePage.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPass forgotPass = new ForgotPass();
            forgotPass.StartPosition = FormStartPosition.Manual;
            forgotPass.Location = this.Location;
            this.Hide();
            forgotPass.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // REMOVE THIS LINE: UserCrud userCrud = new UserCrud();

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            if (!LaunchPage.userCrud.ValidateUsername(textBox1.Text)) // Use LaunchPage.userCrud
            {
                MessageBox.Show("Invalid Username. Username should not contains symbols, numeral or spaces.");
                return;
            }

            if (!LaunchPage.userCrud.ValidatePassword(textBox2.Text)) // Use LaunchPage.userCrud
            {
                MessageBox.Show("Invalid Password. Password must be at least 6 characters long and should be numeric.");
                return;
            }

            string pass = LaunchPage.userCrud.HashPassword(textBox2.Text); // Use LaunchPage.userCrud
            bool loginSuccess = LaunchPage.userCrud.Login(textBox1.Text, pass);

            if (loginSuccess)
            {
                MessageBox.Show("Login Successful!");
                Users user = LaunchPage.userCrud.FindProfile(textBox1.Text);
                SessionManager.Login(user);

                Dashboard dashboard = new Dashboard();
                dashboard.StartPosition = FormStartPosition.Manual;
                dashboard.Location = this.Location;
                this.Hide();
                dashboard.Show();
            }
            else
            {
                MessageBox.Show("Login Failed. Please check your username and password.");
                return;
            }
        
        }
    }
}
