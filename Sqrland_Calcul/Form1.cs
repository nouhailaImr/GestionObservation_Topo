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

namespace Sqrland_Calcul
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable();
        
        string extension;
        private void Form1_Load(object sender, EventArgs e)
        {

            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (extension)
            {
                case ".txt":
                    table.Columns.Add("Element");
                    table.Columns.Add("Matricule");
                    table.Columns.Add("X");
                    table.Columns.Add("Y");
                    table.Columns.Add("Z");
                    table.Columns.Add("D");

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
                        table.Rows.Add(list2.ToArray());
                    }
                    break;

                case "excel":
                    /*string name = "Items";
                     string constr = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source=" + textpath.Text + "; Extented Properties =\"Excel 8.0; HDR=Yes;\";";
                     OleDbConnection con = new OleDbConnection(constr);
                     OleDbDataAdapter sda = new OleDbDataAdapter("Select * From [Sheet1$]", con);
                     DataTable data = new DataTable();
                     sda.Fill(data);
                     break;*/

                    break;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "XLS files (*.xls, *.xlt)|*.xls;*.xlt|XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|CSV files (*.csv, *.tsv)|*.csv;*.tsv|Text Files(*.txt)|*.txt;";

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                this.textpath.Text = openfile.FileName;
            }
            string[] st = openfile.SafeFileName.Split('.');
            extension = Path.GetExtension(textpath.Text);
            /*if (st[1] == "txt")
            {
                extension = "text";
            }
            if (st[1] == "xls" || st[1] == "xlsx")
            {
                extension = "excel";
            }
            if (st[1] == "csv" || st[1] == "tsv")
            {
                extension = "csv";
            }
            label1.Text = extension;*/
        }
    }
}

