using DSA_mid_project.BL;
using MySql.Data.MySqlClient;
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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();


            dataGridView1.DataSource = LaunchPage.postsCrud.GetAllPosts();
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.DefaultCellStyle.BackColor = Color.Gray; // Light gray background
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 12f); 
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12f, FontStyle.Bold); 
            dataGridView1.Columns[2].Width = 440;  
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // Search form
        private void button2_Click(object sender, EventArgs e)
        {
            Search search = new Search();
            search.StartPosition = FormStartPosition.Manual;
            search.Location = this.Location;
            NavigateToForm(search);            
        }

        // Posts form
        private void button3_Click(object sender, EventArgs e)
        {
            PostsForm postsForm = new PostsForm();
            postsForm.StartPosition = FormStartPosition.Manual;
            postsForm.Location = this.Location;
            NavigateToForm(postsForm);

        }

        //  logout
        private void button9_Click(object sender, EventArgs e)
        {
            NavigationStack.Clear();
            SessionManager.Logout();
            LaunchPage loginForm = new LaunchPage();
            loginForm.StartPosition = FormStartPosition.Manual;
            loginForm.Location = this.Location;
            this.Hide();
            loginForm.Show();
        }

        // exit
        private void button1_Click(object sender, EventArgs e)
        {
            NavigationStack.Clear();
            SessionManager.Logout();
            Environment.Exit(0);
        }

        // Profile
        private void label2_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile();
            profile.StartPosition = FormStartPosition.Manual;
            profile.Location = this.Location;
            NavigateToForm(profile);

        }

        private void panel3_Paint(object sender, PaintEventArgs e) 
        {
        
        }
        private void panel4_Paint(object sender, PaintEventArgs e) 
        {
        
        }
        private void panel2_Paint(object sender, PaintEventArgs e) 
        {
        
        }
        private void panel1_Paint(object sender, PaintEventArgs e) 
        {
        
        }
        private void pictureBox1_Click(object sender, EventArgs e) 
        {
        
        }
        private void label1_Click(object sender, EventArgs e) 
        {
        
        }
        private void panel5_Paint(object sender, PaintEventArgs e) 
        {
        
        }
        private void panel6_Paint(object sender, PaintEventArgs e) 
        {
        
        }
        private void panel7_Paint(object sender, PaintEventArgs e) 
        {
        
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e) 
        {
        
        }

        // Comment section
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            button8.Visible = true;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter a comment to post.");
                return;
            }
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a post to comment on.");
                return;
            }
            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("Please select only one post to comment on.");
                return;
            }
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int postId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Sr.no"].Value);
                string comment = textBox1.Text;
                bool success = LaunchPage.postsCrud.AddComment(postId, comment);
                if (success)
                {
                    MessageBox.Show("Comment added successfully.");
                    RefreshPostsData();
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show("Failed to add comment. Please try again.");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) 
        {
        
        }

        // Like/Unlike 
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a post to like/unlike.");
                return;
            }

            int postId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Sr.no"].Value);

            // exists in the queue
            Post post = LaunchPage.postsCrud.postQueue.FindPost(postId);
            if (post == null)
            {
                MessageBox.Show("Post not found in queue!");
                return;
            }

            // Check if post exists in database (for foreign key constraint)
            string checkQuery = $"SELECT COUNT(*) FROM posts WHERE post_id = {postId}";
            MySqlDataReader reader = DatabaseHelper.Instance.getData(checkQuery);
            bool postExistsInDB = false;
            if (reader.Read())
            {
                postExistsInDB = reader.GetInt32(0) > 0;
            }
            reader.Close();

            if (!postExistsInDB)
            {
                MessageBox.Show("Post exists in queue but not in database. Please save the post first.");
                return;
            }

            bool hasLiked = LaunchPage.postsCrud.HasUserLikedPost(postId);

            if (hasLiked)
            {
                LaunchPage.browsingHistory.SaveAction("LIKE", postId, "");
                bool success = LaunchPage.postsCrud.UnlikePost(postId);
                if (success)
                {
                    MessageBox.Show("Post unliked!");
                    RefreshPostsData();
                }
            }
            else
            {
                bool success = LaunchPage.postsCrud.LikePost(postId);
                if (success)
                {
                    MessageBox.Show("Post liked!");
                    RefreshPostsData();
                }
            }
        }

        private void RefreshPostsData()
        { 
            LaunchPage.postsCrud.LoadPostsFromDatabase();
            var allPosts = LaunchPage.postsCrud.GetAllPosts();
            DataTable distinctPosts = RemoveDuplicatesFromDataTable(allPosts);
            dataGridView1.DataSource = distinctPosts;
        }

        private DataTable RemoveDuplicatesFromDataTable(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return dataTable;

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



        // Sort 
        private void button6_Click(object sender, EventArgs e)
        {
            button12.Visible = true;
            button13.Visible = true;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            var trendingPosts = LaunchPage.postsCrud.GetTrendingPosts();
            DataTable distinctPosts = RemoveDuplicatesFromDataTable(trendingPosts);
            dataGridView1.DataSource = distinctPosts;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        
        }

        
        private void button13_Click(object sender, EventArgs e)
        {
            var recentPosts = LaunchPage.postsCrud.GetRecentPosts();
            DataTable distinctPosts = RemoveDuplicatesFromDataTable(recentPosts);
            dataGridView1.DataSource = distinctPosts;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Refresh button
        private void button10_Click(object sender, EventArgs e)
        {
            RefreshPostsData();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) 
        {
        
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) 
        {
        
        }

        private void NavigateToForm(Form newForm)
        {
            NavigationStack.Push(this);
            this.Hide();
            newForm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!LaunchPage.browsingHistory.CanUndo())
            {
                MessageBox.Show("Nothing to undo!", "Undo",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // action as a string
            string lastAction = LaunchPage.browsingHistory.GetLastAction();

            // Split it into parts
            string[] parts = lastAction.Split('|');

            // Make sure we have all 3 parts
            if (parts.Length < 3)
            {
                MessageBox.Show("Error: Invalid action data!", "Error");
                return;
            }

            string actionType = parts[0];
            int postID = int.Parse(parts[1]);
            string postContent = parts[2];


            switch (actionType)
            {
                case "LIKE":
                    LaunchPage.postsCrud.UnlikePost(postID);
                    RefreshPostsData();
                    MessageBox.Show("Undid like action!", "Undo");
                    break;
                case "UNLIKE":
                    LaunchPage.postsCrud.LikePost(postID);
                    RefreshPostsData();
                    MessageBox.Show("Undid unlike action!", "Undo");
                    break;
                case "DELETE":
                    bool restored = LaunchPage.postsCrud.RestorePost(postID, postContent);

                    if (restored)
                    {
                        
                        RefreshPostsData();
                        MessageBox.Show("Post restored!", "Undo");

                    }
                    else
                    {
                        MessageBox.Show("Could not restore post", "Error");
                    }
                    break;
            }
        }


        private void Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}