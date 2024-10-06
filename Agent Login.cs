using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace LEARASHU
{
    public partial class Agent_Login : Form
    {
        public Agent_Login()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

        }

        private void showPasswordCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            EtxtBoxPassword.PasswordChar = showPasswordCheckBox1.Checked ? '\0' : '*';
        }

        private void EbtnLogin_Click(object sender, EventArgs e)
        {
            string username = EtxtBoxPhoneOrEmail.Text;
            string password = EtxtBoxPassword.Text;

            try
            {
                string query = "SELECT * FROM Agent WHERE phoneNumber = @username AND password = @password";
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-EQ55Q8H\SQLEXPRESS;Initial Catalog=LEARASHU;Integrated Security=True"))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        Agent_Dashboard agent_Dashboard = new Agent_Dashboard();
                        agent_Dashboard.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Login Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EtxtBoxPassword.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainPage mainPage = new mainPage();
            this.Close();
            mainPage.Show();
        }

        private void topPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
