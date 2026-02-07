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
        private FlowLayoutPanel resultsFlowPanel;

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

            }
            else if (category == "Search Keyword")
            {
                textBox2.Visible = true;
                button2.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            ClearResultsPanel();

            string category = comboBox1.SelectedItem.ToString();
            if (category == "Search User")
            {
                string username = textBox2.Text;
                Users user = LaunchPage.userCrud.FindProfile(username);
                if (user != null)
                {
                    CreateUserPanel(user);
                    textBox2.Text = null;
                }
                else
                {
                    MessageBox.Show("User not found.");
                }
            }
            else if (category == "Search Keyword")
            {
                string keyword = textBox2.Text;
                var searchResults = LaunchPage.postsCrud.SearchPosts(keyword);

                if (searchResults == null || searchResults.Rows.Count == 0)
                {
                    MessageBox.Show("No Posts Found matching your keyword");
                    return;
                }

                DataTable distinctResults = RemoveDuplicatesFromDataTable(searchResults);
                foreach (DataRow row in distinctResults.Rows)
                {
                    CreatePostPanel(row);
                }
            }
        }

        private void ClearResultsPanel()
        {
            if (resultsFlowPanel != null)
            {
                this.Controls.Remove(resultsFlowPanel);
                resultsFlowPanel.Dispose();
            }

            resultsFlowPanel = new FlowLayoutPanel
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

            resultsFlowPanel.HorizontalScroll.Enabled = false;
            resultsFlowPanel.HorizontalScroll.Visible = false;
            resultsFlowPanel.HorizontalScroll.Maximum = 0;

            this.Controls.Add(resultsFlowPanel);
        }

        private void CreateUserPanel(Users user)
        {
            Panel userPanel = new Panel
            {
                Size = new Size(760, 80),
                BackColor = Color.FromArgb(24, 24, 24),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 10),
                Tag = user.UserId
            };

            userPanel.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, userPanel.ClientRectangle,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(119, 110, 101), 1, ButtonBorderStyle.Solid);
            };

            Label avatar = new Label
            {
                Location = new Point(10, 20),
                Size = new Size(40, 40),
                BackColor = Color.FromArgb(156, 136, 115),
                Text = user.Username.Length > 0 ? user.Username.Substring(0, 1).ToUpper() : "U",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White
            };

            Label nameLabel = new Label
            {
                Location = new Point(60, 25),
                Size = new Size(300, 30),
                Text = user.Username,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label emailLabel = new Label
            {
                Location = new Point(400, 25),
                Size = new Size(200, 30),
                Text = user.Email,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(136, 153, 166),
                BackColor = Color.Transparent
            };

            userPanel.Controls.Add(avatar);
            userPanel.Controls.Add(nameLabel);
            userPanel.Controls.Add(emailLabel);

            bool isFriend = IsAlreadyFriend(user.UserId);

            Button friendButton = new Button
            {
                Location = new Point(650, 20),
                Size = new Size(90, 40),
                Text = isFriend ? "Friend" : "Add Friend",
                BackColor = isFriend ? Color.FromArgb(40, 167, 69) : Color.FromArgb(29, 161, 242),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Tag = user.UserId,
                Enabled = !isFriend
            };
            friendButton.FlatAppearance.BorderSize = 0;

            if (!isFriend)
            {
                friendButton.Click += (s, ev) => AddFriendButton_Click(user.UserId, friendButton);
            }

            userPanel.Controls.Add(friendButton);
            resultsFlowPanel.Controls.Add(userPanel);
        }

        private bool IsAlreadyFriend(int userId)
        {
            List<int> friendIds = LaunchPage.userCrud.friendsList.GetFriends(SessionManager.UserID);
            return friendIds.Contains(userId);
        }

        private void AddFriendButton_Click(int friendId, Button friendButton)
        {
            bool success = LaunchPage.userCrud.AddFriend(friendId);
            if (success)
            {
                friendButton.Text = "Friend";
                friendButton.BackColor = Color.FromArgb(40, 167, 69);
                friendButton.Enabled = false;
                MessageBox.Show("Friend added!");
            }
        }

        private void CreatePostPanel(DataRow post)
        {
            int postId = Convert.ToInt32(post["Sr.no"]);
            string content = post["Content"].ToString();
            string username = post.Table.Columns.Contains("Username") ? post["Username"].ToString() : "Unknown";
            DateTime postDate = post.Table.Columns.Contains("CreatedAt") ? Convert.ToDateTime(post["CreatedAt"]) : DateTime.Now;
            int likeCount = Convert.ToInt32(post["Likes"]);
            int commentCount = Convert.ToInt32(post["Comments"]);

            Panel postPanel = new Panel
            {
                Size = new Size(760, 200),
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
                Text = username.Length > 0 ? username.Substring(0, 1).ToUpper() : "U",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White
            };

            Label userNameLabel = new Label
            {
                Location = new Point(50, 0),
                Size = new Size(200, 20),
                Text = username,
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

            Label likeLabel = new Label
            {
                Location = new Point(0, 10),
                Size = new Size(100, 20),
                Text = $"❤️ {likeCount} Likes",
                ForeColor = Color.FromArgb(224, 36, 94),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };

            Label commentLabel = new Label
            {
                Location = new Point(120, 10),
                Size = new Size(120, 20),
                Text = $"💬 {commentCount} Comments",
                ForeColor = Color.FromArgb(29, 161, 242),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9)
            };

            userPanel.Controls.Add(avatar);
            userPanel.Controls.Add(userNameLabel);
            userPanel.Controls.Add(dateLabel);

            interactionPanel.Controls.Add(likeLabel);
            interactionPanel.Controls.Add(commentLabel);

            postPanel.Controls.Add(userPanel);
            postPanel.Controls.Add(contentLabel);
            postPanel.Controls.Add(interactionPanel);

            resultsFlowPanel.Controls.Add(postPanel);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

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

        private void Search_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}