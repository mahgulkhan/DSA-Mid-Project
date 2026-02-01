using DSA_mid_project.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DSA_mid_project.UI
{
    public partial class PostsForm : Form
    {
        public PostsForm()
        {
            InitializeComponent();
            SetupDataGridView();
            HideAllControls();
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
        }

        private void HideAllControls()
        {
            label2.Visible = false;
            textBox1.Visible = false;
            button3.Visible = false;
            label3.Visible = false;
            textBox2.Visible = false;
            button4.Visible = false;
            button7.Visible = false;
            label5.Visible = false;
        }

        private void PostsForm_Load(object sender, EventArgs e)
        {
            LoadPostsData();
        }

        private void LoadPostsData()
        {
            try
            {
                dataGridView1.DataSource = null;

                var posts = LaunchPage.postsCrud.GetUserPosts();

                // Removing duplicates from DataTable
                DataTable distinctTable = RemoveDuplicatesFromDataTable(posts);
                dataGridView1.DataSource = distinctTable;

                if (dataGridView1.Columns.Contains("PostID"))
                {
                    dataGridView1.Columns["PostID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading posts: {ex.Message}");
            }
        }

        private DataTable RemoveDuplicatesFromDataTable(DataTable dataTable)
        {
            DataTable distinctTable = dataTable.Clone(); // temp

            HashSet<string> uniqueContent = new HashSet<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                string content = row["Content"].ToString();

                // If we haven't seen this content before, add it to distinct table
                if (uniqueContent.Add(content))
                {
                    distinctTable.ImportRow(row);
                }
            }

            return distinctTable;
        }

        //add post
        private void button1_Click(object sender, EventArgs e)
        {
            HideAllControls();
            label2.Visible = true;
            textBox1.Visible = true;
            button3.Visible = true;
        }

        //edit Post
        private void button2_Click(object sender, EventArgs e)
        {
            HideAllControls();
            label3.Visible = true;
            textBox2.Visible = true;
            button4.Visible = true;
        }

        //Create Post
        private void button3_Click(object sender, EventArgs e)
        {
            string content = textBox1.Text;
            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Please enter post content.");
                return;
            }

            bool isAdded = LaunchPage.postsCrud.CreatePost(content);

            if (isAdded)
            {
                LaunchPage.browsingHistory.SaveAction("CREATE", -1, content);
                MessageBox.Show("Post added successfully.");
                textBox1.Clear();
                LoadPostsData();
                HideAllControls();
            }
            else
            {
                MessageBox.Show("Failed to add post.");
            }
        }

        //edit post
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a post to edit.");
                return;
            }

            string content = textBox2.Text;
            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Please enter post content.");
                return;
            }

            int postId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PostID"].Value);
            bool isUpdated = LaunchPage.postsCrud.EditPost(postId, content);

            if (isUpdated)
            {
                MessageBox.Show("Post updated successfully.");
                textBox2.Clear();
                LoadPostsData();
                HideAllControls();
            }
            else
            {
                MessageBox.Show("Failed to update post.");
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

        //delete Post
        private void button6_Click_1(object sender, EventArgs e)
        {
            HideAllControls();
            button7.Visible = true;
            label5.Visible = true;
        }


        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a post to delete.");
                return;
            }

            int postId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PostID"].Value);
            string postContent = dataGridView1.SelectedRows[0].Cells["Content"].Value?.ToString() ?? "";

            LaunchPage.browsingHistory.SaveAction("DELETE", postId, postContent);
            bool isDeleted = LaunchPage.postsCrud.DeletePost(postId);

            if (isDeleted)
            {
                MessageBox.Show("Post deleted successfully.");
                LoadPostsData();
                HideAllControls();
            }
            else
            {
                MessageBox.Show("Failed to delete post.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) 
        {
        
        }
        private void textBox1_TextChanged(object sender, EventArgs e) 
        {
        
        }
        private void label4_Click(object sender, EventArgs e) 
        {
        
        }
    }
}