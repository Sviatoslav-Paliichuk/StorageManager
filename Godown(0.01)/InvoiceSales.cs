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
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;

namespace Godown_0._01_
{
    public partial class InvoiceSales : Form
    {      
        DataTable zamoblennya;
        static string mon;
        static string day;
        static string s;
        string NumericMaximum;
        string PriceTovar;
        static double Summa;
        SqlConnection con;
        SqlCommand com;
        SqlDataReader reader;
        string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ToString();
        public InvoiceSales()
        {
            InitializeComponent();
            monthCalendar1.DateSelected += monthCalendar1_DateSelected;
        }

        void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
           
            //throw new NotImplementedException();
            s=GetMounthString();
            toolStripLabel1.Text = s;

        //    MessageBox.Show(monthCalendar1.SelectionRange.Start.Year + "/" + mon + "/" + day);
        }
        private string GetMounthString()
        {
            #region modernizated mounth calendar for Time.Mode 'yyyy/mm/dd'
            if ((int)monthCalendar1.SelectionRange.Start.Month < 10 && (int)monthCalendar1.SelectionRange.Start.Day < 10)
            {
                mon = "0" + monthCalendar1.SelectionRange.Start.Month;
                day = "0" + monthCalendar1.SelectionRange.Start.Day;

                return monthCalendar1.SelectionRange.Start.Year + "/" + mon + "/" + day;
            }
            if ((int)monthCalendar1.SelectionRange.Start.Month < 10 && (int)monthCalendar1.SelectionRange.Start.Day >= 10)
            {
                mon = "0" + monthCalendar1.SelectionRange.Start.Month;
              //  day = "0" + monthCalendar1.SelectionRange.Start.Day;

                return monthCalendar1.SelectionRange.Start.Year + "/" + mon + "/" + monthCalendar1.SelectionRange.Start.Day;
            }
            if ((int)monthCalendar1.SelectionRange.Start.Month == 10 && (int)monthCalendar1.SelectionRange.Start.Day < 10)
            {
                //mon = "0" + monthCalendar1.SelectionRange.Start.Month;
                  day = "0" + monthCalendar1.SelectionRange.Start.Day;

                  return monthCalendar1.SelectionRange.Start.Year + "/" + monthCalendar1.SelectionRange.Start.Month + "/" + day;
            }
            if ((int)monthCalendar1.SelectionRange.Start.Month >= 10 && (int)monthCalendar1.SelectionRange.Start.Day < 10)
            {
                //mon = "0" + monthCalendar1.SelectionRange.Start.Month;
                day = "0" + monthCalendar1.SelectionRange.Start.Day;

                return monthCalendar1.SelectionRange.Start.Year + "/" + monthCalendar1.SelectionRange.Start.Month + "/" + day;
            }
            return monthCalendar1.SelectionRange.Start.Year + "/" + monthCalendar1.SelectionRange.Start.Month + "/" + monthCalendar1.SelectionRange.Start.Day;
            #endregion modernizated mounth calendar for Time.Mode 'yyyy/mm/dd'
        }
        private void InvoiceSales_Load(object sender, EventArgs e)
        {
            //toolStripLabel1.Text = monthCalendar1.SelectionRange.Start.Year + "/" + monthCalendar1.SelectionRange.Start.Month + "/" + monthCalendar1.SelectionRange.Start.Day;
            s = GetMounthString();
            toolStripLabel1.Text = s;
            GetAllProviders();
            GetAllPurchase();
            GetTovar();
            CreateAndWorkWithPrintTable();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SellTovar(com);
            comboBox3_SelectedIndexChanged(sender, e);
            FillInvoice();
            toolStripButton4_Click(sender, e);
        }

        private void SellTovar(SqlCommand com)
        {
            #region update Tovar in DB
            string[] split = textBox1.Text.Split(new Char[] { ' ' });
            string err = split[0];
            string correctString = err.Replace(",", ".");
            string sellTovar = "update [Постачальник-Товар] set [Постачальник-Товар].[Кількість, шт] =" + Convert.ToString(Convert.ToInt32(textBox3.Text) - Convert.ToInt32(numericUpDown1.Value)) + " where [Ціна, грн] =" + correctString;
         //  MessageBox.Show(sellTovar);
            try
            {
                con = new SqlConnection(connection_string);
                com = new SqlCommand(sellTovar);
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();

                MessageBox.Show("Продано:-> " + comboBox3.SelectedItem.ToString() + " " + numericUpDown1.Value.ToString() + " шт" + ", Загальною вартістю: " + textBox2.Text, "Звіт", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            #endregion update Tovar in DB
        }

        private void FillInvoice()
        {
            #region fill Invoice for print
            string fill = "insert into [Замовлення] ([№ Покупця],[№ Товару],[Дата замовлення]) values((select [Контрагенти].[№] from [Контрагенти] where [Контрагенти].Назва='" +
                comboBox2.SelectedItem.ToString() + "\')," +
                "(select [Товар].[№ Товару] from [Товар] where [Товар].Назва='" +
                comboBox3.SelectedItem.ToString() + "\')," + "\'" + toolStripLabel1.Text + "\')";

            // MessageBox.Show(fill);
            try
            {
                con = new SqlConnection(connection_string);
                com = new SqlCommand(fill);
                con.Open();
                com.Connection = con;
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            #endregion fill Invoice for print
        }
        private void GetAllProviders()
        {
            string recproviders = @"SELECT *from [Контрагенти] where [Постачальник] = 1";
            GetItemsFromBase(recproviders, comboBox1, reader, com,1);
        }

        private void GetAllPurchase()
        {
            string recproviders = @"SELECT *from [Контрагенти] where [Постачальник] = 0";
            GetItemsFromBase(recproviders, comboBox2, reader, com,1);
        }

        private void GetTovar()
        {
            string recTovar = @"SELECT [Товар].Назва from  [Постачальник-Товар] inner join [Товар] on [Товар].[№ Товару] = [Постачальник-Товар].[№ Товару]where [Постачальник-Товар].[Кількість, шт] > 0";
            GetItemsFromBase(recTovar, comboBox3, reader, com,0);
        }

        private void GetItemsFromBase(string rec, ComboBox combobox, SqlDataReader reader, SqlCommand com, int numb_string)
        {
            #region universal method for get items to combobox
            con = new SqlConnection(connection_string);
           com = new SqlCommand(rec, con);
            con.Open();
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    string result = reader.GetString(numb_string);
                    if(result!="")
                    { 
                        combobox.Items.Add(result);
                        combobox.Text = result;
                    }
                }
                catch { }

            }
            con.Close();
            #endregion universal method for get items to combobox
        }
        private void GetItemsFromBaseTextBox(SqlDataReader reader, SqlCommand com)
        {
            #region universal method for get items to textbox
            PriceTovar = @"SELECT [Постачальник-Товар].[Ціна, грн] from  [Постачальник-Товар] inner join [Товар] on [Товар].[№ Товару] = [Постачальник-Товар].[№ Товару]where [Товар].Назва=" + "\'" + comboBox3.SelectedItem + "\'";
            con = new SqlConnection(connection_string);
            com = new SqlCommand(PriceTovar, con);
            con.Open();
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    double result = reader.GetDouble(0);
                    //comboBox4.Items.Add(result);
                    textBox1.Text = Convert.ToString(result + " грн");
                    textBox2.Text = Convert.ToString(result * Convert.ToInt32(numericUpDown1.Value) + " грн");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

            }
            con.Close();
            #endregion universal method for get items to textbox
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region for select sell type
            // MessageBox.Show(comboBox3.SelectedItem.ToString());
            NumericMaximum = @"SELECT [Постачальник-Товар].[Кількість, шт] from  [Постачальник-Товар] inner join [Товар] on [Товар].[№ Товару] = [Постачальник-Товар].[№ Товару]where [Товар].Назва='" + comboBox3.Text + "\'";           
           
            con = new SqlConnection(connection_string);
            com = new SqlCommand(NumericMaximum, con);
            con.Open();
            GetItemsFromBaseTextBox(reader, com);
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    string result = reader.GetString(0);
                    if (result != "")
                    {
                        numericUpDown1.Maximum = Convert.ToInt32(result);
                        textBox3.Text = Convert.ToString(result);
                    }
                }
                catch { }
            }
            con.Close();
            #endregion for select sell type
        }

        private void numericUpDown1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == numericUpDown1.Maximum)
            {
                MessageBox.Show("Ви вибрали максимальну кількість товару, що знаходиться на складі -> " + comboBox3.SelectedItem, "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);    
            }
            GetItemsFromBaseTextBox(reader, com);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //WebBrowser web = new WebBrowser();
            //Uri uri = new Uri("http://uk.wikipedia.org/wiki/Самогон");
            //web.Url = uri;
            //web.Navigate(uri, true);

            //Open the print dialog
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;
            printDialog.UseEXDialog = true;
            //Get the document
            if (DialogResult.OK == printDialog.ShowDialog())
            {
                printDocument1.DocumentName = "Test Page Print";
                printDocument1.Print();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }
       
        private void CreateAndWorkWithPrintTable()
        {
            zamoblennya = new DataTable();

            DataColumn TovarName = new DataColumn("Назва");
            DataColumn TovarCount = new DataColumn("Кількість");
            DataColumn TovarPrice = new DataColumn("Ціна", typeof(Double));
            DataColumn TovarSumm = new DataColumn("Сума", typeof(Double));

            zamoblennya.Columns.Add(TovarName);
            zamoblennya.Columns.Add(TovarCount);
            zamoblennya.Columns.Add(TovarPrice);
            zamoblennya.Columns.Add(TovarSumm);

            dataGridView1.DataSource = zamoblennya;
        
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            string[] split = textBox1.Text.Split(new Char[] { ' ' });
            string err = split[0];        
            zamoblennya.Rows.Add(comboBox3.SelectedItem.ToString(), numericUpDown1.Value.ToString(), Convert.ToDouble(err), Convert.ToDouble(err) * Convert.ToDouble(numericUpDown1.Value));
            Summa = 0;
            for(int i=0; i<dataGridView1.RowCount;i++)
            {
                Summa += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
            }         
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Bitmap bmp = new Bitmap(dataGridView1.Size.Width + 800, dataGridView1.Size.Height + 800);
            //dataGridView1.DrawToBitmap(bmp, dataGridView1.Bounds);
            //e.Graphics.DrawImage(bmp, 0, 0);

            Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            Font fon = new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold);
            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));

            e.Graphics.DrawImage(bm, 120, 200);
            Font s = new System.Drawing.Font("Times New Roman", 14);
           // Font ss = new System.Drawing.Font("Times New Roman", 14,FontStyle.Underline);

            e.Graphics.DrawString("Видаткова накладна    № ________", s, Brushes.Black, 120, 30);

            e.Graphics.DrawString("Постачальник               "+comboBox1.Text, s, Brushes.Black, 120, 60);
      
            e.Graphics.DrawString("Одержувач                    " + comboBox2.Text, s, Brushes.Black, 120, 90);

            e.Graphics.DrawString("Умова продажу:           Безготівковий рахунок", s, Brushes.Black, 120, 120);

            e.Graphics.DrawString("Всього на суму          " + Summa + " грн", s, Brushes.Black, 120, 600);

            e.Graphics.DrawString("Від постачальника ______________________      " +"Отримав(ла) ________________", s, Brushes.Black, 120, 650);

            e.Graphics.DrawString(DateTime.Now.ToShortDateString(), s, Brushes.Black, 120, 700);
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }
    }  
}
