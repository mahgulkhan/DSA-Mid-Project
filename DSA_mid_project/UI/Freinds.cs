using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DSA_mid_project.UI
{
    public partial class Freinds : Form
    {
        public Freinds()
        {
            InitializeComponent();
            dataGridView3.DataSource = LaunchPage.userCrud.GetFriendsList(); 
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            FormDataBL previousFormData = NavigationStack.Pop();

            if (previousFormData?.FormInstance != null)
            {
                previousFormData.FormInstance.StartPosition = FormStartPosition.Manual;
                previousFormData.FormInstance.Location = this.Location;
                previousFormData.FormInstance.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("No previous page!");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string friendUsername = textBox1.Text;

            if (string.IsNullOrEmpty(friendUsername))
            {
                MessageBox.Show("Please enter a username.");
                return;
            }

            Users user = LaunchPage.userCrud.FindProfile(friendUsername);
            if (user != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Username", typeof(string));
                dt.Rows.Add(user.Username);

                dataGridView1.DataSource = dt;
            }
            else
            {
                dataGridView1.DataSource = null;
                MessageBox.Show("User not found.");
            }
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string category = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Please select a sorting category.");
                return;
            }
            
            if(category == "Alphabetical Order")
            {
                dataGridView2.DataSource = LaunchPage.userCrud.GetFriendsListAlphabetical();
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            if (category == "Oldest to Newest")
            {
                dataGridView2.DataSource = LaunchPage.userCrud.FriendsListOldest();
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}
