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
        private FlowLayoutPanel postsFlowPanel;
        private List<PostPanel> postPanels = new List<PostPanel>();

        public Dashboard()
        {
            InitializeComponent();
            InitializePostsPanel();
            LoadPostsInPanels();
            this.AutoScaleMode = AutoScaleMode.Font;
        }


        private void LoadPostsInPanels()
        {
            postsFlowPanel.Controls.Clear();
            postPanels.Clear();

            var allPosts = LaunchPage.postsCrud.GetAllPosts();
            DataTable distinctPosts = RemoveDuplicatesFromDataTable(allPosts);

            foreach (DataRow row in distinctPosts.Rows)
            {
                CreatePostPanel(row);
            }

        }

        private void CreatePostPanel(DataRow post)
        {
            int postId = Convert.ToInt32(post["Sr.no"]);
            string content = post["Content"].ToString();

            // Check if columns exist in the DataTable
            string username = post.Table.Columns.Contains("Username") ? post["Username"].ToString() : "Unknown";
            DateTime postDate = post.Table.Columns.Contains("CreatedAt") ? Convert.ToDateTime(post["CreatedAt"]) : DateTime.Now;
            int likeCount = GetLikeCount(postId);
            int commentCount = GetCommentCount(postId);

            // Create main panel
            Panel postPanel = new Panel
            {
                Size = new Size(780, 200), // CHANGED TO 780
                BackColor = Color.FromArgb(24,24,24),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 10),
                Tag = postId
            };

            // Custom painting for border color
            postPanel.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, postPanel.ClientRectangle,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid);
            };

            // User info panel
            Panel userPanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(760, 40), // CHANGED TO 760
                BackColor = Color.Transparent
            };

            // User avatar (placeholder)
            Label avatar = new Label
            {
                Location = new Point(0, 0),
                Size = new Size(40, 40),
                BackColor = Color.FromArgb(156, 136, 115),
                Text = username.Length > 0 ? username.Substring(0, 1).ToUpper() : "U",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White
            };

            // User name label
            Label userNameLabel = new Label
            {
                Location = new Point(50, 0),
                Size = new Size(200, 20),
                Text = username,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Post date label
            Label dateLabel = new Label
            {
                Location = new Point(50, 20),
                Size = new Size(200, 20),
                Text = postDate.ToString("MMM dd, yyyy"),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(136, 153, 166),
                BackColor = Color.Transparent
            };

            // Content label
            Label contentLabel = new Label
            {
                Location = new Point(10, 60),
                Size = new Size(760, 80), // CHANGED TO 760
                Text = content,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                MaximumSize = new Size(760, 0), // CHANGED TO 760
                AutoSize = true
            };

            // Calculate content label height
            int contentHeight = TextRenderer.MeasureText(content, contentLabel.Font,
                new Size(760, 0), TextFormatFlags.WordBreak).Height;
            contentLabel.Height = Math.Min(contentHeight, 120);

            // Adjust panel height based on content
            int panelHeight = 140 + contentLabel.Height;
            postPanel.Height = panelHeight;

            // Interaction panel
            Panel interactionPanel = new Panel
            {
                Location = new Point(10, contentLabel.Bottom + 10),
                Size = new Size(760, 40), // CHANGED TO 760
                BackColor = Color.Transparent
            };

            // Like button
            Button likeButton = new Button
            {
                Location = new Point(0, 0),
                Size = new Size(120, 30),
                Text = "❤️ Like",
                Tag = postId,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(224, 36, 94),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };
            likeButton.FlatAppearance.BorderSize = 0;
            likeButton.Click += LikeButton_Click;

            // Like count label
            Label likeCountLabel = new Label
            {
                Location = new Point(125, 5),
                Size = new Size(50, 20),
                Text = likeCount.ToString(),
                ForeColor = Color.FromArgb(136, 153, 166),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9),
                Tag = postId
            };

            // Comment button
            Button commentButton = new Button
            {
                Location = new Point(180, 0),
                Size = new Size(120, 30),
                Text = "💬 Comment",
                Tag = postId,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(29, 161, 242),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };
            commentButton.FlatAppearance.BorderSize = 0;
            commentButton.Click += CommentButton_Click;

            // Comment count label
            Label commentCountLabel = new Label
            {
                Location = new Point(305, 5),
                Size = new Size(50, 20),
                Text = commentCount.ToString(),
                ForeColor = Color.FromArgb(136, 153, 166),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9),
                Tag = postId
            };

            // Add controls to user panel
            userPanel.Controls.Add(avatar);
            userPanel.Controls.Add(userNameLabel);
            userPanel.Controls.Add(dateLabel);

            // Add controls to interaction panel
            interactionPanel.Controls.Add(likeButton);
            interactionPanel.Controls.Add(likeCountLabel);
            interactionPanel.Controls.Add(commentButton);
            interactionPanel.Controls.Add(commentCountLabel);

            // Add all to main post panel
            postPanel.Controls.Add(userPanel);
            postPanel.Controls.Add(contentLabel);
            postPanel.Controls.Add(interactionPanel);

            // Store panel information
            PostPanelInfo panelInfo = new PostPanelInfo
            {
                Panel = postPanel,
                LikeButton = likeButton,
                LikeCountLabel = likeCountLabel,
                CommentCountLabel = commentCountLabel,
                PostId = postId
            };
            postPanels.Add(new PostPanel(panelInfo));

            // Update like button appearance
            UpdateLikeButtonAppearance(likeButton, postId);

            // Add to flow panel
            postsFlowPanel.Controls.Add(postPanel);
        }

        private void UpdateLikeButtonAppearance(Button likeButton, int postId)
        {
            bool hasLiked = LaunchPage.postsCrud.HasUserLikedPost(postId);
            if (hasLiked)
            {
                likeButton.Text = "❤️ Liked";
                likeButton.ForeColor = Color.FromArgb(224, 36, 94);
            }
            else
            {
                likeButton.Text = "🤍 Like";
                likeButton.ForeColor = Color.FromArgb(136, 153, 166);
            }
        }

        private void LikeButton_Click(object sender, EventArgs e)
        {
            Button likeButton = (Button)sender;
            int postId = (int)likeButton.Tag;

            bool hasLiked = LaunchPage.postsCrud.HasUserLikedPost(postId);

            if (hasLiked)
            {
                // Unlike the post
                LaunchPage.browsingHistory.SaveAction("LIKE", postId, "");
                bool success = LaunchPage.postsCrud.UnlikePost(postId);
                if (success)
                {
                    likeButton.Text = "🤍 Like";
                    likeButton.ForeColor = Color.FromArgb(136, 153, 166);
                    UpdateLikeCount(postId);
                }
            }
            else
            {
                // Like the post
                bool success = LaunchPage.postsCrud.LikePost(postId);
                if (success)
                {
                    likeButton.Text = "❤️ Liked";
                    likeButton.ForeColor = Color.FromArgb(224, 36, 94);
                    UpdateLikeCount(postId);
                }
            }
        }

        private void UpdateLikeCount(int postId)
        {
            foreach (var postPanel in postPanels)
            {
                if (postPanel.Info.PostId == postId)
                {
                    int likeCount = GetLikeCount(postId);
                    postPanel.Info.LikeCountLabel.Text = likeCount.ToString();
                    break;
                }
            }
        }

        private int GetLikeCount(int postId)
        {
            // Simple method - returns the count from existing data
            var allPosts = LaunchPage.postsCrud.GetAllPosts();

            foreach (DataRow row in allPosts.Rows)
            {
                if (row["Sr.no"].ToString() == postId.ToString())
                {
                    if (row.Table.Columns.Contains("Likes"))
                    {
                        string countStr = row["Likes"].ToString();
                        if (int.TryParse(countStr, out int count))
                            return count;
                    }
                }
            }

            return 0;
        }

        private void CommentButton_Click(object sender, EventArgs e)
        {
            Button commentButton = (Button)sender;
            int postId = (int)commentButton.Tag;

            ShowCommentInput(postId);
        }

        private void ShowCommentInput(int postId)
        {
            foreach (var postPanel in postPanels)
            {
                if (postPanel.Info.PostId == postId)
                {
                    // Check if comment input already exists
                    foreach (Control control in postPanel.Info.Panel.Controls)
                    {
                        if (control.Tag != null && control.Tag.ToString() == "commentInput")
                        {
                            control.Visible = true;
                            return;
                        }
                    }

                    // Create comment input panel
                    Panel commentPanel = new Panel
                    {
                        Location = new Point(10, postPanel.Info.Panel.Height - 40),
                        Size = new Size(760, 80), // CHANGED TO 760
                        BackColor = Color.FromArgb(200, 200, 200),
                        Tag = "commentInput"
                    };

                    TextBox commentBox = new TextBox
                    {
                        Location = new Point(10, 10),
                        Size = new Size(600, 30), // CHANGED BACK TO 600
                        Multiline = false,
                        Font = new Font("Segoe UI", 10),
                        Tag = postId
                    };

                    Button submitCommentBtn = new Button
                    {
                        Location = new Point(620, 10), // CHANGED BACK TO 620
                        Size = new Size(120, 30),
                        Text = "Post Comment",
                        BackColor = Color.FromArgb(29, 161, 242),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9),
                        Tag = postId
                    };
                    submitCommentBtn.FlatAppearance.BorderSize = 0;
                    submitCommentBtn.Click += (s, ev) => SubmitComment(postId, commentBox, commentPanel);

                    commentPanel.Controls.Add(commentBox);
                    commentPanel.Controls.Add(submitCommentBtn);
                    postPanel.Info.Panel.Controls.Add(commentPanel);
                    commentPanel.BringToFront();

                    // Adjust panel height
                    postPanel.Info.Panel.Height += 80;
                    break;
                }
            }
        }

        private void SubmitComment(int postId, TextBox commentBox, Panel commentPanel)
        {
            if (string.IsNullOrEmpty(commentBox.Text))
            {
                MessageBox.Show("Please enter a comment to post.");
                return;
            }

            bool success = LaunchPage.postsCrud.AddComment(postId, commentBox.Text);
            if (success)
            {
                MessageBox.Show("Comment added successfully.");
                commentBox.Clear();
                commentPanel.Visible = false;

                // Update comment count
                UpdateCommentCount(postId);

                // Also refresh the post data
                LaunchPage.postsCrud.LoadPostsFromDatabase();

                // Restore panel height
                commentPanel.Parent.Height -= 80;
            }
            else
            {
                MessageBox.Show("Failed to add comment. Please try again.");
            }
        }

        private void UpdateCommentCount(int postId)
        {
            foreach (var postPanel in postPanels)
            {
                if (postPanel.Info.PostId == postId)
                {
                    int commentCount = GetCommentCount(postId);
                    postPanel.Info.CommentCountLabel.Text = commentCount.ToString();
                    break;
                }
            }
        }

        private int GetCommentCount(int postId)
        {
            // First try to get from DataTable
            var allPosts = LaunchPage.postsCrud.GetAllPosts();

            foreach (DataRow row in allPosts.Rows)
            {
                if (row["Sr.no"].ToString() == postId.ToString())
                {
                    if (row.Table.Columns.Contains("Comments"))
                    {
                        string countStr = row["Comments"].ToString();
                        if (int.TryParse(countStr, out int count))
                            return count;
                    }
                }
            }

            return 0;
        }

        private void InitializePostsPanel()
        {
            // Create FlowLayoutPanel for posts
            postsFlowPanel = new FlowLayoutPanel
            {
                Location = new Point(320, 120),
                Size = new Size(800, 600),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.FromArgb(64, 64, 64),
                BorderStyle = BorderStyle.None,
                Padding = new Padding(10)
            };

            // DISABLE HORIZONTAL SCROLL
            postsFlowPanel.AutoScroll = false; // Disable auto scroll
            postsFlowPanel.HorizontalScroll.Enabled = false;
            postsFlowPanel.HorizontalScroll.Visible = false;
            postsFlowPanel.AutoScroll = true; // Re-enable but horizontal is still disabled
            postsFlowPanel.HorizontalScroll.Maximum = 0; // Set max to 0

            this.Controls.Add(postsFlowPanel);
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
            ShowNewPostPanel();
        }

        private void ShowNewPostPanel()
        {
            Panel newPostPanel = new Panel
            {
                Location = new Point(320, 50),
                Size = new Size(800, 150),
                BackColor = Color.FromArgb(64, 64, 64),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            TextBox postContentBox = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(780, 80),
                Multiline = true,
                Font = new Font("Segoe UI", 11),

            };

            Button postButton = new Button
            {
                Location = new Point(600, 100),
                Size = new Size(90, 30),
                Text = "Post",
                BackColor = Color.FromArgb(29, 161, 242),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            postButton.FlatAppearance.BorderSize = 0;
            postButton.Click += (s, ev) => CreateNewPost(postContentBox, newPostPanel);

            Button cancelButton = new Button
            {
                Location = new Point(700, 100),
                Size = new Size(90, 30),
                Text = "Cancel",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += (s, ev) =>
            {
                this.Controls.Remove(newPostPanel);
                newPostPanel.Dispose();
            };

            newPostPanel.Controls.Add(postContentBox);
            newPostPanel.Controls.Add(postButton);
            newPostPanel.Controls.Add(cancelButton);

            this.Controls.Add(newPostPanel);
            newPostPanel.BringToFront();
        }

        private void CreateNewPost(TextBox postContentBox, Panel newPostPanel)
        {
            string content = postContentBox.Text;
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

                this.Controls.Remove(newPostPanel);
                newPostPanel.Dispose();

                LoadPostsInPanels();
            }
            else
            {
                MessageBox.Show("Failed to add post.");
            }
        }

        // Logout
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

        // Exit
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

        // Sort - Trending
        private void button12_Click(object sender, EventArgs e)
        {
            var trendingPosts = LaunchPage.postsCrud.GetTrendingPosts();
            DataTable distinctPosts = RemoveDuplicatesFromDataTable(trendingPosts);
            LoadPostsFromDataTable(distinctPosts);
        }

        // Sort - Recent
        private void button13_Click(object sender, EventArgs e)
        {
            var recentPosts = LaunchPage.postsCrud.GetRecentPosts();
            DataTable distinctPosts = RemoveDuplicatesFromDataTable(recentPosts);
            LoadPostsFromDataTable(distinctPosts);
        }

        private void LoadPostsFromDataTable(DataTable posts)
        {
            postsFlowPanel.Controls.Clear();
            postPanels.Clear();

            foreach (DataRow row in posts.Rows)
            {
                CreatePostPanel(row);
            }

        }

        // Refresh button
        private void button10_Click(object sender, EventArgs e)
        {
            LoadPostsInPanels();
        }

        // Undo button
        private void button7_Click(object sender, EventArgs e)
        {
            if (!LaunchPage.browsingHistory.CanUndo())
            {
                MessageBox.Show("Nothing to undo!", "Undo",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string lastAction = LaunchPage.browsingHistory.GetLastAction();
            string[] parts = lastAction.Split('|');

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
                    UpdateLikeButtonForPost(postID);
                    UpdateLikeCount(postID);
                    MessageBox.Show("Undid like action!", "Undo");
                    break;
                case "UNLIKE":
                    LaunchPage.postsCrud.LikePost(postID);
                    UpdateLikeButtonForPost(postID);
                    UpdateLikeCount(postID);
                    MessageBox.Show("Undid unlike action!", "Undo");
                    break;
                case "DELETE":
                    bool restored = LaunchPage.postsCrud.RestorePost(postID, postContent);
                    if (restored)
                    {
                        LoadPostsInPanels();
                        MessageBox.Show("Post restored!", "Undo");
                    }
                    else
                    {
                        MessageBox.Show("Could not restore post", "Error");
                    }
                    break;
            }
        }

        private void UpdateLikeButtonForPost(int postId)
        {
            foreach (var postPanel in postPanels)
            {
                if (postPanel.Info.PostId == postId)
                {
                    UpdateLikeButtonAppearance(postPanel.Info.LikeButton, postId);
                    break;
                }
            }
        }

        private DataTable RemoveDuplicatesFromDataTable(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return dataTable;

            DataTable distinctTable = dataTable.Clone();
            HashSet<string> uniqueContent = new HashSet<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Content"] != DBNull.Value)
                {
                    string content = row["Content"].ToString();
                    if (uniqueContent.Add(content))
                    {
                        distinctTable.ImportRow(row);
                    }
                }
            }

            return distinctTable;
        }

        private void NavigateToForm(Form newForm)
        {
            NavigationStack.Push(this);
            this.Hide();
            newForm.Show();
        }

        // Helper class to store post panel information
        private class PostPanelInfo
        {
            public Panel Panel { get; set; }
            public Button LikeButton { get; set; }
            public Label LikeCountLabel { get; set; }
            public Label CommentCountLabel { get; set; }
            public int PostId { get; set; }
        }

        private class PostPanel
        {
            public PostPanelInfo Info { get; set; }

            public PostPanel(PostPanelInfo info)
            {
                Info = info;
            }
        }

        // Sort button
        private void button6_Click(object sender, EventArgs e)
        {
            button12.Visible = true;
            button13.Visible = true;
        }

        // Comment section button
        private void button5_Click(object sender, EventArgs e)
        {
            // This is now handled per-post with the comment button
            MessageBox.Show("Click the comment button on any post to add a comment.");
        }

        // Like/Unlike button
        private void button4_Click(object sender, EventArgs e)
        {
            // This is now handled per-post with the like button
            MessageBox.Show("Click the like button on any post to like/unlike it.");
        }

        // Post comment button
        private void button8_Click(object sender, EventArgs e)
        {
            // This is now handled in the SubmitComment method
            MessageBox.Show("Please use the comment button on a post to add comments.");
        }

        // TextBox text changed
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // This textbox is no longer used in the new design
        }

        // Other event handlers
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void panel6_Paint(object sender, PaintEventArgs e) { }
        private void panel7_Paint(object sender, PaintEventArgs e) { }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e) { }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void Dashboard_Load(object sender, EventArgs e) { }
    }
}