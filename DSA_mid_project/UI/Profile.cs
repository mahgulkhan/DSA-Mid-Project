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
    public partial class Profile : Form
    {
        private FlowLayoutPanel postsFlowPanel;
        private List<PostPanel> postPanels = new List<PostPanel>();

        public Profile()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            LoadUserProfileData();
            LoadUserPosts();
        }

        private void LoadUserProfileData()
        {
            Users user = LaunchPage.userCrud.ViewProfile(SessionManager.UserID);
            if (user != null)
            {
                label2.Text = user.Username.ToUpper();
            }
        }

        private void LoadUserPosts()
        {
            var userPosts = LaunchPage.postsCrud.GetUserPosts();
            DataTable distinctPosts = RemoveDuplicatesFromDataTable(userPosts);

            int postCount = distinctPosts.Rows.Count;
            int postHeight = 250; // Height of each post panel
            int margin = 10; // Margin between posts
            int padding = 20; // Top + bottom padding
            int maxVisibleHeight = 600; // Maximum visible area

            // Calculate total height needed
            int totalHeightNeeded = (postHeight + margin) * postCount + padding;

            // Set height: use total height if more than max, otherwise use what's needed
            int panelHeight = Math.Min(totalHeightNeeded, maxVisibleHeight);

            postsFlowPanel = new FlowLayoutPanel
            {
                Location = new Point(320, 120),
                Size = new Size(800, panelHeight), // Dynamic height
                AutoScroll = totalHeightNeeded > maxVisibleHeight, // Only scroll if needed
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.FromArgb(64, 64, 64),
                BorderStyle = BorderStyle.None,
                Padding = new Padding(10)
            };

            postsFlowPanel.HorizontalScroll.Enabled = false;
            postsFlowPanel.HorizontalScroll.Visible = false;
            postsFlowPanel.HorizontalScroll.Maximum = 0;

            this.Controls.Add(postsFlowPanel);

            foreach (DataRow row in distinctPosts.Rows)
            {
                CreateUserPostPanel(row);
            }
        }


        private void CreateUserPostPanel(DataRow post)
        {
            int postId = Convert.ToInt32(post["PostID"]);
            string content = post["Content"].ToString();
            DateTime postDate = post.Table.Columns.Contains("CreatedAt") ? Convert.ToDateTime(post["CreatedAt"]) : DateTime.Now;
            int likeCount = Convert.ToInt32(post["Likes"]);
            int commentCount = Convert.ToInt32(post["Comments"]);

            Panel postPanel = new Panel
            {
                Size = new Size(760, 250),
                BackColor = Color.FromArgb(24, 24, 24),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 10),
                Tag = postId
            };

            postPanel.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, postPanel.ClientRectangle,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid);
            };

            Panel userPanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(740, 40),
                BackColor = Color.Transparent
            };

            Label avatar = new Label
            {
                Location = new Point(0, 0),
                Size = new Size(40, 40),
                BackColor = Color.FromArgb(156, 136, 115),
                Text = SessionManager.Username.Length > 0 ? SessionManager.Username.Substring(0, 1).ToUpper() : "U",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White
            };

            Label userNameLabel = new Label
            {
                Location = new Point(50, 0),
                Size = new Size(200, 20),
                Text = SessionManager.Username,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label dateLabel = new Label
            {
                Location = new Point(50, 20),
                Size = new Size(200, 20),
                Text = postDate.ToString("MMM dd, yyyy"),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(136, 153, 166),
                BackColor = Color.Transparent
            };

            Label contentLabel = new Label
            {
                Location = new Point(10, 60),
                Size = new Size(740, 80),
                Text = content,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                MaximumSize = new Size(740, 0),
                AutoSize = true
            };

            int contentHeight = TextRenderer.MeasureText(content, contentLabel.Font,
                new Size(740, 0), TextFormatFlags.WordBreak).Height;
            contentLabel.Height = Math.Min(contentHeight, 120);

            int panelHeight = 140 + contentLabel.Height;
            postPanel.Height = panelHeight;

            Panel interactionPanel = new Panel
            {
                Location = new Point(10, contentLabel.Bottom + 10),
                Size = new Size(740, 40),
                BackColor = Color.Transparent
            };

            Button likeButton = new Button
            {
                Location = new Point(0, 0),
                Size = new Size(80, 30),
                Text = "❤️ Likes",
                Tag = postId,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(224, 36, 94),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };
            likeButton.FlatAppearance.BorderSize = 0;

            Label likeCountLabel = new Label
            {
                Location = new Point(85, 5),
                Size = new Size(40, 20),
                Text = likeCount.ToString(),
                ForeColor = Color.FromArgb(136, 153, 166),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };

            Button commentButton = new Button
            {
                Location = new Point(130, 0),
                Size = new Size(100, 30),
                Text = "💬 Comment",
                Tag = postId,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(29, 161, 242),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };
            commentButton.FlatAppearance.BorderSize = 0;

            Label commentCountLabel = new Label
            {
                Location = new Point(235, 5),
                Size = new Size(40, 20),
                Text = commentCount.ToString(),
                ForeColor = Color.FromArgb(136, 153, 166),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };

            Button editButton = new Button
            {
                Location = new Point(280, 0),
                Size = new Size(80, 30),
                Text = "✏️ Edit",
                Tag = postId,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(29, 161, 242),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };
            editButton.FlatAppearance.BorderSize = 0;
            editButton.Click += EditButton_Click;

            Button deleteButton = new Button
            {
                Location = new Point(365, 0),
                Size = new Size(90, 30),
                Text = "🗑️ Delete",
                Tag = postId,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(220, 53, 69),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.Click += DeleteButton_Click;

            userPanel.Controls.Add(avatar);
            userPanel.Controls.Add(userNameLabel);
            userPanel.Controls.Add(dateLabel);

            interactionPanel.Controls.Add(likeButton);
            interactionPanel.Controls.Add(likeCountLabel);
            interactionPanel.Controls.Add(commentButton);
            interactionPanel.Controls.Add(commentCountLabel);
            interactionPanel.Controls.Add(editButton);
            interactionPanel.Controls.Add(deleteButton);

            postPanel.Controls.Add(userPanel);
            postPanel.Controls.Add(contentLabel);
            postPanel.Controls.Add(interactionPanel);

            PostPanelInfo panelInfo = new PostPanelInfo
            {
                Panel = postPanel,
                LikeButton = likeButton,
                LikeCountLabel = likeCountLabel,
                CommentCountLabel = commentCountLabel,
                PostId = postId
            };
            postPanels.Add(new PostPanel(panelInfo));

            postsFlowPanel.Controls.Add(postPanel);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            Button editButton = (Button)sender;
            int postId = (int)editButton.Tag;

            PostPanel postPanel = null;
            foreach (var panel in postPanels)
            {
                if (panel.Info.PostId == postId)
                {
                    postPanel = panel;
                    break;
                }
            }

            if (postPanel == null)
            {
                MessageBox.Show("Post not found.");
                return;
            }

            int originalHeight = postPanel.Info.Panel.Height;

            Panel editPanel = new Panel
            {
                Location = new Point(10, postPanel.Info.Panel.Height - 40),
                Size = new Size(740, 80),
                BackColor = Color.FromArgb(200, 200, 200),
                Tag = "editInput"
            };

            TextBox editBox = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(600, 30),
                Multiline = false,
                Font = new Font("Segoe UI", 10),
                Tag = postId
            };

            foreach (Control control in postPanel.Info.Panel.Controls)
            {
                if (control is Label label && control.Location.Y == 60)
                {
                    editBox.Text = label.Text;
                    break;
                }
            }

            Button submitEditBtn = new Button
            {
                Location = new Point(620, 10),
                Size = new Size(100, 30),
                Text = "Update",
                BackColor = Color.FromArgb(29, 161, 242),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Tag = postId
            };
            submitEditBtn.FlatAppearance.BorderSize = 0;
            submitEditBtn.Click += (s, ev) => SubmitEdit(postId, editBox, editPanel, postPanel);

            Button cancelEditBtn = new Button
            {
                Location = new Point(620, 45),
                Size = new Size(100, 30),
                Text = "Cancel",
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Tag = postId
            };
            cancelEditBtn.FlatAppearance.BorderSize = 0;
            cancelEditBtn.Click += (s, ev) =>
            {
                editPanel.Visible = false;
                postPanel.Info.Panel.Controls.Remove(editPanel);
                postPanel.Info.Panel.Height = originalHeight;
            };

            editPanel.Controls.Add(editBox);
            editPanel.Controls.Add(submitEditBtn);
            editPanel.Controls.Add(cancelEditBtn);
            postPanel.Info.Panel.Controls.Add(editPanel);
            editPanel.BringToFront();

            postPanel.Info.Panel.Height += 80;
        }

        private void SubmitEdit(int postId, TextBox editBox, Panel editPanel, PostPanel postPanel)
        {
            string content = editBox.Text;
            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Please enter post content.");
                return;
            }

            bool isUpdated = LaunchPage.postsCrud.EditPost(postId, content);

            if (isUpdated)
            {
                MessageBox.Show("Post updated successfully.");

                Label contentLabel = null;
                Panel interactionPanel = null;

                foreach (Control control in postPanel.Info.Panel.Controls)
                {
                    if (control is Panel panel && panel.Location.Y > 60)
                    {
                        interactionPanel = panel;
                    }
                    else if (control is Label label && control.Location.Y == 60)
                    {
                        contentLabel = label;
                    }
                }

                if (contentLabel != null)
                {
                    contentLabel.Text = content;

                    int contentHeight = TextRenderer.MeasureText(content, contentLabel.Font,
                        new Size(740, 0), TextFormatFlags.WordBreak).Height;
                    contentLabel.Height = Math.Min(contentHeight, 120);

                    if (interactionPanel != null)
                    {
                        interactionPanel.Location = new Point(10, contentLabel.Bottom + 10);
                    }

                    postPanel.Info.Panel.Height = 140 + contentLabel.Height;
                }

                editPanel.Visible = false;
                postPanel.Info.Panel.Controls.Remove(editPanel);
            }
            else
            {
                MessageBox.Show("Failed to update post.");
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Button deleteButton = (Button)sender;
            int postId = (int)deleteButton.Tag;

            DialogResult result = MessageBox.Show("Are you sure you want to delete this post?",
                "Confirm Delete", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                bool success = LaunchPage.postsCrud.DeletePost(postId);
                if (success)
                {
                    MessageBox.Show("Post deleted successfully");
                    postsFlowPanel.Controls.Clear();
                    postPanels.Clear();
                    LoadUserPosts();
                }
                else
                {
                    MessageBox.Show("Failed to delete post");
                }
            }
        }

        private DataTable RemoveDuplicatesFromDataTable(DataTable dataTable)
        {
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

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Freinds freinds = new Freinds();
            freinds.StartPosition = FormStartPosition.Manual;
            freinds.Location = this.Location;
            this.Hide();
            freinds.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PostsForm postsForm = new PostsForm();
            postsForm.StartPosition = FormStartPosition.Manual;
            postsForm.Location = this.Location;
            postsForm.Show();
            this.Hide();
        }


        private void button8_Click(object sender, EventArgs e)
        {
            PostsForm postsForm = new PostsForm();
            postsForm.StartPosition = FormStartPosition.Manual;
            postsForm.Location = this.Location;
            this.Hide();
            postsForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void Profile_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}