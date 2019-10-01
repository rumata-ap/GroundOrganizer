using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public struct DataS
    {
        public string Bore { get; set; }
        public string Base { get; set; }
        public double Sp { get; set; }
        public double Sr { get; set; }
        public double p { get; set; }
        public double Hc { get; set; }
        public double Hmin { get; set; }
        public double YIIu { get; set; }
        public List<double> Z { get; set; }
        public List<double?> Alfa { get; set; }       
        public List<double> sig_zp { get; set; }
        public List<double> sig_zy { get; set; }
        public List<double> sig_zg { get; set; }
        public List<double> E { get; set; }
        public List<double> N { get; set; }
    }
}
