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
        SQLiteCommandBuilder builder;
        public mydb(int count, DataTable dtIn)
        {
            connection = new SQLiteConnection("Data Source= sqrLand.db");
            if (!File.Exists("./sqrLand.db"))
            {
                SQLiteConnection.CreateFile("sqrLand.db");
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE observation([Column 1] VARCHAR(100),[Column 2] VARCHAR(100), constraint pkObservation primary key ([Column 1],[Column 2]) )", connection);

                cmd.ExecuteNonQuery();
                for (int i = 3; i < count + 1; i++)
                {
                    addColumns("column " + i);
                }

                Fill(dtIn);
            }
            else
            {
                connection.Open();
                SQLiteCommand commande = new SQLiteCommand("SELECT * FROM observation", connection);
                SQLiteDataReader dr = commande.ExecuteReader();
                DataTable dt = new DataTable("observation");
                dt.Load(dr);
                int dbTableCount = dt.Columns.Count;

                while (count > dbTableCount)
                {
                    addColumns("column " + ++dbTableCount);
                }

                Fill(dtIn);
            }
        }

        private void addColumns(string name)
        {
            SQLiteCommand cmd = new SQLiteCommand("ALTER TABLE observation ADD column [" + name + "] VARCHAR(100)", connection);
            cmd.ExecuteNonQuery();
        }

        private void Fill(DataTable dt)
        {
            SQLiteCommand com = new SQLiteCommand("SELECT * FROM observation", connection);
            SQLiteDataReader dr = com.ExecuteReader();
            DataTable datatb = new DataTable("ob");
            datatb.Load(dr);


            SQLiteDataAdapter da = new SQLiteDataAdapter(com);
            builder = new SQLiteCommandBuilder(da);

            DataTable dtfin = new DataTable();
            int max = datatb.Columns.Count;
            if (dt.Columns.Count > max)
                max = dt.Columns.Count;
            for (int i = 1; i < max + 1; i++)
            {
                dtfin.Columns.Add("Column " + i);
            }
            for (int i = 0; i < datatb.Rows.Count ; i++)
            {
                List<string> list = new List<string>();
                for (int j = 0; j < datatb.Columns.Count; j++)
                {
                    list.Add(datatb.Rows[i][j].ToString());
                }
                dtfin.Rows.Add(list.ToArray());
            }

           for (int i = 0; i < dt.Rows.Count; i++)
           {
                List<string> list = new List<string>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    list.Add(dt.Rows[i][j].ToString());
                }
                dtfin.Rows.Add(list.ToArray());
            }


            try
            {
                da.Update(dtfin);
            }
            catch (Exception) { }
        }
    }
}
