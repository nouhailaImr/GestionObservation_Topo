using Amazon.Athena.Model;
using Microsoft.TeamFoundation.Build.WebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Sqrland_Calcul
{
    class mydb
    {
        public SQLiteConnection connection;
        public mydb()
        {
            connection = new SQLiteConnection("Data Source= sqrLand.db");
            connection.Open();
            if (!File.Exists("./sqrLand.db"))
            {
                
                SQLiteConnection.CreateFile("sqrLand.db");
            }
               
                SQLiteCommand cmd = new SQLiteCommand(
                    "CREATE TABLE IF NOT EXISTS  Observation (" +
                        "\"id\"    INTEGER," +
                        "\"Name\"   TEXT," +
                        "\"Description\"   TEXT," +
                        "\"Created_At\"  DATE," +
                        "PRIMARY KEY(\"id\" AUTOINCREMENT)" +
                    ");" +
                    "CREATE TABLE IF NOT EXISTS  \"Observation_row\"(" +
                        "\"id\"    INTEGER," +
                        "\"Station\"   TEXT," +
                        "\"Point_vise\"    TEXT," +
                        "\"Ah1\"    DOUBLE," +
                        "\"Ah2\"    DOUBLE," +
                        "\"Distance\"  DOUBLE," +
                        "\"Av\" double," +
                        "\"hp\" double," +
                        "\"hs\" double," +
                        "\"Z\" double," +
                        "\"id_observation\"    INTEGER NOT NULL," +
                        "PRIMARY KEY(\"id\" AUTOINCREMENT)," +
                        "FOREIGN KEY(\"id_observation\") REFERENCES \"Observation\"(\"id\")" +
                    ");", connection);

                cmd.ExecuteNonQuery();
                connection.Close();
            //}
            
        }
        public mydb(DataTable dtIn, int idObs)
        {
            connection = new SQLiteConnection("Data Source= sqrLand.db");
            Fill(dtIn, idObs);
        }

        private void Fill(DataTable dt, int id)
        {
            connection.Open();
            
            for(int i = 0;i<dt.Rows.Count;i++)
            {
                string query = "INSERT INTO Observation_Row values (null,'";
                var item = dt.Rows[i].ItemArray;
                for(int j=0;j<item.Length;j++)
                {
                    
                    if (j == 0)
                        query += item[j] + "','";
                    else if (j == 1)
                        query += item[j] + "',";
                    else if (item[j].GetType().Equals(typeof(DBNull)))
                        query += "null,";
                    else
                        query += item[j] + ",";
                        
                }
                query += id+");";
                SQLiteCommand com = new SQLiteCommand(query, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
