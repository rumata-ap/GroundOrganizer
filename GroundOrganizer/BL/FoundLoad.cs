using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public class FoundLoad
    {
        public double NC { get; set; }
        public double MxC { get; set; }
        public double MyC { get; set; }
        public double QyC { get; set; }
        public double QxC { get; set; }


        public double N { get; set; }
        public double Mx { get; set; }
        public double My { get; set; }
        public double Qy { get; set; }
        public double Qx { get; set; }
        public double q { get; set; }

        public int Number { get; set; }

        public bool InFoot { get; set; }

        public UnitsForce UnitsF { get; set; }
        public UnitsLin UnitsL { get; set; }

        internal string PropsToString()
        {
            string s = ";";
            return Number + s + N + s + Mx + s + My + s + Qx + s + Qy + s + q;
        }

        internal List<object> PropsToList()
        {
            return new List<object> { Number, N, Mx, My, Qx, Qy, q, InFoot };
        }

        internal void StringToProps(string line, char separator = ';')
        {
            string[] src = line.Split(separator);
            try
            {
                Number = Int32.Parse(src[0]);
                N = double.Parse(src[1]);
                Mx = double.Parse(src[2]);
                My = double.Parse(src[3]);
                Qx = double.Parse(src[4]);
                Qy = double.Parse(src[5]);
                q = double.Parse(src[6]);
            }
            catch
            {
                return;
            }
        }

        internal void ListToProps(List<object> src)
        {
            try
            {
                Number = (int)(double)src[0];
                N = (double)src[1];
                Mx = (double)src[2];
                My = (double)src[3];
                Qx = (double)src[4];
                Qy = (double)src[5];
                q = (double)src[6];
                InFoot = (bool)src[7];
            }
            catch
            {
                return;
            }
        }
    }
}
