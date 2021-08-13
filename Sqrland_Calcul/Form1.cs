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
                    table.Columns.Add("Z");

                    string[] lines = File.ReadAllLines(textpath.Text);

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
                        table.Rows.Add(list2.ToArray());

                    }
                    mydb databaseObject = new mydb(table, int.Parse(id));
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
        }

        private void btn_update(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
                cn.Open();

                string tsthp = row.Cells[7].Value.ToString();
                string tsths = row.Cells[8].Value.ToString();
                string tstZ = row.Cells[9].Value.ToString();

                if (tsthp != string.Empty)
                {
                    SQLiteCommand cmd = new SQLiteCommand("update observation_row set hp = " + tsthp + " where id = " + row.Cells[0].Value, cn);
                    cmd.ExecuteNonQuery();
                }
                if (tsths != string.Empty)
                {
                    SQLiteCommand cmd = new SQLiteCommand("update observation_row set hs = " + tsths + " where id = " + row.Cells[0].Value, cn);
                    cmd.ExecuteNonQuery();
                }
                if (tstZ != string.Empty)
                {
                    SQLiteCommand cmd = new SQLiteCommand("update observation_row set Z = " + tstZ + " where id = " + row.Cells[0].Value, cn);
                    cmd.ExecuteNonQuery();
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
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            /*SQLiteCommand cmd = new SQLiteCommand("select station,Point_vise,ah1,ah2,distance,av,hp,hs,z from observation_row where id_observation like " + id + " order by Station", cn);
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("obs");
            dt.Load(dr);*/
            adpt = new SQLiteDataAdapter("select * from observation_row where id_observation like " + id + " order by Station, Ah2", cn);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView2.DataSource = dt;
            
            if (dataGridView2.Rows.Count > 0)
            {
                string removedup = dataGridView2.Rows[0].Cells[1].Value.ToString();
                for (int i = 1; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[1].Value.ToString() == removedup)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = string.Empty;
                    }
                    else
                        removedup = dataGridView2.Rows[i].Cells[1].Value.ToString();
                }
            }
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[10].Visible = false;


        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            for (int j = 0; j < dataGridView2.Rows.Count; j++)
            {
                string duplicated = dataGridView2.Rows[j].Cells[1].Value.ToString();
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (e.ColumnIndex == 2 && e.Value != null)
                    {
                        string val = Convert.ToString(e.Value);
                        if (val == duplicated)
                        {
                            e.CellStyle.ForeColor = Color.White;
                            e.CellStyle.BackColor = Color.FromArgb(27, 91, 104);
                        }
                    }

                }
            }
        }

        private void btn_angle(object sender, EventArgs e)
        {
            Calc_Angle.calc_angle(dataGridView2);
        }

        
    }
}



