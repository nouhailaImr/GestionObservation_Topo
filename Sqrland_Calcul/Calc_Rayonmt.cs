using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sqrland_Calcul
{
    class Calc_Rayonmt
    {
        public static List<Rayonnement> Main(List<Rayonnement> rayonnements)
        {
            double x1 = rayonnements[0].X;
            double x2 = rayonnements[1].X;
            double y2 = rayonnements[1].X;
            double y1 = rayonnements[0].X;
            int n = rayonnements.Count;
            double G1 = 0;
            double dx = x1 - x2;
            double dy = y1 - y2;
            if (dx > 0 && dy > 0)
                G1 += Math.Atan2(dx, dy);
            else if (dx > 0 && dy < 0 || dx < 0 && dy < 0)
                G1 += Math.Atan2(dx, dy) + 200;
            else if (dx < 0 && dy > 0)
                G1 += Math.Atan2(dx, dy) + 400;

            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                    rayonnements[i].Gisement = G1;
                else
                {
                    rayonnements[i].Gisement = rayonnements[i - 1].Gisement + rayonnements[i].Ah2 + 200;
                }
                if (rayonnements[i].Gisement > 400)
                    rayonnements[i].Gisement -= 400;
                else if (rayonnements[i].Gisement < 0)
                    rayonnements[i].Gisement += 200;

                if (i == 0)
                {
                    rayonnements[i].X = rayonnements[i].X + (rayonnements[i].Distance * Math.Sin(rayonnements[i].Gisement));
                    rayonnements[i].Y = rayonnements[i].Y + (rayonnements[i].Distance * Math.Cos(rayonnements[i].Gisement));
                }
                else
                {
                    rayonnements[i].X = rayonnements[i - 1].X + (rayonnements[i].Distance * Math.Sin(rayonnements[i].Gisement));
                    rayonnements[i].Y = rayonnements[i - 1].Y + (rayonnements[i].Distance * Math.Cos(rayonnements[i].Gisement));
                }
            }
            foreach (Rayonnement ch in rayonnements)
            {

                ch.Ah2 = double.Parse(ch.Ah2.ToString("0.00"));
                ch.Gisement = double.Parse(ch.Gisement.ToString("0.00"));
                ch.X = double.Parse(ch.X.ToString("0.00"));
                ch.Y = double.Parse(ch.Y.ToString("0.00"));

            }


            return rayonnements;

        }
    }
}
