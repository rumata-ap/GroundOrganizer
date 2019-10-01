using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public class Layer
    {
        public int Number { get; set; }

        public string NumIGE { get; set; }
        public IGE IGE { get; set; }
        /// <summary>
        ///Относительная отметка верхней границы слоя относительно устья скважины
        /// </summary>
        public double Up { get; set; }
        /// <summary>
        ///Относительная отметка нижней границы слоя относительно устья скважины
        /// </summary>
        public double Down { get; set; }
        /// <summary>
        ///Мощность слоя
        /// </summary>
        public double H { get; set; }
        /// <summary>
        ///Абсолютная отметка подошвы слоя
        /// </summary>
        public double Z { get; set; }
        /// <summary>
        /// Описание слоя грунта
        /// </summary>
        public string Description { get; set; }

        public Layer Clone()
        {
            return new Layer()
            {
                Number = this.Number,
                NumIGE = this.NumIGE,
                IGE = this.IGE,
                Up = this.Up,
                Down = this.Down,
                H = this.H,
                Z = this.Z,
                Description = this.Description
            };
        }

        internal string PropsToString()
        {
            string s = ";";
            return Number + s + NumIGE + s + Down + s + H + s + Z;
        }

        internal List<object> PropsToList()
        {
            return new List<object> { Number, NumIGE, Down, H, Z };
        }

        internal void StringToProps(string line, char separator = ';')
        {
            string[] src = line.Split(separator);
            try
            {
                Number = Int32.Parse(src[0]);
                NumIGE = src[1];
                Down = double.Parse(src[2]);
                H = double.Parse(src[3]);
                Z = double.Parse(src[4]);
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
                NumIGE = (string)src[1];
                Down = (double)src[2];
                H = (double)src[3];
                Z = (double)src[4];
            }
            catch
            {
                return;
            }
        }
    }
}
