using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private void Form1_Load(object sender, EventArgs e)
        {

            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            table.Columns.Add("Element", typeof(string));
            table.Columns.Add("Matricule", typeof(string));
            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            table.Columns.Add("Z", typeof(int));
            table.Columns.Add("D", typeof(int));

            string[] lines = File.ReadAllLines(textpath.Text);

            foreach(string line in lines)
            {
                string[] values = line.ToString().Split('\t');
                table.Rows.Add(values);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "XLS files (*.xls, *.xlt)|*.xls;*.xlt|XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|CSV files (*.csv, *.tsv)|*.csv;*.tsv|Text Files(*.txt)|*.txt;";

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                this.textpath.Text = openfile.FileName;
            }
        }
    }
}
