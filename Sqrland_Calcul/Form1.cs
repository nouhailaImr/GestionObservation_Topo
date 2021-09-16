using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Sqrland_Calcul
{
    public partial class Form1 : Form
    {
        SQLiteDataAdapter adpt;
        DataTable dt;
        string id;
        string extension;
        string filename;
        public Form1(string id)
        {
            InitializeComponent();
            this.id = id;

        }
        //Load --Uplaod to database--
        private void btn_load(object sender, EventArgs e)
        {
            switch (extension)
            {
                case "text":
                    DataTable table = new DataTable();
                    DataTable tableDB = new DataTable();
                    table.Columns.Add("station");
                    table.Columns.Add("point vise");
                    table.Columns.Add("Ah1");
                    table.Columns.Add("Ah2");
                    table.Columns.Add("distance");
                    table.Columns.Add("Av");
                    table.Columns.Add("hp");
                    table.Columns.Add("hs");
                    table.Columns.Add("X");
                    table.Columns.Add("Y");
                    table.Columns.Add("Z");

                    string[] lines = File.ReadAllLines(textpath.Text);
                    List<string> liststa = new List<string>();
                    foreach(DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Cells[2].Value.ToString() != string.Empty)
                            liststa.Add(row.Cells[2].Value.ToString());
                    }
                    foreach (string line in lines)
                    {
                        string[] values = line.ToString().Split(' ');
                        List<string> list2 = new List<string>();
                        int cp = 0;
                        for (int i = 0; i < values.Length; i++)
                        { 
                            if (values[i] != string.Empty)
                            {
                                cp++;
                                if (cp == 4)
                                {
                                    list2.Add(null);
                                }
                                list2.Add(values[i]);

                            }
                        }
                        string station = list2[0];
                        if (!liststa.Contains(station) && station is string)
                        {
                            table.Rows.Add(new string[] { station, null, null, null, null, null });
                            liststa.Add(station);
                        }
                        table.Rows.Add(list2.ToArray());
                    }

                    mydb databaseObject = new mydb(table, int.Parse(id),filename);
                    FillDataGridView();
                    break;
            }
        }

        //Importation
        private void btn_import(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Text Files(*.txt)|*.txt;";
            

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                this.textpath.Text = openfile.FileName;
            }
            string[] st = openfile.SafeFileName.Split('.');
            extension = Path.GetExtension(textpath.Text);
            try
            {
                if (st[1] == "txt")
                {
                    extension = "text";
                }
            }
            catch (Exception) { }
            filename = openfile.SafeFileName;
        }

        private void btn_update(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
                cn.Open();
                for(int i = 3; i < row.Cells.Count-2; i++)
                {
                    if(i != 5)
                    {
                        string value = row.Cells[i].Value.ToString();
                        if (value == string.Empty && i>3)
                        {
                            value = "null";
                        }
                        string query="";
                        switch (i)
                        {
                            case 3:query = "point_vise = '"+value+"'";break;
                            case 4:query = "Ah1 = "+value;break;
                            case 6:query = "Distance = "+value;break;
                            case 7:query = "Av = "+value;break;
                            case 8:query = "hp = "+value;break;
                            case 9:query = "hs = "+value;break;
                            case 10:query = "X = "+value;break;
                            case 11:query = "Y = "+value;break;
                            case 12:query = "Z = "+value;break;
                        }
                        SQLiteCommand cmd = new SQLiteCommand("update observation_row set " + query + " where id = " + row.Cells[0].Value, cn);
                        cmd.ExecuteNonQuery();
                    }
                }
               

            }
            MessageBox.Show("update with success");
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            FillDataGridView();
        }

        private void FillDataGridView()
        {
            try
            {
                SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
                cn.Open();
                adpt = new SQLiteDataAdapter("select * from observation_row where id_observation = " + id + " order by Station, Ah2", cn);
                dt = new DataTable();
                adpt.Fill(dt);

                dataGridView2.DataSource = dt;

                if (dataGridView2.Rows.Count > 0)
                {
                    string removedup = dataGridView2.Rows[0].Cells[2].Value.ToString();
                    for (int i = 1; i < dataGridView2.Rows.Count; i++)
                    {
                        if (dataGridView2.Rows[i].Cells[2].Value.ToString() == removedup)
                        {
                            dataGridView2.Rows[i].Cells[2].Value = string.Empty;
                        }
                        else
                        {
                            removedup = dataGridView2.Rows[i].Cells[2].Value.ToString();
                        }

                    }
                }

                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Width = 35;
                dataGridView2.Columns[13].Visible = false;
                dataGridView2.Columns[14].Visible = false;
            }
            catch (Exception) { }
        }
        int index=-15;
        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            for (int j = 0; j < dataGridView2.Rows.Count; j++)
            {
                string duplicated = dataGridView2.Rows[j].Cells[2].Value.ToString();

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (e.ColumnIndex == 3 && e.Value.ToString() != string.Empty)
                    {
                        string val = Convert.ToString(e.Value);
                        if (val == duplicated)
                        {
                            e.CellStyle.ForeColor = Color.White;
                            e.CellStyle.BackColor = Color.FromArgb(27, 91, 104);
                        }
                    }
                    if (e.ColumnIndex == 3 && e.Value.ToString() == string.Empty)
                    {
                        index = e.RowIndex;
                    }
                    if (index != -15 && e.RowIndex == index && e.ColumnIndex >2 && e.ColumnIndex<10)
                    {
                        e.CellStyle.BackColor = Color.FromArgb(228, 233, 224);
                    }
                    
                }
            }
        }

        private void btn_angle(object sender, EventArgs e)
        {
            Calc_Angle.calc_angle(dataGridView2);
            FillDataGridView();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(.ToString());
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            if (!(bool)dataGridView2.Rows[e.RowIndex].Cells[1].Value)
            {
                SQLiteCommand cmd = new SQLiteCommand("update observation_row set fixe = 1 where id = " + dataGridView2.Rows[e.RowIndex].Cells[0].Value, cn);
                cmd.ExecuteNonQuery();
            }
            else
            {
                SQLiteCommand cmd = new SQLiteCommand("update observation_row set fixe = 0 where id = " + dataGridView2.Rows[e.RowIndex].Cells[0].Value, cn);
                cmd.ExecuteNonQuery();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmCheminement ch = new FrmCheminement(int.Parse(id));
            this.Hide();
            ch.ShowDialog();
            this.Show();

        }
       
        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (e.ColumnIndex == 2)
                {
                    if (e.Value.ToString() == string.Empty)
                    {
                        e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                        e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    else
                    {
                        e.AdvancedBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Inset;
                    }
                   

                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_raynmt_Click(object sender, EventArgs e)
        {
            FrmRayonmt ch = new FrmRayonmt(int.Parse(id));
            this.Hide();
            ch.ShowDialog();
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}



