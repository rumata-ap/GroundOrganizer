using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public struct DataR
    {
        public string Bore { get; set; }
        public string Base { get; set; }
        public double R { get; set; }
        public double P { get; set; }
        public double PmaxX { get; set; }
        public double PmaxY { get; set; }
        public double GapX { get; set; }
        public double GapY { get; set; }
        public double CheckP { get; set; }
        public double CheckGap { get; set; }
        public double YIIu { get; set; }
        public double YII { get; set; }
        public double FiII { get; set; }
        public double CII { get; set; }
        public double Yc1 { get; set; }
        public double Yc2 { get; set; }
        public double My { get; set; }
        public double Mq { get; set; }
        public double Mc { get; set; }
        public double IL { get; set; }
        public double Ys { get; set; }
        public double Ke { get; set; }
        public double Kz { get; set; }
        public double K { get; set; }
        public double d1 { get; set; }
        public double db { get; set; }       
        public string Ground { get; set; }     
        public string GroundType { get; set; }
        public List<DataPair> FullData { get; private set; }
        public List<DataPair> MediumData { get; private set; }
        public List<DataPair> SmallData { get; private set; }

        internal void CreateFullData()
        {
            FullData = new List<DataPair>();
            DataPair dataPair = new DataPair() { Описание = "Фундамент", Параметр = Base };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Скважина", Параметр = Bore };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Расчетное сопротивление", Параметр = R, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Среднее давление под подошвой", Параметр = P, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Максимальное давление под подошвой в направлении длины фундамента",
                                        Параметр = PmaxX, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Максимальное давление под подошвой в направлении ширины фундамента",
                                        Параметр = PmaxY, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффициент использования расчетного сопротивления", Параметр = CheckP };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении длины фундамента", Параметр = GapX};
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении ширины фундамента", Параметр = GapY};
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Максимальный отрыв подошвы", Параметр = CheckGap };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Глубина заложения", Параметр = d1, LenghtsUnits = UnitsLin.м };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Глубина подвала", Параметр = db, LenghtsUnits = UnitsLin.м };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Грунт под подошвой", Параметр = Ground };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Объемный вес (осредненный) грунта выше подошвы", Параметр = YIIu,
                                        AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Объемный вес (осредненный) грунта ниже подошвы", Параметр = YII,
                                        AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Угол внутреннего трения грунта основания (осредненный)", Параметр = FiII };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Удельное сцепление грунта основания  (осредненное)", Параметр = CII,
                                        AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Показатель текучести грунта основания ", Параметр = IL };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффициент пористости грунта основания ", Параметр = Ke };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Тип грунта основания согласно таблице 5.4", Параметр = GroundType };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Yc1 согласно таблице 5.4", Параметр = Yc1 };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Yc2 согласно таблице 5.4", Параметр = Yc2 };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт My согласно таблице 5.5", Параметр = My };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Mq согласно таблице 5.5", Параметр = Mq };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Mc согласно таблице 5.5", Параметр = Mc };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Kz", Параметр = Kz };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт K", Параметр = K };
            FullData.Add(dataPair);

        }
        internal void CreateMediumData()
        {
            MediumData = new List<DataPair>();
            DataPair dataPair = new DataPair() { Описание = "Фундамент", Параметр = Base };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Расчетное сопротивление", Параметр = R, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Среднее давление под подошвой", Параметр = P, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении длины фундамента",
                Параметр = PmaxX,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении ширины фундамента",
                Параметр = PmaxY,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффициент использования расчетного сопротивления", Параметр = CheckP };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении длины фундамента", Параметр = GapX };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении ширины фундамента", Параметр = GapY };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Максимальный отрыв подошвы", Параметр = CheckGap };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Глубина заложения", Параметр = d1, LenghtsUnits = UnitsLin.м };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Глубина подвала", Параметр = db, LenghtsUnits = UnitsLin.м };
            MediumData.Add(dataPair);
            //dataPair = new DataPair() { Descriptions = "Грунт под подошвой", DataEntity = Ground };
            //MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Объемный вес (осредненный) грунта выше подошвы",
                Параметр = YIIu,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Объемный вес (осредненный) грунта ниже подошвы",
                Параметр = YII,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Угол внутреннего трения грунта основания (осредненный)", Параметр = FiII };
            MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Удельное сцепление грунта основания  (осредненное)",
                Параметр = CII,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
        }
        internal void CreateSmallData()
        {
           SmallData = new List<DataPair>();
            DataPair dataPair = new DataPair() { Описание = "Фундамент", Параметр = Base };
           SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Расчетное сопротивление", Параметр = R, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
           SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Среднее давление под подошвой", Параметр = P, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
           SmallData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении длины фундамента",
                Параметр = PmaxX,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
           SmallData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении ширины фундамента",
                Параметр = PmaxY,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
           SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении длины фундамента", Параметр = GapX };
           SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении ширины фундамента", Параметр = GapY };
           SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Максимальный отрыв подошвы", Параметр = CheckGap };
           SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффициент использования расчетного сопротивления", Параметр = CheckP };
            SmallData.Add(dataPair);
        }

        internal List<DataPair> FullResults()
        {
            FullData = new List<DataPair>();
            DataPair dataPair = new DataPair() { Описание = "Фундамент", Параметр = Base };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Скважина", Параметр = Bore };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Расчетное сопротивление", Параметр = R, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Среднее давление под подошвой", Параметр = P, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            FullData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении длины фундамента",
                Параметр = PmaxX,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            FullData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении ширины фундамента",
                Параметр = PmaxY,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении длины фундамента", Параметр = GapX };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении ширины фундамента", Параметр = GapY };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Максимальный отрыв подошвы", Параметр = CheckGap };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффициент использования расчетного сопротивления", Параметр = CheckP };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Глубина заложения", Параметр = d1, LenghtsUnits = UnitsLin.м };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Глубина подвала", Параметр = db, LenghtsUnits = UnitsLin.м };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Грунт под подошвой", Параметр = Ground };
            FullData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Объемный вес (осредненный) грунта выше подошвы",
                Параметр = YIIu,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            FullData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Объемный вес (осредненный) грунта ниже подошвы",
                Параметр = YII,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Угол внутреннего трения грунта основания (осредненный)", Параметр = FiII };
            FullData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Удельное сцепление грунта основания  (осредненное)",
                Параметр = CII,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Показатель текучести грунта основания ", Параметр = IL };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффициент пористости грунта основания ", Параметр = Ke };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Тип грунта основания согласно таблице 5.4", Параметр = GroundType };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Yc1 согласно таблице 5.4", Параметр = Yc1 };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Yc2 согласно таблице 5.4", Параметр = Yc2 };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт My согласно таблице 5.5", Параметр = My };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Mq согласно таблице 5.5", Параметр = Mq };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Mc согласно таблице 5.5", Параметр = Mc };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт Kz", Параметр = Kz };
            FullData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффицинт K", Параметр = K };
            FullData.Add(dataPair);

            return FullData;
        }
        internal List<DataPair> MediumResults()
        {
            MediumData = new List<DataPair>();
            DataPair dataPair = new DataPair() { Описание = "Фундамент", Параметр = Base };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Расчетное сопротивление", Параметр = R, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Среднее давление под подошвой", Параметр = P, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении длины фундамента",
                Параметр = PmaxX,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении ширины фундамента",
                Параметр = PmaxY,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении длины фундамента", Параметр = GapX };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении ширины фундамента", Параметр = GapY };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Максимальный отрыв подошвы", Параметр = CheckGap };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффициент использования расчетного сопротивления", Параметр = CheckP };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Глубина заложения", Параметр = d1, LenghtsUnits = UnitsLin.м };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Глубина подвала", Параметр = db, LenghtsUnits = UnitsLin.м };
            MediumData.Add(dataPair);
            //dataPair = new DataPair() { Descriptions = "Грунт под подошвой", DataEntity = Ground };
            //MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Объемный вес (осредненный) грунта выше подошвы",
                Параметр = YIIu,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Объемный вес (осредненный) грунта ниже подошвы",
                Параметр = YII,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Угол внутреннего трения грунта основания (осредненный)", Параметр = FiII };
            MediumData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Удельное сцепление грунта основания  (осредненное)",
                Параметр = CII,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            MediumData.Add(dataPair);

            return MediumData;
        }
        internal List<DataPair> SmallResults()
        {
            SmallData = new List<DataPair>();
            DataPair dataPair = new DataPair() { Описание = "Фундамент", Параметр = Base };
            SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Расчетное сопротивление", Параметр = R, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Среднее давление под подошвой", Параметр = P, AreasUnits = UnitsArea.м, ForcesUnits = UnitsForce.т };
            SmallData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении длины фундамента",
                Параметр = PmaxX,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            SmallData.Add(dataPair);
            dataPair = new DataPair()
            {
                Описание = "Максимальное давление под подошвой в направлении ширины фундамента",
                Параметр = PmaxY,
                AreasUnits = UnitsArea.м,
                ForcesUnits = UnitsForce.т
            };
            SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении длины фундамента", Параметр = GapX };
            SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Отрыв в направлении ширины фундамента", Параметр = GapY };
            SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Максимальный отрыв подошвы", Параметр = CheckGap };
            SmallData.Add(dataPair);
            dataPair = new DataPair() { Описание = "Коэффициент использования расчетного сопротивления", Параметр = CheckP };
            SmallData.Add(dataPair);

            return SmallData;
        }
    }
}
