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

        public Form1()
        {
            InitializeComponent();

        }

        string extension;
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            switch (extension)
            {
                case "text":

                    DataTable table = new DataTable();
                    DataTable tableDB = new DataTable();

                    string[] lines = File.ReadAllLines(textpath.Text);
                    int max = 0;

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
                        if (list2.Count > max)
                            max = list2.Count;


                    }
                    
                    for (int i = 1; i <= max; i++)
                    {
                        table.Columns.Add("Column "+i.ToString());
                        tableDB.Columns.Add("Column "+i.ToString());
                    }


                    foreach (string line in lines)
                    {
                        string[] values = line.ToString().Split(' ');
                        List<string> list2 = new List<string>();
                        List<string> list3 = new List<string>();
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] != string.Empty)
                            {
                                bool tst = true;
                                for (int j = 0; j < table.Rows.Count; j++)
                                {
                                    var tt = table.Rows[j][0];
                                    if (tt.ToString() == values[i] && tt.ToString() == values[0])
                                    {
                                        list2.Add("");
                                        tst = false;
                                    }
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

                    dataGridView1.DataSource = table;
                    mydb databaseObject = new mydb(max,tableDB);
                    break;

                case "excel":

                    using (var stream = File.Open(textpath.Text, FileMode.Open, FileAccess.Read))
                    {

                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            dataGridView1.DataSource = result.Tables[0];
                        }
                    }

                    break;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "XLS files (*.xls, *.xlt)|*.xls;*.xlt|XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|Text Files(*.txt)|*.txt;";

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
                if (st[1] == "xls" || st[1] == "xlt" || st[1] == "xlsx" || st[1] == "xlsm" || st[1] == "xltx" || st[1] == "xltm")
                {
                    extension = "excel";
                }
            }
            catch (Exception) { }


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("drdrcrc");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}

