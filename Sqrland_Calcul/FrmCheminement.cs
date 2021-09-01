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
    public partial class FrmCheminement : Form
    {
        int id;
        List<Cheminement> cheminements = new List<Cheminement>();
        public FrmCheminement(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void cheminement_Load(object sender, EventArgs e)
        {
            List<string> listStation = new List<string>();
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            SQLiteCommand com = new SQLiteCommand("select DISTINCT Station from observation_row where id_observation = "+id, cn);
            SQLiteDataReader dr = com.ExecuteReader();
            while (dr.Read())
                listStation.Add(dr[0].ToString());
            comboBox2.DataSource = listStation;
            dr.Close();
            cn.Close();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            List<string> listPointDeVise = new List<string>();
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            SQLiteCommand com = new SQLiteCommand("select DISTINCT Point_vise from observation_row where fixe = 1 and  Station like '"+comboBox2.SelectedValue+"' and id_observation = "+id, cn);
            SQLiteDataReader dr = com.ExecuteReader();
            while (dr.Read())
                listPointDeVise.Add(dr[0].ToString());
            comboBox1.DataSource = null;
            comboBox1.DataSource = listPointDeVise;
            dr.Close();
            cn.Close();
        }

        private void btn_Calc_Click(object sender, EventArgs e)
        {
            if (comboBox2.Items.Count == 0 || comboBox1.Items.Count == 0)
                MessageBox.Show("Vous devez choisir un point !!");
            else
            {
                foreach(Cheminement cheminement in cheminements)
                {
                    if(cheminement.Station.Equals(comboBox2.SelectedValue.ToString()) && cheminement.Point.Equals(comboBox1.SelectedValue.ToString()))
                    {
                        MessageBox.Show("Le point est deja choisit !!");
                        return;
                    }
                }
                SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
                cn.Open();
                SQLiteCommand com = new SQLiteCommand("select Ah2,Distance,X,Y,Ref from observation_row where point_Vise  like '"+ comboBox1.SelectedValue + "' and Station like '" + comboBox2.SelectedValue + "' and id_observation = " + id, cn);
                SQLiteDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    Cheminement cheminement = new Cheminement();
                    cheminement.Station = comboBox2.SelectedValue.ToString();
                    cheminement.Point = comboBox1.SelectedValue.ToString();
                    cheminement.Ah2 = double.Parse(dr[0].ToString());
                    cheminement.Gisement = 0;
                    cheminement.Distance = double.Parse(dr[1].ToString());
                    if (dr[2].ToString() != string.Empty)
                        cheminement.X = double.Parse(dr[2].ToString());
                    if (dr[3].ToString() != string.Empty)
                        cheminement.Y = double.Parse(dr[3].ToString());
                    cheminement.Ref = dr[4].ToString();
                    cheminements.Add(cheminement);
                }
                    

                dr.Close();
                cn.Close();
               
                dgCheminement.DataSource = null;
                dgCheminement.DataSource = cheminements;
                dgCheminement.Columns[2].Visible = false;
                dgCheminement.Columns[3].Visible = false;
                dgCheminement.Columns[4].Visible = false;
                dgCheminement.Columns[5].Visible = false;
                dgCheminement.Columns[6].Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cheminements.RemoveAt(dgCheminement.CurrentRow.Index); 
            dgCheminement.DataSource = null;
            dgCheminement.DataSource = cheminements;
            dgCheminement.Columns[2].Visible = false;
            dgCheminement.Columns[3].Visible = false;
            dgCheminement.Columns[4].Visible = false;
            dgCheminement.Columns[5].Visible = false;
            dgCheminement.Columns[6].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cheminements.Count < 4)
                MessageBox.Show("fegfregegeg");
            else
            {
                Calc_Angle.cheminements = Calc_Cheminement.Main(cheminements);
                if(Calc_Angle.cheminements != null)
                {
                    this.Hide();
                    FrmResultatChemeniment f = new FrmResultatChemeniment();
                    f.ShowDialog();
                    this.Show();
                }
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dgCheminement_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
