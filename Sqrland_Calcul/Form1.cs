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

                    tableDB.Columns.Add("station");
                    tableDB.Columns.Add("point vise");
                    tableDB.Columns.Add("Ah1");
                    tableDB.Columns.Add("Ah2");
                    tableDB.Columns.Add("distance");
                    tableDB.Columns.Add("Av");
                    tableDB.Columns.Add("hp");
                    tableDB.Columns.Add("hs");
                    tableDB.Columns.Add("Z");




                    string[] lines = File.ReadAllLines(textpath.Text);

                    foreach (string line in lines)
                    {
                        string[] values = line.ToString().Split(' ');
                        List<string> list2 = new List<string>();
                        foreach (string str in values)
                        {
                            if (str != string.Empty)
                            {
                                list2.Add(str);
                            }
                        }
                    }

                    foreach (string line in lines)
                    {
                        string[] values = line.ToString().Split(' ');
                        List<string> list2 = new List<string>();
                        List<string> list3 = new List<string>();
                        int cp = 0;
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] != string.Empty)
                            {
                                cp++;
                                bool tst = true;
                                for (int j = 0; j < table.Rows.Count; j++)
                                {
                                    var tt = table.Rows[j][0];
                                    if (tt.ToString() == values[0])
                                    {
                                        list2.Add(null);
                                        tst = false;
                                    }
                                }
                                if (cp == 4 && testTableEmpty == false)
                                {
                                    list2.Add(null);
                                    list3.Add(null);
                                }
                                if (tst)
                                {
                                    list2.Add(values[i]);
                                }
                                list3.Add(values[i]);
                            }
                        }
                        table.Rows.Add(list2.ToArray());
                        tableDB.Rows.Add(list3.ToArray());

                    }

                    dataGridView2.DataSource = table;
                    mydb databaseObject = new mydb(tableDB, int.Parse(id));
                    break;

                    /*case "excel":

                        using (var stream = File.Open(textpath.Text, FileMode.Open, FileAccess.Read))
                        {

                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });
                                dataGridView2.DataSource = result.Tables[0];
                            }
                        }

                        break;*/
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
                /*if (st[1] == "xls" || st[1] == "xlt" || st[1] == "xlsx" || st[1] == "xlsm" || st[1] == "xltx" || st[1] == "xltm")
                {
                    extension = "excel";
                }*/
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
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            SQLiteCommand cmd = new SQLiteCommand("select station,Point_vise,ah1,ah2,distance,av,hp,hs,z from observation_row", cn);
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable("obs");
            dt.Load(dr);
            dataGridView2.DataSource = dt;
            if (dt.Rows.Count == 0)
                testTableEmpty = false;

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
}

