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
    public partial class WelcomePage : Form
    {
        public WelcomePage( )
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.StartPosition = FormStartPosition.Manual;
            login.Location = this.Location;
            this.Hide();
            login.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Signup signup = new Signup();
            signup.StartPosition = FormStartPosition.Manual;
            signup.Location = this.Location;
            this.Hide();
            signup.Show();
        }

        private void WelcomePage_Load(object sender, EventArgs e)
        {

        }
    }
}
