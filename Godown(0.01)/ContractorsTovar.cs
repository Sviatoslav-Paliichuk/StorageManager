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
namespace Godown_0._01_
{
    public partial class ContractorsTovar : Form
    {
        int myId;
        int tovarId;
        string StartDate;
        string EndDate;
        SqlConnection con;
        SqlConnection con2;
        string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ToString();
        public ContractorsTovar()
        {
            InitializeComponent();
             
        }

        private void ContractorsTovar_Load(object sender, EventArgs e)
        {
            LoadContractors();
            LoadTovarCategories();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy/MM/dd";
            timer1.Start();
        }

        private void LoadContractors()
        {
            con = new SqlConnection(connection_string);
            SqlCommand com = new SqlCommand(@"SELECT *from [Контрагенти] where [Постачальник] = 1", con);
            con.Open();
            SqlDataReader reader;
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    string result = reader.GetString(1);
                    comboBox1.Items.Add(result);
                    comboBox1.Text = result;
                }
                catch { }

            }
            con.Close();
        }

        private void LoadTovarCategories()
        {
            #region Load [Tovar].[Name] from [Categories]
            con = new SqlConnection(connection_string);
            SqlCommand com = new SqlCommand(@"SELECT [Назва] from [Категорії товару]", con);
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
              
                  reader.Close();
                  con.Close();
            
        }
            #endregion Load [Tovar].[Name] from [Categories]


        private void button1_Click(object sender, EventArgs e) // this is magic, don`t try edit it
        {          
            int a = comboBox2.SelectedIndex;
             a = a + 1;
           //  StartDate = dateTimePicker1.Value.ToShortDateString();
          //   EndDate = dateTimePicker2.Value.ToShortDateString();
              //MessageBox.Show("insert into [Товар]" + " ([Назва],[№ Категорії])" + "values(" + "\'" + textBox1.Text + "\'" + "," + a + ")");
             try
             {
                 con = new SqlConnection(connection_string);

                 SqlCommand com = new SqlCommand(@"SELECT [№] from [Контрагенти] where [Назва] ='" + comboBox1.SelectedItem + "'", con);

                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();

                 {
                     while (reader.Read())
                     {
                         myId = (int)reader[0];
                       //  MessageBox.Show(myId.ToString());
                     }

                 }

                 reader.Close();
   
             }

             catch(Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
             finally
             {
                 con.Close();
             }


             #region WTF

             try
             {
                 con = new SqlConnection(connection_string);
                 con.Open();
                 SqlCommand cmd = new SqlCommand("insert into [Товар]" + " ([Назва],[№ Категорії])" + "values(" + "\'" + textBox1.Text + "\'" + "," + a + ")", con);
                 cmd.ExecuteNonQuery();
                 con.Close();
             }

            catch(Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }


             try
             {
                 con = new SqlConnection(connection_string);

                 SqlCommand com = new SqlCommand(@"SELECT [№ Товару] from [Товар] where [Назва] ='" + textBox1.Text + "'", con);

                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();

                 {
                     while (reader.Read())
                     {
                         tovarId = (int)reader[0];
                        // MessageBox.Show(tovarId.ToString());
                     }

                 }

                 reader.Close();

             }

             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
             finally
             {
                 con.Close();
             }

             #endregion WTF


             try
            {
                con = new SqlConnection(connection_string);
                con.Open();


              
                SqlCommand cmd2 = new SqlCommand("insert into [Постачальник-Товар]" +
                    " ([№ Постачальника],[№ Товару],[Кількість, шт],[Ціна, грн], [Дата постачання],[Термін придатності])" +

                    "values(" + myId +","+ tovarId +"," + numericUpDown1.Value + "," + textBox2.Text + "," +
                    "\'" + dateTimePicker1.Text + "\'" + "," + "\'" + dateTimePicker2.Text + "\'" + ")", con);


              
                cmd2.ExecuteNonQuery();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // GetIdWithName();
        }

        private void GetIdWithName()
        {
            //con = new SqlConnection(connection_string);
            
            //SqlCommand com = new SqlCommand(@"SELECT [№] from [Контрагенти] where [Назва] ='" + comboBox1.SelectedItem + "'", con);
            //con.Open();
            //SqlDataReader reader = com.ExecuteReader();
            //{
            //    while (reader.Read())
            //    {
            //        MessageBox.Show(reader[0].ToString());
            //    }
            //}
            //con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
         //   MessageBox.Show(StartDate = dateTimePicker1.Value.ToLongDateString());
            timer1.Stop();
            this.Close();
        }

        private void toolStripLabel1_Paint(object sender, PaintEventArgs e)
        {
         

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
         
            toolStripLabel1.Text = DateTime.Now.ToString("Поточна година :"+"HH:mm:ss");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Categories cat = new Categories();
            comboBox2.Items.Clear();
            cat.ShowDialog();
            LoadTovarCategories();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {            
            AddContractors adc = new AddContractors();
            comboBox1.Items.Clear();
            adc.ShowDialog();
            LoadContractors();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
