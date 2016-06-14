using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
namespace Godown_0._01_
{
    public partial class Categories : Form
    {
        Regex only_Symbols = new Regex(@"\d");  
        SqlConnection con;
        string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ToString();
        string name;
        public Categories()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con = null;
            name = textBox1.Text;
           

            con = new SqlConnection(connection_string);
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                  "insert into [Категорії товару]" + " ([Назва])" + "values(" +  "\'" + name + "\'"+ ")", con);
                            

                cmd.ExecuteNonQuery();

                MessageBox.Show("Категорію " + name + " успішно додано в базу");

                this.Close();



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                con.Close();
            }
        }

        private void Categories_Load(object sender, EventArgs e)
        {

        }

        private void Categories_Paint(object sender, PaintEventArgs e)
        {
            if ((only_Symbols.IsMatch(textBox1.Text)) || String.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.ForeColor = Color.Red;
                button1.Enabled = false;
            }

            else
            {
                textBox1.ForeColor = Color.Black;
                button1.Enabled = true;
            }
        }
    }
}
