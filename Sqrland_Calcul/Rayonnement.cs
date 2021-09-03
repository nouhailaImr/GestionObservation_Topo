using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqrland_Calcul
{
    class Rayonnement
    {

        public string Station { get; set; }
        public string Point { get; set; }
        public double Ah2 { get; set; }
        public double Distance { get; set; }
        public double Gisement { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Ref { get; set; }

        public Rayonnement() { }
        public Rayonnement(string station, string point, double ah2, double distance, double gisement, double x, double y, string reef)
        {
            Station = station;
            Point = point;
            Ah2 = ah2;
            Distance = distance;
            Gisement = gisement;
            X = x;
            Y = y;
            Ref = reef;
        }
    }
}
