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
    public partial class AddContractors : Form
    {
        Regex only_Symbols = new Regex(@"\d");
        Regex only_Numbers = new Regex("[0-9]");
        bool isSeller;
        SqlConnection con;
        string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ToString();
        public AddContractors()
        {
            InitializeComponent();
         
        }

        private void AddContractors_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Покупець");
            comboBox1.Items.Add("Постачальник");
            GetAllMfo();
            GetMfoFromName();
            timer1.Start();
           // comboBox2.SelectedIndex = 0;
            //comboBox3.SelectedIndex = 0;
        }

        private void GetAllMfo()
        {
            con = new SqlConnection(connection_string);
            SqlCommand com = new SqlCommand(@"SELECT [Назва] from [Банк]", con);
            con.Open();
            SqlDataReader reader;
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    string result = reader.GetString(0);                  
                    comboBox2.Items.Add(result);
                    comboBox2.Text = result;
                }
                catch { }

            }
            con.Close();
        }

        private void GetMfoFromName()
        {
            //MessageBox.Show(@"SELECT [МФО] from [Банк] where [Назва] = [" + comboBox2.Text + "]");
            con = new SqlConnection(connection_string);
            SqlCommand com = new SqlCommand(@"SELECT [МФО] from [Банк] where [Назва] ='" + comboBox2.SelectedItem.ToString() + "'", con);
            con.Open();
            SqlDataReader reader;
            reader = com.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    string result = reader.GetString(0);
                    comboBox3.Items.Add(result);
                    comboBox3.Text = result;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

            }
         
            con.Close();
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem == "Постачальник")
            {
                isSeller = true;
               
            }
            else if(comboBox1.SelectedItem == "Покупець")
            {
                isSeller = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            timer1.Stop();

          //     MessageBox.Show(comboBox2.SelectedIndex.ToString());
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con = null;
            int ComIndex = comboBox2.SelectedIndex+1;

            if (comboBox1.SelectedItem == "Постачальник")
            {
                isSeller = true;

            }
            else if (comboBox1.SelectedItem == "Покупець")
            {
                isSeller = false;
            }

            #region test
            //MessageBox.Show(comboBox3.SelectedItem.ToString());
            //MessageBox.Show( "insert into [Контрагенти]" + " ([Назва], [ЄДРПОУ], [Розрахунковий рахунок], [МФО Банку], [Постачальник], [Адреса],[Телефон])" + "values(" +
            //        "\'" + textBox1.Text + "\'" + "," +
            //        "\'" + textBox2.Text + "\'" + "," +
            //        "\'" + textBox3.Text + "\'" + "," +
            //         "\'" + comboBox3.SelectedItem.ToString() + "\'" + "," +

            //        (bool)isSeller  + "," +
            //          "\'" + textBox6.Text + "\'" + "," +
            //           "\'" + textBox7.Text + "\'" + ")");

            #endregion test

            con = new SqlConnection(connection_string);
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                  "insert into [Контрагенти]" + " ([Назва], [ЄДРПОУ], [Розрахунковий рахунок],[№ Банку], [МФО Банку], [Постачальник], [Адреса],[Телефон])" + "values(" +
                    "\'" + textBox1.Text + "\'" + "," +
                    "\'" + textBox2.Text + "\'" + "," +
                    "\'" + textBox3.Text + "\'" + "," +
                    //bank numb error & bug
                    ComIndex + "," +
                     "\'" + comboBox3.SelectedItem.ToString() + "\'" + "," +

                     "\'" + (bool)isSeller + "\'" + "," +
                      "\'" + textBox6.Text + "\'" + "," +
                       "\'" + textBox7.Text + "\'" + ")", con);


                cmd.ExecuteNonQuery();

                MessageBox.Show("Контрагент " + textBox1.Text + " успішно доданий в базу");

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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
              GetMfoFromName();           
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  GetNameFromMFO();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripLabel1.Text = DateTime.Now.ToString("Поточна година " + "HH:mm:ss");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < comboBox2.Items.Count; i++)
            //{
            //    comboBox2.Items.RemoveAt(i);
            //}
            AddNewBank adb = new AddNewBank();
            comboBox2.Items.Clear();
            adb.ShowDialog();
            
            GetAllMfo();
            //GetMfoFromName();
        }

        private void AddContractors_Paint(object sender, PaintEventArgs e)
        {          

            if ((only_Numbers.IsMatch(textBox2.Text)))
            {
                textBox2.ForeColor = Color.Black;
            }

            else
            {
                textBox2.ForeColor = Color.Red;
            }

            if ((only_Numbers.IsMatch(textBox3.Text)))
            {
                textBox3.ForeColor = Color.Black;
            }

            else
            {
                textBox3.ForeColor = Color.Red;
            }
          
        }
    }
}
