using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace Godown_0._01_
{
    public class GlobalFunc
    {
        SqlConnection con;
        string connection_string = @"Server=ADMIN-LAPTOP\SQL2014; Database=Godown; User ID=sa; Password=1";
        ComboBox comboBox2;
        ComboBox comboBox3;
       public GlobalFunc()
        {
            GetAllMfo();
            //GetMfoFromName();
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
    }


}
