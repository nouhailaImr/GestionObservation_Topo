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
        string id;
        string extension;
        bool testTableEmpty = true;
        public Form1(string id)
        {
            InitializeComponent();
            this.id = id;

        }
        //Load --Uplaod to database--
        private void button1_Click(object sender, EventArgs e)
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
                                if (cp == 4 && !testTableEmpty)
                                {
                                    list2.Add(null);
                                }
                                else if (cp == 4 && testTableEmpty)
                                {
                                    list2.Add("0");
                                }
                                list2.Add(values[i]);
                            }
                        }
                        table.Rows.Add(list2.ToArray());

                    }
                    mydb databaseObject = new mydb(table, int.Parse(id));
                    FillDataGridView();
                    testTableEmpty = true;
                    break;
            }
        }

        //Importation
        private void button2_Click(object sender, EventArgs e)
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show("drdrcrc");


        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillDataGridView();
            //this.dataGridView2.Rows[1].DefaultCellStyle.BackColor = Color.Cornsilk;


        }

        private void FillDataGridView()
        {
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            SQLiteCommand cmd = new SQLiteCommand("select station,Point_vise,ah1,ah2,distance,av,hp,hs,z from observation_row where id_observation like " + id + " order by Station", cn);
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("obs");
            dt.Load(dr);
            dataGridView2.DataSource = dt;
            if (dt.Rows.Count == 0)
                testTableEmpty = false;

            if (dataGridView2.Rows.Count > 0)
            {
                string removedup = dataGridView2.Rows[0].Cells[0].Value.ToString();
                for (int i = 1; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[0].Value.ToString() == removedup)
                    {
                        dataGridView2.Rows[i].Cells[0].Value = string.Empty;
                    }
                    else
                        removedup = dataGridView2.Rows[i].Cells[0].Value.ToString();
                }
            }


        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            for (int j = 0; j < dataGridView2.Rows.Count; j++)
            {
                string duplicated = dataGridView2.Rows[j].Cells[0].Value.ToString();
                for (int i = 0;  i < dataGridView2.Rows.Count; i++)
                {
                    if( e.ColumnIndex==1 & e.Value != null)
                    {
                        string val = Convert.ToString(e.Value);
                        if (val == duplicated)
                        {
                            e.CellStyle.ForeColor = Color.White;
                            e.CellStyle.BackColor = Color.DeepPink;
                        }
                    }
                    
                }
            }
                

                

            



        }
    }
}

