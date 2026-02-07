using DSA_mid_project.BL;
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
    public partial class LaunchPage : Form
    {
        public static UserCrud userCrud;
        public static PostsCrud postsCrud;
        public static BrowsingHistory browsingHistory;

        public LaunchPage()
        {
            InitializeComponent();
            userCrud = new UserCrud();
            postsCrud = new PostsCrud();
            browsingHistory = new BrowsingHistory();
            this.DoubleBuffered = true;
        }

        private void button1_Click(object sender, EventArgs e)
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

    }
}
