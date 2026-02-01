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
        public Profile()
        {
            InitializeComponent();
            dataGridView1.DataSource = GetCurrentUserProfile();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        public DataTable GetCurrentUserProfile()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            Users user = LaunchPage.userCrud.ViewProfile(SessionManager.UserID);

            if (user != null)
            {
                dt.Rows.Add(user.UserId, user.Username, user.Email);
            }

            return dt;
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
            checkBox1.Visible = true;
            checkBox2.Visible = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && checkBox2.Checked)
            {
                MessageBox.Show("Please select only one option at a time.");
            }

            if (checkBox1.Checked)
            {
                label3.Visible = true;  
                label4.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                button7.Visible = true;
                label5.Visible = false;
                label6.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && checkBox2.Checked)
            {
                MessageBox.Show("Please select only one option at a time.");
            }

            if (checkBox2.Checked)
            {
                label5.Visible = true;
                label6.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible =  true;
                button7.Visible = true;
                label4.Visible = false;
                label3.Visible = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Please fill in all the fields for updating profile.");
                    return;
                }

                if (!LaunchPage.userCrud.ValidateEmail(textBox2.Text))
                {
                    MessageBox.Show("Invalid Email format. Email must contain '@'.");
                    return;
                }

                bool updateEmailSuccess = LaunchPage.userCrud.EditEmail(SessionManager.Username, textBox1.Text, textBox2.Text);
                if (updateEmailSuccess)
                {
                    MessageBox.Show("Email updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update email. Please check your inputs.");
                }
            }

            if (checkBox2.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Please fill in all the fields for updating profile.");
                    return;
                }

                if (!LaunchPage.userCrud.ValidatePassword(textBox2.Text))
                {
                    MessageBox.Show("Invalid Password format. Password must be at least 6 characters long.");
                    return;
                }

                bool updatePasswordSuccess = LaunchPage.userCrud.EditPassword(SessionManager.Username, textBox1.Text, textBox2.Text);
                if (updatePasswordSuccess)
                {
                    MessageBox.Show("Password updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update password. Please check your inputs.");
                }
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

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
    }
}
