using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Sqrland_Calcul
{
    static class Calc_Angle
    {

        public static void calc_angle(DataGridView dataGridView2) {

            string duplicated = dataGridView2.Rows[0].Cells[1].Value.ToString();
            string duplicated2 = dataGridView2.Rows[0].Cells[2].Value.ToString();
            int x = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells[1].Value.ToString() == duplicated.ToString() || dataGridView2.Rows[i].Cells[1].Value.ToString() == string.Empty)
                {

                }
                else
                {
                    double variable = 0;
                    int cp = 0;
                    List<double> lista_d = new List<double>();
                    List<double> lista_v = new List<double>();
                    List<string> lista_v_double = new List<string>();
                    List<int> lista_doublant = new List<int>();
                    for (int j = x; j < i; j++)
                    {
                        for (int y = x; y < i; y++)
                        {
                            if (dataGridView2.Rows[j].Cells[2].Value.ToString() == dataGridView2.Rows[y].Cells[2].Value.ToString() && j != y)
                            {
                                cp++;
                                double distance = Convert.ToDouble(dataGridView2.Rows[j].Cells[5].Value) - Convert.ToDouble(dataGridView2.Rows[y].Cells[5].Value);
                                double angle = Convert.ToDouble(dataGridView2.Rows[j].Cells[6].Value) - Convert.ToDouble(dataGridView2.Rows[y].Cells[6].Value);
                                if (distance <= 10 && angle <= 0.001)
                                {
                                    variable = Convert.ToDouble(dataGridView2.Rows[y].Cells[3].Value);
                                    //moyenne = (Convert.ToDouble(dataGridView2.Rows[j].Cells[3].Value) + Convert.ToDouble(dataGridView2.Rows[y].Cells[3].Value)) / 2;
                                    lista_d.Add(distance);
                                    lista_v.Add(variable);
                                    
                                }
                                if (lista_v_double.Count == 0)
                                {
                                    lista_v_double.Add(dataGridView2.Rows[j].Cells[2].Value.ToString());
                                }
                                else
                                {
                                    string tt= null;
                                    foreach (string item in lista_v_double)
                                    {
                                        if (item == dataGridView2.Rows[j].Cells[2].Value.ToString())
                                        {
                                            //MessageBox.Show(j.ToString());
                                            lista_doublant.Add(j);
                                        }
                                        else
                                        {
                                            tt = dataGridView2.Rows[j].Cells[2].Value.ToString();
                                        }
                                    }
                                    if (tt != null)
                                    {
                                        lista_v_double.Add(tt);
                                    }
                                }
                            }
                        }
                    }
                    for (int z = x; z < i; z++)
                    {
                        if (cp == 2)
                        {

                            dataGridView2.Rows[z].Cells[4].Value = Convert.ToDouble(dataGridView2.Rows[z].Cells[3].Value) - variable;
                            if (Convert.ToDouble(dataGridView2.Rows[z].Cells[4].Value) < 0)
                            {
                                dataGridView2.Rows[z].Cells[4].Value = Convert.ToDouble(dataGridView2.Rows[z].Cells[4].Value) + 400 ;
                            }
                            
                            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
                            cn.Open();
                            SQLiteCommand cmd = new SQLiteCommand("update observation_row set Ah2 = " + dataGridView2.Rows[z].Cells[4].Value + " where id = " + dataGridView2.Rows[z].Cells[0].Value, cn);
                            cmd.ExecuteNonQuery();
                            cn.Close();

                        }
                        else if(cp > 2)
                        {
                            int w = 0;
                            double tst = 0;
                           for(int s = 0; i < lista_d.Count; i++)
                            {
                                if (s == 0)
                                    tst = lista_d[s];
                                if (lista_d[s] < tst)
                                {
                                    tst = lista_d[s];
                                    w = s;
                                }
                            }
                            w++;
                            for(int f=x; f < i; f++)
                            {

                                dataGridView2.Rows[f].Cells[4].Value = Convert.ToDouble(dataGridView2.Rows[f].Cells[3].Value) - lista_v[w];
                                if (Convert.ToDouble(dataGridView2.Rows[f].Cells[4].Value) < 0)
                                {
                                    dataGridView2.Rows[f].Cells[4].Value = Convert.ToDouble(dataGridView2.Rows[f].Cells[4].Value) + 400;
                                }
                                else if (Convert.ToDouble(dataGridView2.Rows[f].Cells[4].Value) > 400)
                                {
                                    //dataGridView2.Rows[z].Cells[4].Value = Convert.ToDouble(dataGridView2.Rows[z].Cells[4].Value) + 400;
                                }
                                SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
                                cn.Open();
                                SQLiteCommand cmd = new SQLiteCommand("update observation_row set Ah2 = " + dataGridView2.Rows[f].Cells[4].Value + " where id = " + dataGridView2.Rows[f].Cells[0].Value, cn);
                                cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                        }
                        /*else if (cp < 2)
                        {
                            dataGridView2.Rows[z].Cells[4].Value = Convert.ToDouble(dataGridView2.Rows[z].Cells[3].Value) - Convert.ToDouble(dataGridView2.Rows[1].Cells[3].Value);
                            if (Convert.ToDouble(dataGridView2.Rows[z].Cells[4].Value) < 0)
                            {
                                dataGridView2.Rows[z].Cells[4].Value = Convert.ToDouble(dataGridView2.Rows[z].Cells[4].Value) + 400;
                            }

                            SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
                            cn.Open();
                            SQLiteCommand cmd = new SQLiteCommand("update observation_row set Ah2 = " + dataGridView2.Rows[z].Cells[4].Value + " where id = " + dataGridView2.Rows[z].Cells[0].Value, cn);
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }*/
                        

                    }

                    foreach (int o in lista_doublant){
                        SQLiteConnection cn = new SQLiteConnection("Data Source= sqrLand.db");
                        cn.Open();
                        SQLiteCommand cmd = new SQLiteCommand("delete from observation_row where id = " + dataGridView2.Rows[o].Cells[0].Value, cn);
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                    x = i;
                }
            }
        }
    }
}
