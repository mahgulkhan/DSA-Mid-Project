using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA_mid_project.UI
{
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            string category = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Please select a search category.");
                return;
            }

            if (category == "Search User")
            {
                textBox2.Visible = true;
                button2.Visible = true;
                button4.Visible = true; 
                dataGridView1.Visible = true;
            }


            else if (category == "Search Keyword")
            {
                textBox2.Visible = true;
                button2.Visible = true;
                dataGridView1.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            string category = comboBox1.SelectedItem.ToString();
            if (category == "Search User")
            {
                string username = textBox2.Text;
                Users user = LaunchPage.userCrud.FindProfile(username);
                if (user != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Username", typeof(string));
                    dt.Rows.Add(user.Username);

                    dataGridView1.DataSource = dt;
                    textBox2.Text = null;
                }
                else
                {
                    dataGridView1.DataSource = null;
                    MessageBox.Show("User not found.");
                }
            }

            else if (category == "Search Keyword")
            {
                string keyword = textBox2.Text;
                var searchResults = LaunchPage.postsCrud.SearchPosts(keyword);

                if (searchResults == null || searchResults.Rows.Count == 0)
                {
                    dataGridView1.DataSource = null;
                    MessageBox.Show("No Posts Found matching your keyword");
                    return;
                }

                DataTable distinctResults = RemoveDuplicatesFromDataTable(searchResults);
                dataGridView1.DataSource = distinctResults;
            }
        }

        private DataTable RemoveDuplicatesFromDataTable(DataTable dataTable)
        {
            DataTable distinctTable = dataTable.Clone();
            HashSet<string> uniqueContent = new HashSet<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                string content = row["Content"].ToString();

                if (uniqueContent.Add(content))
                {
                    distinctTable.ImportRow(row);
                }
            }

            return distinctTable;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            button3.Visible = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string friendUsername = textBox1.Text;

            Users friend = LaunchPage.userCrud.FindProfile(friendUsername);

            if (friend != null)
            {
                bool success = LaunchPage.userCrud.AddFriend(friend.UserId);
                if (success)
                {
                    MessageBox.Show("Friend added!");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
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
    }
}
