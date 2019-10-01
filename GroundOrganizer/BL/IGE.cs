using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public class IGE
    {
        private string groundType;
        public int Number { get; set; }
        /// <summary>
        /// Номер иженерно-геологического элемента
        /// </summary>
        public string NumIGE { get; set; }
        /// <summary>
        /// Тип грунта согласно таблицы 5.4 СП 22.13330.2011
        /// </summary>
        public string GroundType { get => groundType; set { groundType = value; ChangeTypeGroundIdx(); } }
        /// <summary>
        /// Плотность грунта для расчетов по 2-й группе предельных сосотояний
        /// </summary>
        public double RoII { get; set; }
        /// <summary>
        /// Угол внутреннего трения грунта в градусах для расчетов по 2-й группе предельных сосотояний
        /// </summary>
        public double FiII { get; set; }
        /// <summary>
        /// Удельное сцепление для расчетов по 2-й группе предельных сосотояний
        /// </summary>
        public double CII { get; set; }
        /// <summary>
        /// Модуль деформации грунта
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// Удельный вес частиц (минеральной части) грунта
        /// </summary>
        public double Ys { get; set; }
        /// <summary>
        /// Коэффициент пористости
        /// </summary>
        public double Ke { get; set; }
        /// <summary>
        ///Показатель текучести
        /// </summary>
        public double IL { get; set; }
        /// <summary>
        /// Наличие водонасыщения
        /// </summary>
        public bool W { get; set; }
        /// <summary>
        /// Описание слоя грунта
        /// </summary>
        public string Description { get; set; }

        public int GroundTypeIdx { get; private set; }

        /// <summary>
        /// Типы грунтов согласно таблицы 5.4 СП 22.13330.2011
        /// </summary>
        private readonly string[] groundTypes  = new string[]
        {
            "Крупнообломочные с песчаным заполнителем и пески кроме мелких и пылеватых",
            "Пески мелкие",
            "Пески пылеватые маловлажные",
            "Пески пылеватые влажные насыщенные водой",
            "Пески рыхлые",
            "Глинистые, а также крупнообломочные с глинистым заполнителем при IL<=0.25",
            "Глинистые, а также крупнообломочные с глинистым заполнителем при 0.25<IL<=0.5",
            "Глинистые, а также крупнообломочные с глинистым заполнителем при 0.5<IL"
        };

        public IGE Clone()
        {
            return new IGE()
            {
                Number=Number,
                NumIGE=NumIGE,
                GroundType = GroundType,
                RoII = RoII,
                FiII = FiII,
                CII = CII,
                E = E,
                Ys = Ys,
                Ke = Ke,
                IL = IL,
                W = W,
                Description = Description
            };
        }

        void ChangeTypeGroundIdx()
        {
            int i = 1;
            foreach (string item in groundTypes)
            {
                if (item == groundType)
                {
                    GroundTypeIdx = i;
                    break;
                }
                i++;
            }
        }

        internal string PropsToString()
        {
            string s = ";";
            return Number.ToString() + s + NumIGE + s + Description + s + RoII.ToString() + s + FiII.ToString() + s + CII.ToString() + s + E.ToString() +
                s + Ys.ToString() + s + Ke.ToString() + s + IL.ToString() + s + W.ToString() + s + GroundType + s + GroundTypeIdx.ToString();
        }

        internal List<object> PropsToList()
        {
            return new List<object> { Number, NumIGE, Description, RoII, FiII, CII, E, Ys, Ke, IL, W, GroundType, GroundTypeIdx };
        }

        internal void StringToProps(string line, char separator = ';')
        {
            string[] src = line.Split(separator);
            try
            {
                Number = Int32.Parse(src[0]);
                NumIGE = src[1];
                Description = src[2];
                RoII = double.Parse(src[3]);
                FiII = double.Parse(src[4]);
                CII = double.Parse(src[5]);
                E = double.Parse(src[6]);
                Ys = double.Parse(src[7]);
                Ke = double.Parse(src[8]);
                IL = double.Parse(src[9]);
                W = bool.Parse(src[10]);
                GroundType = src[11];
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
                Description = (string)src[2];
                RoII = (double)src[3];
                FiII = (double)src[4];
                CII = (double)src[5];
                E = (double)src[6];
                Ys = (double)src[7];
                Ke = (double)src[8];
                IL = (double)src[9];
                W = (bool)src[10];
                GroundType = (string)src[11];
            }
            catch
            {
                return;
            }
        }
    }
}
