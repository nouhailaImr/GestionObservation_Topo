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
    public partial class FrmRayonmt : Form
    {
        int id;

        List<Rayonnement> rayonnement = new List<Rayonnement>();
        public FrmRayonmt(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void FrmRayonmt_Load(object sender, EventArgs e)
        {
            List<string> listStation = new List<string>();
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            SQLiteCommand com = new SQLiteCommand("select DISTINCT Station from observation_row where id_observation = " + id, cn);
            SQLiteDataReader dr = com.ExecuteReader();
            while (dr.Read())
                listStation.Add(dr[0].ToString());
            comboBox3.DataSource = listStation;
            dr.Close();
            cn.Close();
        }



        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            //List<string> listPoint = new List<string>();
           /* SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            SQLiteCommand com = new SQLiteCommand("select DISTINCT Point_vise from observation_row where fixe = 1 and  Station like '" + comboBox3.SelectedValue + "' and id_observation = " + id, cn);
            SQLiteDataReader dr = com.ExecuteReader();
            //while (dr.Read())
                //listPoint.Add(dr[0].ToString());
            //dgRayonmt.DataSource = null;
            //dgRayonmt.DataSource = listPoint;
            dr.Close();
            cn.Close();*/
        }

        private void btn_ajouter_Click(object sender, EventArgs e)
        {
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            SQLiteCommand com = new SQLiteCommand("select point_Vise from observation_row where station  like '" + comboBox3.SelectedValue + "' and id_observation = " + id, cn);
            SQLiteDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                Rayonnement rayonnements = new Rayonnement();
                rayonnements.Station = comboBox3.SelectedValue.ToString();
                rayonnements.Point = dr[0].ToString();
                //rayonnements.Ah2 = double.Parse(dr[1].ToString());
                /*rayonnements.Gisement = 0;
                rayonnements.Distance = double.Parse(dr[1].ToString());
                if (dr[2].ToString() != string.Empty)
                    rayonnements.X = double.Parse(dr[2].ToString());
                if (dr[3].ToString() != string.Empty)
                    rayonnements.Y = double.Parse(dr[3].ToString());
                rayonnements.Ref = dr[5].ToString();*/
                rayonnement.Add(rayonnements);
            }


            dr.Close();
            cn.Close();

            dgRayonmt.DataSource = null;
            dgRayonmt.DataSource = rayonnement;
            /*dgRayonmt.Columns[0].Visible = false;
            dgRayonmt.Columns[2].Visible = false;
            dgRayonmt.Columns[3].Visible = false;
            dgRayonmt.Columns[4].Visible = false;
            dgRayonmt.Columns[5].Visible = false;
            dgRayonmt.Columns[6].Visible = false;
            dgRayonmt.Columns[7].Visible = false;*/
        }
    

    private void button2_Click(object sender, EventArgs e)
    {
        /*rayonnement.RemoveAt(dgRayonmt.CurrentRow.Index);
        dgRayonmt.DataSource = null;
        dgRayonmt.DataSource = rayonnement;
        dgRayonmt.Columns[2].Visible = false;
        dgRayonmt.Columns[3].Visible = false;
        dgRayonmt.Columns[4].Visible = false;
        dgRayonmt.Columns[5].Visible = false;
        dgRayonmt.Columns[6].Visible = false;*/
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Calc_Angle.rayonnements = Calc_Rayonmt.Main(rayonnement);
        if (Calc_Angle.rayonnements != null)
        {
            this.Hide();
            FrmResultatRayonmt f = new FrmResultatRayonmt();
            f.ShowDialog();
            this.Show();
        }
    }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
