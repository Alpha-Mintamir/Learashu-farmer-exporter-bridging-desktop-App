using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEARASHU
{
    public partial class Agent_Dashboard : Form
    {
        public Agent_Dashboard()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void EbtnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Agent_Login agent_Login = new Agent_Login();
            agent_Login.Show();
        }

        private void topPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Agent_Dashboard_Load(object sender, EventArgs e)
        {

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

        bool sidebarExpand = false;

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

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void EbtnMenu_Click(object sender, EventArgs e)
        {
            menuTransition.Start();
        }

        private void EbtnSuperHome_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
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

        private void EbtnRegister_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            registereddProducts();
        }

        private void ExporterSpecificationss()
        {
            string connectionString = @"Data Source=DESKTOP-EQ55Q8H\SQLEXPRESS; Initial Catalog=LEARASHU; Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM productRegistration", sqlConnection);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            dataGridView2.DataSource = table;
        }

        private void EbtnExplore_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            ExporterSpecificationss();
        }

        private void EbtnProfile_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }
    }
}
