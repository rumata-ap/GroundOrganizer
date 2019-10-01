using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public class Bore
    {
        /// <summary>
        /// Порядковый номер скважины
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Имя скважины
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Х-координата распложения скважины
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y-координата распложения скважины
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Абсолютная отметка устья скважины
        /// </summary>
        public double Z { get; set; }
        /// <summary>
        /// Относительная отметка уровня грунтовых вод
        /// </summary>
        public double? WL { get; set; }
        /// <summary>
        /// Превышение глубины сважины в расчетах осадки
        /// </summary>
        public double DZ { get; set; }
        /// <summary>
        /// Массив грунтовых слоев
        /// </summary>
        public ObservableCollection<Layer> Layers { get; set; }

        public Bore()
        {
            Layers = new ObservableCollection<Layer>();
        }

        public void AddLayer(Layer layer)
        {
            if (Layers == null) Layers = new ObservableCollection<Layer>();
            Layers.Add(layer);
        }
        public void AddLayers(ObservableCollection<Layer> layers)
        {
            Layers = layers;
        }
        public void AddLayers(List<Layer> layers)
        {
            if (Layers == null) Layers = new ObservableCollection<Layer>();
            foreach (var item in layers) Layers.Add(item);
        }

        public void DeleteLayers()
        {
            Layers = new ObservableCollection<Layer>();
        }

        internal string PropsToString()
        {
            string s = ";";
            return Number + s + Name + s + X + s + Y + s + Z + s + WL + s + DZ;
        }

        internal List<object> PropsToList()
        {
            return new List<object> { Number, Name, X, Y, Z, WL, DZ };
        }

        internal void StringToProps(string line, char separator = ';')
        {
            string[] src = line.Split(separator);
            try
            {
                Number = Int32.Parse(src[0]);
                Name = src[1];
                X = double.Parse(src[2]);
                Y = double.Parse(src[3]);
                Z = double.Parse(src[4]);
                WL = double.Parse(src[5]);
                DZ = double.Parse(src[6]);
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
                Name = (string)src[1];
                X = (double)src[2];
                Y = (double)src[3];
                Z = (double)src[4];
                WL = (double)src[5];
                DZ = (double)src[6];
            }
            catch
            {
                return;
            }
        }
    }
}
