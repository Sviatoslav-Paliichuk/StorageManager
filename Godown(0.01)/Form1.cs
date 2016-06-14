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
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;

namespace Godown_0._01_
{
    public partial class Form1 : Form
    {
      //  SqlConnection global = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ToString());
        SqlCommand com;
        SqlConnection con;
        DataTable dataTable;
        SqlDataAdapter adp;
        //string connection_string = @"Server=ADMIN-LAPTOP\SQL2014; Database=Godown; User ID=sa; Password=1";
        string connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ToString();
        public Form1()
        {         
            InitializeComponent();          
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy/MM/dd";
            toolStripStatusLabel1.Text = "Для того, щоб розпочати роботу, підключіться до сервера";
            timer1.Start();
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;
            toolStripButton4.Enabled = false;
            toolStripButton5.Enabled = false;
            toolStripButton6.Enabled = false;
            toolStripButton7.Enabled = false;
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = 100;
        }
        private void LoadPackage()
        {
            PopulateTreeViewFromBase();
            SettingsDataGridView();           
            textBox1.Text += "Запуск ПЗ облік товарів - пройшов успішно\r\n";
           
        }
        private void PopulateTreeViewFromBase()
        {
            try
            {
                con = new SqlConnection(connection_string);
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", con))
                {
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                       // comboBox1.Items.Clear();
                        while (reader.Read())
                        {
                            if ((string)reader["TABLE_NAME"] != "sysdiagrams")
                               // comboBox1.Items.Add((string)reader["TABLE_NAME"]);
                            treeView1.Nodes.Add((string)reader["TABLE_NAME"]);
                            treeView1.Sort();                            
                        }
                    }
                }            
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

        private void ViewSelectedTreeViewItem()
        {
            
            
            //bindingSource1.DataSource = dataTable;
            //dataGridView1.DataSource = bindingSource1;
        }

        private void treeView1_Click(object sender, EventArgs e)
        {                      
          //  treeView1_DoubleClick(sender, e);
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            //MessageBox.Show(treeView1.SelectedNode.ToString());

            //string item = treeView1.SelectedNode.ToString();
            //string[] split = item.Split(new Char[] { 'e' });
            //MessageBox.Show(split[1]);
            // MessageBox.Show(treeView1.SelectedNode.ToString());
        
            string ss = treeView1.SelectedNode.ToString();           
            ss = ss.Remove(0, 10);
            string rectovar = "SELECT [Товар].[Назва], [Категорії товару].[Назва] as[Категорія] FROM  [Категорії товару] inner join [Товар]on [Категорії товару].[№] = [Товар].[№ Категорії] where[Товар].[Назва]!= ''";
            string reccontragent = "SELECT [Контрагенти].Назва, [Контрагенти].ЄДРПОУ, [Контрагенти].[Розрахунковий рахунок], [Банк].[Назва] as[Назва Банку],[Банк].МФО, Постачальник,[Контрагенти].Адреса, [Контрагенти].Телефон  from [Банк] inner join [Контрагенти] on [Банк].[№] = [Контрагенти].[№ Банку]";
            string recbank ="SELECT Назва, МФО, Адреса from Банк";
            string reccategories = "SELECT Назва from [Категорії товару]";
            string reccontractorsTovar = "SELECT [Контрагенти].Назва, [Товар].Назва as [Назва товару], [Постачальник-Товар].[Кількість, шт], [Постачальник-Товар].[Ціна, грн],[Постачальник-Товар].[Дата постачання], [Постачальник-Товар].[Термін придатності] from [Контрагенти] inner join [Постачальник-Товар] on  [Постачальник-Товар].[№ Постачальника]=[Контрагенти].[№]  inner join [Товар] on [Постачальник-Товар].[№ Товару] = [Товар].[№ Товару]";
            string recorder = "SELECT * from [Замовлення]";
            string selltype = "select  [Контрагенти].Назва, [Замовлення].[Дата замовлення], [Товар].Назва as [Найбільша поставка]from [Замовлення] left join [Контрагенти]on [Замовлення].[№ Замовлення] = [Контрагенти].[№] inner join [Товар] on [Замовлення].[№ Товару] = [Товар].[№ Товару]where [Контрагенти].Назва !=''";

            if (ss == "Банк")
            {
                TovarType(recbank, dataTable, adp);
                toolStripButton6.Text = "Додати новий Банк";
            }            
            if(ss=="Товар")
            {
                TovarType(rectovar,dataTable,adp);
                toolStripButton6.Text = "Додати товар";
            }
           
            if (ss == "Категорії товару")
            {
                TovarType(reccategories, dataTable, adp);
                toolStripButton6.Text = "Додати нову Категорію товару";
            }
            if (ss == "Контрагенти")
            {
                TovarType(reccontragent, dataTable, adp);
                toolStripButton6.Text = "Додати Покупця/Постачальника";
            }
            if(ss == "Постачальник-Товар")
            {
                TovarType(reccontractorsTovar, dataTable, adp);
                toolStripButton6.Text = "Сформувати прихідну накладну";              
            }
            if (ss == "Замовлення")
            {
                TovarType(selltype, dataTable, adp);
                toolStripButton6.Enabled = false;
                toolStripButton6.Text = "Додати";            
                toolStripButton7.Enabled = true;               
            }
            else
            {
                toolStripButton6.Enabled = true;
                toolStripButton7.Enabled = false;
            }
            
            //else
            //{
            //    string s = treeView1.SelectedNode.ToString();
            //    s = s.Remove(0, 10);

            //    dataGridView1.ClearSelection();




            //    con = new SqlConnection(connection_string);
            //    con.Open();

            //    SqlDataAdapter adp2 = new SqlDataAdapter(@"SELECT * from" + "[" + s + "]", con);
            //    DataTable dataTable2 = new DataTable();
            //    adp2.Fill(dataTable2);
            //    con.Close();
            //    //   dataGridView1.DataSource = dataTable;
            //    bindingSource1.DataSource = dataTable2;
            //    dataGridView1.DataSource = bindingSource1;
            //}
            
            #region
//            if (s == "Товар")
//            {
//                dataGridView1.ClearSelection();

//                con = new SqlConnection(connection_string);
//                con.Open();

//                adp = new SqlDataAdapter(@"SELECT [Товар].Назва, [Категорії товару].Назва as[Категорія]
// FROM [Категорії товару] inner join [Товар]
//on [Категорії товару].[№] = [Товар].[№ Категорії]", con);
//                dataTable = new DataTable();
//                adp.Fill(dataTable);
//                con.Close();
//                bindingSource1.DataSource = dataTable;
//                dataGridView1.DataSource = bindingSource1;
//            }
            #endregion // test  // test inner table
        }

        private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void treeView1_TabIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView1_DoubleClick(sender, e);
        }  
    
        private void SettingsDataGridView()
        {
            bindingNavigator1.BindingSource = bindingSource1;
            dataGridView1.DataSource = bindingSource1;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {         
            string s = treeView1.SelectedNode.ToString();
            s = s.Remove(0, 10);
             
            if(s == "Банк")
            {
                AddNewBank fm = new AddNewBank();
                fm.Owner = this;
                fm.ShowDialog();
               // textBox1.Text += "=============  Добавлено вибраний елемент  ============= \r\n";
                treeView1_DoubleClick(sender, e);
                //
            }
            if(s == "Контрагенти")
            {
                AddContractors adc = new AddContractors();
                adc.ShowDialog();
               // textBox1.Text += "=============  Добавлено вибраний елемент  ============= \r\n";
                treeView1_DoubleClick(sender, e);
            }
            if (s == "Товар")
            {
                MessageBox.Show("Заборонено додавання, необлікованого товару, за винятком продажу товарів особами, які відповідно до законодавства оподатковуються за правилами, що не передбачають ведення обліку обсягів реалізованих товарів (наданих послуг).", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);    
                
                //AddTovar adt = new AddTovar();
                //adt.ShowDialog();                  
            }

            if (s == "Постачальник-Товар")
            {
                ContractorsTovar contr = new ContractorsTovar();
                contr.ShowDialog();
                //textBox1.Text += "=============  Добавлено вибраний елемент  ============= \r\n";
                treeView1_DoubleClick(sender, e);
            }

            if (s == "Категорії товару")
            {
                Categories cat = new Categories();
                cat.ShowDialog();
               // textBox1.Text += "=============  Добавлено вибраний елемент  ============= \r\n";
                treeView1_DoubleClick(sender, e);
            }          
        }

        private void налаштуванняToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TestConnect test = new TestConnect();
            test.ShowDialog();

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            string remainder="SELECT [Товар].Назва, [Кількість, шт], [Ціна, грн]  FROM  [Постачальник-Товар] inner join [Товар]on [Постачальник-Товар].[№ Товару] = [Товар].[№ Товару]";

            if (radioButton3.Checked == true)
            {
                TovarType(remainder, dataTable, adp);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
           #region select All Tovar with start/end period
            string rec = @"SELECT [Товар].Назва, [Кількість, шт], [Ціна, грн]  FROM  [Постачальник-Товар] inner join [Товар]on [Постачальник-Товар].[№ Товару] = [Товар].[№ Товару]where ([Дата постачання]) between" + "('" + dateTimePicker1.Text + "') and ('" + dateTimePicker2.Text + "')";

            if (radioButton1.Checked == true)
            {
                TovarType(rec, dataTable, adp);
            }
            #endregion select All Tovar with start/end period
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            #region select start spoiled Tovar
            string rec = @"SELECT [Товар].Назва, [Кількість, шт], [Ціна, грн]  FROM  [Постачальник-Товар] inner join [Товар]on [Постачальник-Товар].[№ Товару] = [Товар].[№ Товару]where CONVERT (date, SYSDATETIME()) >= (dateadd(MONTH, -1 ,[Термін придатності]))";
            if (radioButton4.Checked == true)
            {
                TovarType(rec,dataTable,adp);
            }
            #endregion select start spoiled Tovar
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            #region select spoiled Tovar
            string rec = @"SELECT [Товар].Назва, [Кількість, шт], [Ціна, грн]  FROM  [Постачальник-Товар] inner join [Товар]on [Постачальник-Товар].[№ Товару] = [Товар].[№ Товару]where CONVERT (date, SYSDATETIME()) > [Термін придатності]";

            if (radioButton2.Checked == true)
            {
                TovarType(rec, dataTable, adp);
            }
            #endregion select spoiled Tovar
        }
        private void TovarType(string request, DataTable dataTable, SqlDataAdapter adp)
        {
            #region for select typeof Tovar
            dataGridView1.ClearSelection();
            try 
            { 
                con = new SqlConnection(connection_string);
                con.Open();
                adp = new SqlDataAdapter(request, con);
                dataTable = new DataTable();
                adp.Fill(dataTable);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            { 
            con.Close();
            }
            bindingSource1.DataSource = dataTable;
            dataGridView1.DataSource = bindingSource1;
            #endregion for select typeof Tovar
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            XmlTextWriter settingwriter = new XmlTextWriter("Zvit.xml", null);

            settingwriter.WriteStartDocument();
            settingwriter.WriteStartElement(dataGridView1.Name);
            int count = dataGridView1.Rows.Count;
            MessageBox.Show(count.ToString());
            count -= 1;         
            for (int i = 0; i < count; i++)
            {
              
                  
                    settingwriter.WriteStartElement("column");
                
                    settingwriter.WriteStartElement(dataGridView1.Columns[0].Name);
                    settingwriter.WriteString(dataGridView1.Rows[i].Cells[0].Value.ToString());
         
                    settingwriter.WriteEndElement();
                   
                    settingwriter.WriteStartElement(dataGridView1.Columns[1].Name);
                    settingwriter.WriteString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    settingwriter.WriteEndElement();
                    settingwriter.WriteStartElement("Ціна");
                    settingwriter.WriteString(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    settingwriter.WriteEndElement();    
                    settingwriter.WriteEndElement();
               
            }

            settingwriter.WriteEndElement();
  
            settingwriter.WriteEndDocument();
           
            settingwriter.Close();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string s = System.Configuration.ConfigurationManager.ConnectionStrings["GodownConnect"].ConnectionString.ToString();
            s=s.Remove(0, 12);
            string[] split = s.Split(new Char[] { ';'});
            LoadPackage();
            toolStripButton1.Enabled = false;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton3.Enabled = true;
            radioButton4.Enabled = true;
            toolStripButton2.Enabled = true;
            toolStripButton3.Enabled = true;
            toolStripButton4.Enabled = true;
            toolStripButton5.Enabled = true;
            toolStripButton6.Enabled = true;
            toolStripProgressBar1.Value = 100;
            toolStripStatusLabel1.Text = "Ви підключені до сервера -> " + split[0];
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now.ToString("Поточна година " + "HH:mm:ss");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            string s = treeView1.SelectedNode.ToString();
            s = s.Remove(0, 10);
          
            if (s == "Замовлення")
            {
                InvoiceSales inv = new InvoiceSales();
                inv.ShowDialog();
                treeView1_DoubleClick(sender, e);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
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

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            Font fon = new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold);
            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));

            e.Graphics.DrawImage(bm, 20, 50);

            Font s = new System.Drawing.Font("Times New Roman", 14);


            e.Graphics.DrawString("         ---- Звіт ----    ", s, Brushes.Black, 50, 20);

        }

        private void друкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(sender, e);  
        }

        private void переглядToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //string ss = treeView1.SelectedNode.ToString();
            //ss = ss.Remove(0, 10);
            //string delBank = "delete from [Банк] where [Банк].[№] =" + ind;

            int ind = dataGridView1.SelectedCells[1].RowIndex;
            dataGridView1.Rows.RemoveAt(ind);
         
           
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            #region hot keys
            if (e.Control && e.KeyCode == Keys.P)       // print
            {
                toolStripButton4_Click(sender, e);  
            }
            if (e.Control && e.KeyCode == Keys.W)       // preview
            {
                printDocument1.Print();
            }
            if (e.Control && e.Alt && e.KeyCode == Keys.S)       // preferences
            {
                налаштуванняToolStripMenuItem1_Click(sender, e);
            }
            if (e.Control && e.Alt && e.KeyCode == Keys.N)       // connect
            {
                toolStripButton1_Click(sender, e);
            }
            #endregion hot keys

        }

        private void підключитисьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(sender, e);
        }
     
    }
}
