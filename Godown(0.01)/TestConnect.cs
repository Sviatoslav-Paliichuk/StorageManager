using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;
namespace Godown_0._01_
{
    public partial class TestConnect : Form
    {
        SqlConnection connection;
        string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ToString();
        public TestConnect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void TestConnect_Load(object sender, EventArgs e)
        {
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = 100;            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationManager.RefreshSection("connectionStrings");
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["GodownConnect"].ConnectionString);
                builder.DataSource = this.textBox1.Text;
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                using (connection)
                {
                    connection.Open();
                    connection.Close();
                    toolStripProgressBar1.Value = 100; 
                    MessageBox.Show("З’єднання успішно встановлене.");
                }
            }
            catch (SqlException exception)
            {
               // MessageBox.Show(exception.Message);
                MessageBox.Show("The requested URL [URL] was not found on this server");
            }
            finally
            {
                this.Close();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
