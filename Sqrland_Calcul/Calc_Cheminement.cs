using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sqrland_Calcul
{
    class Calc_Cheminement
    {
        public static List<Cheminement> Main(List<Cheminement> cheminements)
        {
            double x1 = cheminements[0].X;
            double x2 = cheminements[1].X;
            double y2 = cheminements[1].X;
            double y1 = cheminements[0].X;
            int n = cheminements.Count;
            double G1=0 ;
            double dx = x1 - x2;
            double dy = y1 - y2;
            if (dx > 0 && dy > 0)
                G1 += Math.Atan2(dx,dy);
            else if (dx > 0 && dy < 0 || dx < 0 && dy < 0)
                G1 += Math.Atan2(dx, dy)+200;
            else if (dx < 0 && dy > 0)
                G1 += Math.Atan2(dx, dy) + 400;

            MessageBox.Show(G1.ToString());
            //Somme angles et de n 
            double ang = 0;
            double di = 0;
            foreach (Cheminement ch in cheminements)
            {
                ang += ch.Ah2;
                di += ch.Distance;
            }

            //Gisement observé d’arrivée 
            double Gobs = 0;
            Gobs = G1 + ang + (n - 1) * 200;

            for (int i = 0; i < n; i++)
            {
                if (Gobs > 400)
                    Gobs -=  400;
                else if (Gobs<0)
                    Gobs += 200;
            }
            //fermeture angulaire
            double fa = 0;
            if (cheminements[0]!=cheminements[n-1]){
                
                fa = G1 - Gobs;
            }

             //Tolérance théorique
            double Ta= 0.0113;

            if (fa > Ta)
            {
                MessageBox.Show("Erreur de calcul (Tolérance théorique) ");
                return null;
            }

            else{
            double da;
                da = fa / n;
                double x = cheminements[0].X;
                double y = cheminements[0].Y;
                for (int i = 0;i<n;i++)
                {
                    
                    cheminements[i].Ah2 = cheminements[i].Ah2 + da;
                    //gisement compense
                    if (i == 0)
                        cheminements[i].Gisement = G1;
                    else
                    {
                        cheminements[i].Gisement = cheminements[i - 1].Gisement + cheminements[i].Ah2 + 200;
                    }
                if (cheminements[i].Gisement > 400)
                    cheminements[i].Gisement -= 400;
                else if (cheminements[i].Gisement < 0)
                        cheminements[i].Gisement += 200;

                    //coord compense
                    if (i == 0)
                    {
                        cheminements[i].X = cheminements[i].X + (cheminements[i].Distance * Math.Sin(cheminements[i].Gisement));
                        cheminements[i].Y = cheminements[i].Y + (cheminements[i].Distance * Math.Cos(cheminements[i].Gisement));
                    }
                    else
                    {
                        cheminements[i].X = cheminements[i-1].X + (cheminements[i].Distance * Math.Sin(cheminements[i].Gisement));
                        cheminements[i].Y = cheminements[i-1].Y + (cheminements[i].Distance * Math.Cos(cheminements[i].Gisement));
                    }
                    // fermeture lineaire
                    double fl;
                    double fx = x-cheminements[0].X;
                    double fy = y - cheminements[0].Y;
                    fl = Math.Sqrt(Math.Pow(fx,2) + Math.Pow(fy,2));
                    //tolerance 
                    double Tl = 0.05 + di / 2000;

                    /*if (fl > Tl)
                    {
                        MessageBox.Show("Erreur de calcul");
                        return null;
                    }*/
                    //else
                    //{
                        double distanceglobal = 0;

                        for (int j = 0; j <= i; j++)
                            distanceglobal += cheminements[j].Distance;

                        double dx1 = (fx * distanceglobal) / di;
                        double dy1 = (fy * distanceglobal) / di;

                        cheminements[i].X += dx1;
                        cheminements[i].Y += dy1;
                    //}
                }

            }
            foreach(Cheminement ch in cheminements)
            {

                ch.Ah2 = double.Parse(ch.Ah2.ToString("0.00"));
                ch.Gisement =   double.Parse(ch.Gisement.ToString("0.00"));
                ch.X =   double.Parse(ch.X.ToString("0.00"));
                ch.Y =   double.Parse(ch.Y.ToString("0.00"));

            }

            return cheminements;
        }

    }
}
