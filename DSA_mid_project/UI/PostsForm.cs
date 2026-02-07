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
            
        }

       

        private void PostsForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            label2.Visible = true;
            textBox1.Visible = true;
            button3.Visible = true;
        }

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
            }
            else
            {
                MessageBox.Show("Failed to add post.");
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

       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) 
        {
        
        }
        private void textBox1_TextChanged(object sender, EventArgs e) 
        {
        
        }
        private void label4_Click(object sender, EventArgs e) 
        {
        
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all the fields for updating profile.");
                    return;
                }

                if (!LaunchPage.userCrud.ValidateEmail(textBox4.Text))
                {
                    MessageBox.Show("Invalid Email format. Email must contain '@'.");
                    return;
                }

                bool updateEmailSuccess = LaunchPage.userCrud.EditEmail(SessionManager.Username, textBox3.Text, textBox4.Text);
                if (updateEmailSuccess)
                {
                    MessageBox.Show("Email updated successfully.");

                    label7.Visible = false;
                    label8.Visible = false;
                    textBox3.Visible = false;
                    textBox3.Text = string.Empty;
                    textBox4.Visible = false;
                    textBox4.Text = string.Empty;
                    label6.Visible = false;
                    label9.Visible = false;
                }
                else
                {
                    MessageBox.Show("Failed to update email. Please check your inputs.");
                }
            }

            if (checkBox2.Checked)
            {
                if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all the fields for updating profile.");
                    return;
                }

                if (!LaunchPage.userCrud.ValidatePassword(textBox4.Text))
                {
                    MessageBox.Show("Invalid Password format. Password must be at least 6 characters long.");
                    return;
                }

                bool updatePasswordSuccess = LaunchPage.userCrud.EditPassword(SessionManager.Username, textBox1.Text, textBox4.Text);
                if (updatePasswordSuccess)
                {
                    MessageBox.Show("Password updated successfully.");

                    label7.Visible = false;
                    label8.Visible = false;
                    textBox3.Visible = false;
                    textBox3.Text = string.Empty;
                    textBox4.Visible = false;
                    textBox4.Text = string.Empty;
                    label6.Visible = false;
                    label9.Visible = false;
                }
                else
                {
                    MessageBox.Show("Failed to update password. Please check your inputs.");
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label6.Visible = true;
                label9.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                label8.Visible = false;
                label7.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                label7.Visible = true;
                label8.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                label6.Visible = false;
                label9.Visible = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}