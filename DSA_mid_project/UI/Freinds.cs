using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DSA_mid_project.UI
{
    public partial class Freinds : Form
    {
        private FlowLayoutPanel friendsFlowPanel;

        public Freinds()
        {
            InitializeComponent();
            LoadFriendsInPanels();
        }

        private void LoadFriendsInPanels()
        {
            friendsFlowPanel = new FlowLayoutPanel
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

            this.Controls.Add(friendsFlowPanel);

            DataTable friends = LaunchPage.userCrud.GetFriendsList();
            foreach (DataRow row in friends.Rows)
            {
                CreateFriendPanel(row);
            }
        }

        private void CreateFriendPanel(DataRow friend)
        {
            string username = friend["Username"].ToString();
            DateTime friendSince = Convert.ToDateTime(friend["created_at"]);

            Panel friendPanel = new Panel
            {
                Size = new Size(760, 80),
                BackColor = Color.FromArgb(24, 24, 24),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 10),
                Tag = username
            };

            friendPanel.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, friendPanel.ClientRectangle,
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
                Text = username.Length > 0 ? username.Substring(0, 1).ToUpper() : "F",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White
            };

            Label nameLabel = new Label
            {
                Location = new Point(60, 25),
                Size = new Size(300, 30),
                Text = username,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label dateLabel = new Label
            {
                Location = new Point(400, 25),
                Size = new Size(300, 30),
                Text = $"Friends since: {friendSince.ToString("MMM dd, yyyy")}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(136, 153, 166),
                BackColor = Color.Transparent
            };

            friendPanel.Controls.Add(avatar);
            friendPanel.Controls.Add(nameLabel);
            friendPanel.Controls.Add(dateLabel);

            friendsFlowPanel.Controls.Add(friendPanel);
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

            if (category == "Alphabetical Order")
            {
                friendsFlowPanel.Controls.Clear();
                DataTable friends = LaunchPage.userCrud.GetFriendsListAlphabetical();
                foreach (DataRow row in friends.Rows)
                {
                    CreateFriendPanel(row);
                }
            }

            if (category == "Oldest to Newest")
            {
                friendsFlowPanel.Controls.Clear();
                DataTable friends = LaunchPage.userCrud.FriendsListOldest();
                foreach (DataRow row in friends.Rows)
                {
                    CreateFriendPanel(row);
                }
            }
        }

        private void Freinds_Load(object sender, EventArgs e)
        {

        }
    }
}