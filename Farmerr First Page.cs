using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LEARASHU
{
    public partial class Farmerr_First_Page : Form
    {
        public Farmerr_First_Page()
        {
            InitializeComponent();
            InitializeTableLayoutPanel();
            DisplayDataOnTableLayoutPanel();
            InitializeComboBoxes();
            this.Load += Farmerr_First_Page_Load;
            EpostTextBox.Enter += EpostTextBox_Enter;
            EpostTextBox.Leave += EpostTextBox_Leave;
            ActiveControl = EfontStylecomboBox;


            this.WindowState = FormWindowState.Maximized;
        }

        private void EbtnSuperHome_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
        }

        private void EbtnMenu_Click(object sender, EventArgs e)
        {
            menuTransition.Start();
        }


        bool menuExpand = false;



        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpand == false)
            {
                menuContainer.Height -= 20; 

                if (menuContainer.Height <= 69)
                {
                    menuContainer.Height = 69; 
                    menuTransition.Stop();
                    menuExpand = true;
                }
            }
            else
            {
                menuContainer.Height += 20; 

                if (menuContainer.Height >= 280)
                {
                    menuContainer.Height = 280; 
                    menuTransition.Stop();
                    menuExpand = false;
                }
            }
        }



        bool sidebarExpand = true;


        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                sidebar.Width -= 15;

                if (sidebar.Width <= 95)
                {
                    sidebar.Width = 95;
                    sidebarExpand = false;
                    sidebarTransition.Stop();
                }
            }
            else
            {
                sidebar.Width += 15;

                if (sidebar.Width >= 309)
                {
                    sidebar.Width = 309;
                    sidebarExpand = true;
                    sidebarTransition.Stop();
                }
            }
        }



        private void LoadProfileAndCoverImages()
        {

            string connectionString = @"Data Source=DESKTOP-EQ55Q8H\SQLEXPRESS; Initial Catalog=LEARASHU; Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT TOP 6 CoverPicture, ProfilePicture FROM farmerAdditionalData";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] coverPictureData = (byte[])reader["CoverPicture"];
                                byte[] profilePictureData = (byte[])reader["ProfilePicture"];

                                Image coverPicture = ByteArrayToImage(coverPictureData);
                                Image profilePicture = ByteArrayToImage(profilePictureData);

                                pictureBox1.Image = coverPicture;
                                pictureBox3.Image = profilePicture;
                                pictureBox4.Image = profilePicture;
                                pictureBox2.Image = coverPicture;


                                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading images: " + ex.Message);
                }
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }




        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }






        private void productRegistered()
        {

            Color errorColor = Color.FromArgb(240, 128, 128);


            string patternDecimal = @"^\d+(\.\d+)?$";
            Regex objDecimal = new Regex(patternDecimal);

            bool isValid = true;

            // Validate price
            if (objDecimal.IsMatch(EpricePerKgTextBox.Text))
            {
                EpricePerKgTextBox.BackColor = Color.White;
            }
            else
            {
                EpricePerKgTextBox.BackColor = errorColor;
                isValid = false;
            }

            // Validate quantity
            if (objDecimal.IsMatch(EquantityInKgTextBox.Text))
            {
                EquantityInKgTextBox.BackColor = Color.White;
            }
            else
            {
                EquantityInKgTextBox.BackColor = errorColor;
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show("Please enter valid numeric values for quantity and price before proceeding.");
                return;
            }


            string connectionString = @"Data Source=DESKTOP-EQ55Q8H\SQLEXPRESS; Initial Catalog=LEARASHU; Integrated Security=True";

            ComboBox desiredProductTextBox = comboBox1; 
            TextBox quantityTextBox = EquantityInKgTextBox; 
            TextBox priceTextBox = EpricePerKgTextBox; 
            ComboBox quauitycomboboxBox = EqualityComboBox; 
            TextBox exportToTextBox = EexportToTextBox; 
            DateTimePicker timeDateTimePicker = EdateTimePicker1; 



            string query = "INSERT INTO productRegistration" +
               "([The Product I want to Sell is:], [I want this amount of Quantity], [I pay this much per Killogrm], [The Quality Level I offer is:], " +
               "[I am Selling the product from:], [I need the product upto:])" +
               "VALUES (@Product, @Quantity, @Price, @QualityLevel, @ExportTo, @Time)";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Product", (comboBox1.SelectedItem != null) ? comboBox1.SelectedItem.ToString() : "DefaultProduct");
                    command.Parameters.AddWithValue("@Quantity", int.Parse(EquantityInKgTextBox.Text)); 
                    command.Parameters.AddWithValue("@Price", decimal.Parse(EpricePerKgTextBox.Text)); 
                    command.Parameters.AddWithValue("@QualityLevel", (EqualityComboBox.SelectedItem != null) ? comboBox1.SelectedItem.ToString() : "DefaultProduct");
                    command.Parameters.AddWithValue("@ExportTo", EexportToTextBox.Text);
                    command.Parameters.AddWithValue("@Time", EdateTimePicker1.Value);


                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Farmer Product inserted successfully.");
                        tabPage4.Show();
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert Farmer Product.");
                    }



                }
            }
        }








        private void DisplayDataOnTableLayoutPanel()
        {
            string connectionString = @"Data Source=DESKTOP-EQ55Q8H\SQLEXPRESS; Initial Catalog=LEARASHU; Integrated Security=True";

            string query = "SELECT * FROM farmerCreateAccountt";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        TabPage tabPage = tabControl1.TabPages[1];

                        TableLayoutPanel tableLayoutPanel = tabPage.Controls["tableLayoutPanel1"] as TableLayoutPanel;

                        Label labelFirstName = tableLayoutPanel.Controls["firstName"] as Label;
                        Label labelLastName = tableLayoutPanel.Controls["lastName"] as Label;
                        Label labelDateOfBirth = tableLayoutPanel.Controls["dateOfBirth"] as Label;
                        Label labelProduct = tableLayoutPanel.Controls["product"] as Label;
                        Label labelRegion = tableLayoutPanel.Controls["region"] as Label;
                        Label labelCity = tableLayoutPanel.Controls["AreaLabel"] as Label;
                        Label labelGender = tableLayoutPanel.Controls["gender"] as Label;
                        Label labelEmail = tableLayoutPanel.Controls["emailLabel"] as Label; 
                        Label labelPhoneNumber = tableLayoutPanel.Controls["PhoneNumberLabel"] as Label; 

                        
                        firstName.Text = reader["firstName"].ToString();
                        lastName.Text = reader["lastName"].ToString();
                        dateOfBirthLabel.Text = reader["dateOfBirth"].ToString();
                        product.Text = reader["product"].ToString();
                        region.Text = reader["region"].ToString();
                        AreaLabel.Text = reader["city"].ToString();
                        gender.Text = reader["gender"].ToString();
                        emailLabel.Text = reader["emailAddress"].ToString();
                        phoneNumberLabel.Text = reader["phoneNumber"].ToString();
                    }

                    reader.Close();
                    connection.Close();
                }
            }
        }




        private void registereddProducts()
        {
            string connectionString = @"Data Source=DESKTOP-EQ55Q8H\SQLEXPRESS; Initial Catalog=LEARASHU; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Exporter_Specifications", sqlConnection);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            dataGridView1.DataSource = table;
        }





        private void InitializeTableLayoutPanel()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.RowCount = 2; 
            tableLayoutPanel1.ColumnCount = 2; 

            Controls.Add(tableLayoutPanel1);

        }






        private void InitializeComboBoxes()
        {
            EfontStylecomboBox.Items.AddRange(Enum.GetNames(typeof(FontStyle)));
            EfontStylecomboBox.SelectedIndex = 0;


            EfontSizecomboBox.Items.AddRange(new object[] { 10, 12, 14, 16 });
            EfontSizecomboBox.SelectedIndex = 1;
        }


        private void ApplyFontStyleAndSize()
        {
            try
            {
                FontStyle selectedStyle = (FontStyle)Enum.Parse(typeof(FontStyle), EfontStylecomboBox.SelectedItem.ToString());

                float selectedSize = Convert.ToSingle(EfontSizecomboBox.SelectedItem);

                if (selectedSize <= 0)
                {
                    selectedSize = 10;
                }

                EpostTextBox.Font = new Font(EpostTextBox.Font.FontFamily, selectedSize, selectedStyle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying font: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string websiteUrl = "https://duresaguye.github.io/learshu-About-Us/";

            System.Diagnostics.Process.Start(websiteUrl);
        }

        private void EbtnAbout_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void EbtnHome_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            LoadProfileAndCoverImages();
        }

        private void EbtnProfile_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            DisplayDataOnTableLayoutPanel();
        }

        private void EbtnExplore_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            registereddProducts();
        }

        private void EbtnRegister_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void EbtnSetting_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void EfontStylecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Farmerr_First_Page_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void EpostTextBox_Enter(object sender, EventArgs e)
        {
            if (EpostTextBox.Text == "Share your thoughts here")
            {
                EpostTextBox.Text = "";
                EpostTextBox.ForeColor = SystemColors.WindowText;
            }
        }

        private void EpostTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EpostTextBox.Text) || EpostTextBox.Text == "|")
            {
                EpostTextBox.Text = "Share your thoughts here";
                EpostTextBox.ForeColor = SystemColors.GrayText;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            productRegistered();
        }

        private void EbtnLogout_Click(object sender, EventArgs e)
        {
            Farmer_Login_Form farmer_Login_Form = new Farmer_Login_Form();
            farmer_Login_Form.Show();
            this.Close();
        }

        private void topPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Request in Pending. Yo will be informed when the Agent Approves");
        }
    }
}
