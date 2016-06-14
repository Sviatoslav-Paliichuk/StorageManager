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
    public partial class AddNewBank : Form
    {
        Regex only_Symbols = new Regex(@"\d");
        Regex only_Numbers = new Regex("[0-9]");
        SqlConnection con;
        string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ToString();
        string name;
        string mfo;
        string adress;
        public AddNewBank()
        {
            InitializeComponent();          
        }

        private void button1_Click(object sender, EventArgs e)
        {                     
            con = null;
            name = textBox1.Text;
            mfo = textBox2.Text;
            adress = textBox3.Text;

            con = new SqlConnection(connection_string);
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                  "insert into [Банк]" + " ([Назва], [МФО], [Адреса])" + "values(" +
                    "\'" + name + "\'" + "," +
                    "\'" + mfo + "\'" + "," +
                    "\'" + adress + "\'" + ")",con);
                #region test message add bank
                //MessageBox.Show("insert into [Банк]" + " ([Назва], [МФО], [Адреса])" + "values(" +
                //    "\'" + name + "\'" + "," +
                //    "\'" + mfo + "\'" + "," +
                //    "\'" + adress + "\'" + ")");
                #endregion test message add bank

                cmd.ExecuteNonQuery();

                MessageBox.Show("Банк "+name+" успішно додано в базу");
               
                
                this.Close();

             
              
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                con.Close();                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddNewBank_Load(object sender, EventArgs e)
        {
           
        }

        private void AddNewBank_Paint(object sender, PaintEventArgs e)
        {
           if ((only_Symbols.IsMatch(textBox1.Text)) || String.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.ForeColor = Color.Red;
               
            }

            else
            {
                textBox1.ForeColor = Color.Black;
              
            }
           if ((only_Symbols.IsMatch(textBox3.Text)) || String.IsNullOrEmpty(textBox3.Text))
           {
               textBox3.ForeColor = Color.Red;
             
           }

           else
           {
               textBox3.ForeColor = Color.Black;
              
           }
           if ((only_Symbols.IsMatch(textBox2.Text)))
           {
              
               textBox2.ForeColor = Color.Black;
           }

           else
           {
               textBox2.ForeColor = Color.Red;
              
           }
        }      

    }
}
