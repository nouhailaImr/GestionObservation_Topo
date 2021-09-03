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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> listPointDeVise = new List<string>();
            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
            cn.Open();
            SQLiteCommand com = new SQLiteCommand("select DISTINCT Point_vise from observation_row where fixe = 1 and  Station like '" + comboBox3.SelectedValue + "' and id_observation = " + id, cn);
            SQLiteDataReader dr = com.ExecuteReader();
            while (dr.Read())
                listPointDeVise.Add(dr[0].ToString());
            comboBox4.DataSource = null;
            comboBox4.DataSource = listPointDeVise;
            dr.Close();
            cn.Close();
        }
    }
}
