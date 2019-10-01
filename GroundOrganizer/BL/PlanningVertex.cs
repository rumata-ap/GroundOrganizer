using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public class PlanningVertex : Geo.Point3d
    {
        public int Number { get; set; }
        public double Red { get; set; }
        public double Black { get; set; }

        internal string PropsToString()
        {
            string s = ";";
            return Number + s + X + s + Y + s + Red + s + Black;
        }

        internal void StringToProps(string line, char separator = ';')
        {
            string[] src = line.Split(separator);
            try
            {
                Number = Int32.Parse(src[0]);
                X = double.Parse(src[1]);
                Y = double.Parse(src[2]);
                Red = double.Parse(src[3]);
                Black = double.Parse(src[4]);
            }
            catch
            {
                return;
            }
        }
    }
}
