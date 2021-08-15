using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sqrland_Calcul
{
    public partial class FrmLoad : Form
    {
        public FrmLoad()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable();
        SQLiteConnection connection;
        private void FrmLoad_Load(object sender, EventArgs e)
        {
            connection = new SQLiteConnection("Data Source= sqrLand.db");
            mydb mydb = new mydb();
            refresh();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            connection.Open();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO observation values (null,'"+ textName.Text+ "', '"+textDescreption.Text+"',Datetime())", connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            refresh();
        }

       private void refresh()
        {
            connection.Open();
            SQLiteCommand commande = new SQLiteCommand("SELECT * FROM observation", connection);
            SQLiteDataReader dr = commande.ExecuteReader();
            DataTable dt = new DataTable("observation");
            dt.Load(dr);
            connection.Close();
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                Form1 frm = new Form1(id);
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
            else if(colName == "Delete")
            {
                if (MessageBox.Show("Are u sure?","Warning",MessageBoxButtons.YesNo)== DialogResult.Yes)
                {
                    connection.Open();
                    SQLiteCommand cmd = new SQLiteCommand("delete from observation where id="+id, connection);
                    cmd.ExecuteNonQuery();
                    cmd = new SQLiteCommand("delete from observation_row where id_observation =" + id, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    refresh();
                }
               
            }
        }
    }
}
