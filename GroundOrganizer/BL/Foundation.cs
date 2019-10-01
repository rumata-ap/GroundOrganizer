using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Tools;

namespace GroundOrganizer
{
    [Serializable]
    public class Foundation
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public TypeFound Type { get; set; }
        public bool Basement { get; set; }
        public double B { get; set; }
        public double L { get; set; }
        public double Db { get; set; }
        public double D1 { get; set; }
        public double H { get; set; }
        public double AlfaDeg { get; set; }
        public double Hs { get; set; }
        public double Hcf { get; set; }
        public double Ycf { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        /// <summary>
        /// Относительная отметка подошвы фундамента
        /// </summary>
        public double FL { get; set; }
        /// <summary>
        /// Абсолютная отметка планировки в центральной точке фундамента
        /// </summary>
        public double DL { get; set; }
        /// <summary>
        /// Абсолютная отметка естественного рельефа в центральной точке фундамента
        /// </summary>
        public double NL { get; set; }
        public ObservableCollection<FoundLoad> Loads { get; set; }
        public UnitsForce UnitsF { get; set; }
        public UnitsLin UnitsL { get; set; }
        public Quadrangle Contour { get; set; }

        public List<DataPair> FullResults { get; private set; }
        public List<DataPairProps> FullProps { get; private set; }
        public List<DataPair> MediumResults { get; private set; }
        public List<DataPairProps> MediumProps { get; private set; }
        public List<DataPair> SmallResults{ get; private set; }
        public List<DataPairProps> SmallProps{ get; private set; }

        public Foundation()
        {
            Loads = new ObservableCollection<FoundLoad>();
            Type = TypeFound.Прямоугольный;
            UnitsF = UnitsForce.т;
            UnitsL = UnitsLin.м;
            Ycf = 2.2;
            FullResults = new List<DataPair>();
            MediumResults = new List<DataPair>();
            SmallResults = new List<DataPair>();
        }

        public Foundation(Quadrangle cntr)
        {           
            if (cntr.Units == UnitsLin.см)
            {
                Contour.Vertex1 = cntr.Vertex1 * 0.01;
                Contour.Vertex2 = cntr.Vertex2 * 0.01;
                Contour.Vertex3 = cntr.Vertex3 * 0.01;
                Contour.Vertex4 = cntr.Vertex4 * 0.01;
            }
            if (cntr.Units == UnitsLin.мм)
            {
                Contour.Vertex1 = cntr.Vertex1 * 0.001;
                Contour.Vertex2 = cntr.Vertex2 * 0.001;
                Contour.Vertex3 = cntr.Vertex3 * 0.001;
                Contour.Vertex4 = cntr.Vertex4 * 0.001;
            }
            if(cntr.Units == UnitsLin.м) Contour = cntr;

            X = Math.Round(Contour.Centroid.X, 3);
            Y = Math.Round(Contour.Centroid.Y, 3);

            IOrderedEnumerable<Line2d> selected = from l in Contour.Segments // определяем каждый объект как
                                                  orderby l.Length  // упорядочиваем по возрастанию
                                                  select l; // выбираем объект

            AlfaDeg = Math.Round(RadToDeg(Math.Acos(selected.First().Directive.Vx / selected.First().Length)), 3);
            B = Math.Round(selected.First().Length, 3);
            L = Math.Round(selected.Last().Length, 3);

            Loads = new ObservableCollection<FoundLoad>();
            Type = TypeFound.Прямоугольный;
            UnitsF = UnitsForce.т;
            UnitsL = UnitsLin.м;
            Ycf = 2.2;
            FullResults = new List<DataPair>();
            MediumResults = new List<DataPair>();
            SmallResults = new List<DataPair>();
        }

        internal void CreateFullPropsList()
        {
            FullProps = new List<DataPairProps>();
            DataPairProps dataPair = new DataPairProps() { Описание = "Номер", Параметр = Number };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Марка", Параметр = Name };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Ширина", Параметр = B, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Длина", Параметр = L, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Высота", Параметр = H, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка подошвы", Параметр = FL, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Глубина заложения", Параметр = D1, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка планировки", Параметр = DL };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка рельефа", Параметр = NL };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Наличие подвала", Параметр = Basement };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Глубина подвала", Параметр = Db, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "h_s", Параметр = Hs, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "h_cf", Параметр = Hcf, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Ycf, т/м3", Параметр = Ycf };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Х", Параметр = X, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Y", Параметр = Y, Ед_изм = UnitsList.м };
            FullProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Поворот", Параметр = AlfaDeg, Ед_изм= UnitsList.градус };
            FullProps.Add(dataPair);

        }

        internal void CreateMediumPropsList()
        {
            MediumProps = new List<DataPairProps>();
            DataPairProps dataPair = new DataPairProps() { Описание = "Номер", Параметр = Number };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Марка", Параметр = Name };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Ширина", Параметр = B, Ед_изм = UnitsList.м };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Длина", Параметр = L, Ед_изм = UnitsList.м };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Высота", Параметр = H, Ед_изм = UnitsList.м };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка подошвы", Параметр = FL, Ед_изм = UnitsList.м };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Глубина заложения", Параметр = D1, Ед_изм = UnitsList.м };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка планировки", Параметр = DL };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка рельефа", Параметр = NL };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Наличие подвала", Параметр = Basement };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Глубина подвала", Параметр = Db, Ед_изм = UnitsList.м };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "h_s", Параметр = Hs, Ед_изм = UnitsList.м };
            MediumProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "h_cf", Параметр = Hcf, Ед_изм = UnitsList.м };
            MediumProps.Add(dataPair);
        }

        internal void CreateSmallPropsList()
        {
            SmallProps = new List<DataPairProps>();
            DataPairProps dataPair = new DataPairProps() { Описание = "Номер", Параметр = Number };
            SmallProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Марка", Параметр = Name };
            SmallProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Ширина b", Параметр = B, Ед_изм = UnitsList.м };
            SmallProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Длина l", Параметр = L, Ед_изм = UnitsList.м };
            SmallProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Высота h", Параметр = H, Ед_изм = UnitsList.м };
            SmallProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка подошвы FL", Параметр = FL, Ед_изм = UnitsList.м };
            SmallProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Глубина заложения d_1", Параметр = D1, Ед_изм = UnitsList.м };
            SmallProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка планировки DL", Параметр = DL, Ед_изм = UnitsList.м };
            SmallProps.Add(dataPair);
            dataPair = new DataPairProps() { Описание = "Отметка рельефа NL", Параметр = NL, Ед_изм = UnitsList.м };
            SmallProps.Add(dataPair);
        }

        public DataS Sp(Bore bore, FoundLoad load, double kHc = 0.5, PointFound ptFond = PointFound.Центр)
        {
            DataS res = new DataS();
            List<Layer> downFS = DownFS(bore);
            List<double> n = res.N;
            n = new List<double>();

            foreach (Layer item in downFS)
            {
                n.Add(Math.Ceiling(item.H / (0.4 * B)));
            }

            List<Layer> layers = new List<Layer>();
            for (int i = 0; i < n.Count; i++)
            {
                double z = downFS[i].Up;
                double dlt = downFS[i].H / n[i];
                while (downFS[i].Down > z)
                {
                    z += dlt;
                    Layer lay = downFS[i].Clone();
                    lay.Down = z;
                    lay.H = dlt;
                    lay.Up = z - dlt;
                    layers.Add(lay);
                }
            }

            double FL;
            if (Basement) FL = Hs + Hcf + Db;
            else FL = D1;

            res.Z = new List<double>() { 0 };
            foreach (Layer item in layers)
            {
                res.Z.Add(item.Up + 0.5 * item.H - FL);
            }

            res.Alfa = TablesInterolator.Table_5_8(res.Z, B, L, Type);

            List<Layer> upF = UpF(bore);
            double roh = 0;
            double h = 0;
            if (upF.Count > 0)
            {
                foreach (Layer item in upF)
                {
                    roh += item.H * item.IGE.RoII;
                    h += item.H;
                }
            }
            res.YIIu = Math.Round(roh / h, 3);

            double sig_zg0 = res.YIIu * FL;
            double p = load.N / (B * L) + load.q + 2 * FL;

            res.sig_zy = new List<double>();
            res.sig_zp = new List<double>();
            for (int i = 0; i < res.Z.Count; i++)
            {
                res.sig_zp.Add((double)res.Alfa[i] * p);
                res.sig_zy.Add((double)res.Alfa[i] * sig_zg0);
            }

            res.sig_zg = new List<double> { sig_zg0 };
            res.E = new List<double>();
            for (int i = 0; i < layers.Count; i++)
            {
                res.sig_zg.Add(sig_zg0 + layers[i].IGE.RoII * (res.Z[i + 1] - (layers[i].Up - FL)));
                sig_zg0 += layers[i].H * layers[i].IGE.RoII;
                res.E.Add(layers[i].IGE.E * 101.972);
            }

            sig_zg0 = res.YIIu * FL;

            if (B <= 10) res.Hmin = B / 2;
            else if (B > 10 && B <= 60) res.Hmin = 4 + 0.1 * B;
            else res.Hmin = 10;

            int j = 0;
            for (int i = 0; i < layers.Count; i++)
            {
                if (res.sig_zp[i] >= kHc * res.sig_zg[i])
                {
                    res.Hc = res.Z[i];
                }
                else
                {
                    j = i;
                    break;
                }
            }

            int t = 0; double pt = layers[t].H;
            while (pt <= res.Hmin)
            {
                t++;
                pt += layers[t].H;
            }

            if (res.Hmin > res.Hc)
            {
                res.Hc = res.Hmin;
                j = t;
            }

            if (sig_zg0 >= p)
            {
                for (int i = 0; i <= j; i++)
                {
                    res.Sp += res.sig_zp[i + 1] * layers[i].H / (5 * res.E[i]);
                }
                res.Sp *= 0.8;
            }
            else
            {
                for (int i = 0; i <= j; i++)
                {
                    res.Sp += (res.sig_zp[i + 1] - res.sig_zy[i + 1]) * layers[i].H / res.E[i] + res.sig_zy[i + 1] * layers[i].H / (5 * res.E[i]);
                }
                res.Sp *= 0.8;
            }

            res.Sp = Math.Round(res.Sp * 100, 1);
            res.p = Math.Round(p, 2);

            return res;
        }
        public DataR P(Bore bore, TypeFlexStructure ts, double ls, double hs, double k = 1)
        {
            DataR dataR = R(bore, ts, ls, hs, k);
            dataR.Base = Name;
            dataR.Bore = "Скв." + bore.Name;

            if (Loads == null || Loads.Count == 0)
            {
                FullResults = dataR.FullResults();
                MediumResults = dataR.MediumResults();
                SmallResults = dataR.SmallResults();
                return dataR;
            }
            List<double> P = new List<double>(Loads.Count);
            List<double> PmaxX = new List<double>(Loads.Count);
            List<double> PmaxY = new List<double>(Loads.Count);
            List<double> chP = new List<double>(Loads.Count);
            List<double> GapX = new List<double>(Loads.Count);
            List<double> GapY = new List<double>(Loads.Count);
            List<double> chGap = new List<double>(Loads.Count);
            foreach (FoundLoad item in Loads)
            {
                P.Add(item.N / (B * L) + item.q + 2 * D1);
                double ex = (Math.Abs(item.Mx) / (item.N + 2 * D1 * L * B)) / L;
                double wx = (B * L * L * L) / 6;
                double pmaxx;
                if (ex <= (1.0 / 6)) {pmaxx= item.N / (B * L) + 2 * D1 + Math.Abs(item.Mx) / wx; PmaxX.Add(pmaxx); GapX.Add(0); ex = 0; }
                else
                {
                    double C0x = 0.5 * L - item.Mx / (item.N  + 2 * D1 * L * B);
                    pmaxx = 2 * (item.N + 2 * D1 * L * B) / (3 * B * C0x);
                    PmaxX.Add(pmaxx);
                    GapX.Add(ex);
                }
                double ey = (Math.Abs(item.My) / (item.N + 2 * D1 * L * B)) / B;
                double wy = (L * B * B * B) / 6;
                double pmaxy;
                if (ey <= (1.0 / 6)) {pmaxy= item.N / (B * L) + 2 * D1 + Math.Abs(item.My) / wy; PmaxY.Add(pmaxy); GapY.Add(0); ey = 0; }
                else
                {
                    double C0y = 0.5 * B - item.My / (item.N + 2 * D1 * L * B);
                    pmaxy = 2 * (item.N + 2 * D1 * L * B) / (3 * L * C0y);
                    PmaxY.Add(pmaxy);
                    GapY.Add(ey);
                }
                chGap.Add(Math.Max(ex, ey));
                List<double> chp = new List<double> { (item.N / (B * L) + item.q + 2 * D1) / dataR.R, pmaxx / (1.2 * dataR.R), pmaxy / (1.2 * dataR.R) };
                chP.Add(chp.Max());
            }
            dataR.P = Math.Round(P.Max(), 3);
            dataR.PmaxX = Math.Round(PmaxX.Max(), 3);
            dataR.PmaxY = Math.Round(PmaxY.Max(), 3);
            dataR.CheckP = Math.Round(chP.Max(), 3);
            dataR.CheckGap = Math.Round(chGap.Max(), 3);
            dataR.GapX = Math.Round(GapX.Max(), 3);
            dataR.GapY = Math.Round(GapY.Max(), 3);

            FullResults = dataR.FullResults();
            MediumResults = dataR.MediumResults();
            SmallResults = dataR.SmallResults();

            return dataR;
        }
        public ObservableCollection<DataR> P(ObservableCollection<Bore> bores, TypeFlexStructure ts, double ls, double hs, double k = 1)
        {
            ObservableCollection<DataR> res = new ObservableCollection<DataR>();
            foreach (Bore bore in bores)
            {               
                DataR dataR = R(bore, ts, ls, hs, k);
                dataR.Base = Name;
                dataR.Bore = "Скв." + bore.Name;

                if (Loads == null || Loads.Count == 0)
                {                   
                    dataR.CreateFullData();
                    dataR.CreateMediumData();
                    dataR.CreateSmallData();
                    res.Add(dataR);
                    continue;
                }
                List<double> P = new List<double>(Loads.Count);
                List<double> PmaxX = new List<double>(Loads.Count);
                List<double> PmaxY = new List<double>(Loads.Count);
                List<double> chP = new List<double>(Loads.Count);
                List<double> GapX = new List<double>(Loads.Count);
                List<double> GapY = new List<double>(Loads.Count);
                List<double> chGap = new List<double>(Loads.Count);
                foreach (FoundLoad item in Loads)
                {
                    P.Add(item.N / (B * L) + item.q + 2 * D1);
                    double ex = (item.Mx / (item.N + 2 * D1 * L * B)) / L;
                    double wx = (B * L * L * L) / 6;
                    double pmaxx;
                    if (ex <= 1 / 6) { pmaxx = item.N / (B * L) + 2 * D1 + Math.Abs(item.Mx) / wx; PmaxX.Add(pmaxx); GapX.Add(0); ex = 0; }
                    else
                    {
                        double C0x = 0.5 * L - item.Mx / (item.N + 2 * D1 * L * B);
                        pmaxx = 2 * (item.N + 2 * D1 * L * B) / (3 * B * C0x);
                        PmaxX.Add(pmaxx);
                        GapX.Add(ex);
                    }
                    double ey = (item.My / (item.N + 2 * D1 * L * B)) / B;
                    double wy = (L * B * B * B) / 6;
                    double pmaxy;
                    if (ey <= 1 / 6) { pmaxy = item.N / (B * L) + 2 * D1 + Math.Abs(item.My) / wy; PmaxY.Add(pmaxy); GapY.Add(0); ey = 0; }
                    else
                    {
                        double C0y = 0.5 * B - item.My / (item.N + 2 * D1 * L * B);
                        pmaxy = 2 * (item.N + 2 * D1 * L * B) / (3 * L * C0y);
                        PmaxY.Add(pmaxy);
                        GapY.Add(ey);
                    }
                    chGap.Add(Math.Max(ex, ey));
                    List<double> chp = new List<double> { (item.N / (B * L) + item.q + 2 * D1) / dataR.R, pmaxx / (1.2 * dataR.R), pmaxy / (1.2 * dataR.R) };
                    chP.Add(chp.Max());
                }
                dataR.P = Math.Round(P.Max(), 3);
                dataR.PmaxX = Math.Round(PmaxX.Max(), 3);
                dataR.PmaxY = Math.Round(PmaxY.Max(), 3);
                dataR.CheckP = Math.Round(chP.Max(), 3);
                dataR.CheckGap = Math.Round(chGap.Max(), 3);
                dataR.GapX = Math.Round(GapX.Max(), 3);
                dataR.GapY = Math.Round(GapY.Max(), 3);

                dataR.CreateFullData();
                dataR.CreateMediumData();
                dataR.CreateSmallData();

                res.Add(dataR);
            }

            return res;
        }
        private DataR R(Bore bore, TypeFlexStructure ts, double ls, double hs, double k = 1)
        {
            DataR res = new DataR();
            List<Layer> upF = UpF(bore);
            List<Layer> downF = DownF(bore);
            double roh = 0;
            double h = 0;
            if (upF.Count > 0)
            {
                foreach (Layer item in upF)
                {
                    roh += item.H * item.IGE.RoII;
                    h += item.H;
                }
            }
            res.YIIu = Math.Round(roh / h, 3);

            double fih = 0;
            double ch = 0;
            roh = 0;
            h = 0;
            if (downF.Count > 0)
            {
                foreach (Layer item in downF)
                {
                    fih += item.H * item.IGE.FiII;
                    ch += item.H * item.IGE.CII * 100;
                    roh += item.H * item.IGE.RoII;
                    h += item.H;
                }
            }
            res.YII = Math.Round(roh / h, 3);
            res.FiII = Math.Round(fih / h);
            res.CII = Math.Round(ch / h, 3);
            res.IL = downF[0].IGE.IL;
            res.Ke = downF[0].IGE.Ke;
            res.Ys = downF[0].IGE.Ys;
            res.Ground = "ИГЭ " + downF[0].IGE.NumIGE + " " + downF[0].IGE.Description;
            res.GroundType = downF[0].IGE.GroundType;
            res.Yc1 = TablesInterolator.Yc1(res.GroundType);
            res.Yc2 = TablesInterolator.Yc2(res.GroundType, ts, ls, hs);
            double[] MyMqMc = TablesInterolator.My_Mq_Mc(res.FiII);
            res.My = MyMqMc[0];
            res.Mq = MyMqMc[1];
            res.Mc = MyMqMc[2];
            res.K = k;

            if (B < 10) res.Kz = 1;
            else res.Kz = 8 / B + 0.2;

            if (Basement) res.d1 = Math.Round(Hs + Hcf * (Ycf / res.YIIu), 3);
            else res.d1 = D1;

            if (Basement && Db > 2) res.db = 2;
            else if (Basement && Db <= 2) res.db = Db;
            else res.db = 0;

            res.R = res.Yc1 * res.Yc2 * (res.My * res.Kz * B * res.YII + res.Mq * res.d1 * res.YIIu + (res.Mq - 1) * res.db * res.YIIu + res.Mc * res.CII) / k;
            res.R = Math.Round(res.R, 2);
            //res.GroundType = "";

            return res;
        }
        private List<Layer> UpF(Bore bore)
        {
            List<Layer> res = new List<Layer>();
            double d;
            if (Basement) d = Hs + Hcf + Db;
            else d = D1;

            double FL;
            FL = d;

            Bore boreW = BoreW(bore);
            if (boreW.Layers.Count == 0) return res;
            foreach (Layer item in boreW.Layers)
            {
                if (item.Up > FL) break;
                res.Add(item.Clone());
            }
            res[res.Count - 1].Down = FL;
            res[res.Count - 1].H = res[res.Count - 1].Down - res[res.Count - 1].Up;

            return res;
        }
        private List<Layer> DownF(Bore bore)
        {
            List<Layer> res = new List<Layer>();
            double d, z;
            if (Basement) d = Hs + Hcf + Db;
            else d = D1;

            if (B < 10) z = 0.5 * B;
            else z = 4 + 0.1 * B;

            double FL, ZL;
            FL = d;
            ZL = d + z;

            Bore boreW = BoreW(bore);
            if (boreW.Layers.Count == 0) return res;
            foreach (Layer item in boreW.Layers)
            {
                if (item.Down > FL) res.Add(item.Clone());
                if (item.Down >= ZL) break;
            }
            res[0].Up = FL;
            res[0].H = res[0].Down - res[0].Up;
            res[res.Count - 1].Down = ZL;
            res[res.Count - 1].H = res[res.Count - 1].Down - res[res.Count - 1].Up;
            return res;
        }
        private List<Layer> UpW(Bore bore)
        {
            List<Layer> res = new List<Layer>();
            foreach (Layer item in bore.Layers)
            {
                if (item.Up > bore.WL) break;
                res.Add(item.Clone());
            }
            if (res.Count > 0)
            {
                res[res.Count - 1].Down = (double)bore.WL;
                res[res.Count - 1].H = res[res.Count - 1].Down - res[res.Count - 1].Up;
            }
            return res;
        }
        private List<Layer> DownW(Bore bore)
        {
            List<Layer> layers = new List<Layer>(bore.Layers.Reverse());
            List<Layer> res = new List<Layer>();
            foreach (Layer item in layers)
            {
                if (item.Down <= bore.WL) break;
                if (item.IGE.W) item.IGE.RoII = (item.IGE.Ys - 1) / (1 + item.IGE.Ke);
                res.Add(item.Clone());
            }
            if (res.Count > 0)
            {
                res.Reverse();
                res[0].Up = (double)bore.WL;
                res[0].H = res[0].Down - res[0].Up;
            }
            return res;
        }
        private List<Layer> DownFS(Bore bore)
        {
            List<Layer> res = new List<Layer>();
            double FL;

            if (Basement) FL = Hs + Hcf + Db;
            else FL = D1;

            Bore boreW = BoreW(bore);
            if (boreW.Layers.Count == 0) return res;

            foreach (Layer item in boreW.Layers.Reverse())
            {
                if (item.Down < FL) break;
                res.Add(item.Clone());
            }

            if (res.Count > 0) res.Reverse();
            res[0].Up = FL;
            res[0].H = res[0].Down - res[0].Up;

            return res;
        }
        private Bore BoreW(Bore bore)
        {
            Bore res = new Bore
            {
                Name = bore.Name,
                Number = bore.Number,
                WL = bore.WL,
                X = bore.X,
                Y = bore.Y,
                Z = bore.Z
            };
            List<Layer> downW = DownW(bore);
            List<Layer> layers = new List<Layer>(UpW(bore));
            if (downW.Count > 0) layers.AddRange(downW);
            if (layers.Count > 0)
            {
                res.Layers = new ObservableCollection<Layer>(layers);
                res.Layers[res.Layers.Count - 1].Down = res.Layers[res.Layers.Count - 1].Down + bore.DZ;
                res.Layers[res.Layers.Count - 1].H = res.Layers[res.Layers.Count - 1].H + bore.DZ;
                res.Layers[res.Layers.Count - 1].Z = res.Layers[res.Layers.Count - 1].Z - bore.DZ;
                return res;
            }
            else
            {
                bore.Layers[bore.Layers.Count - 1].Down = bore.Layers[bore.Layers.Count - 1].Down + bore.DZ;
                bore.Layers[bore.Layers.Count - 1].H = bore.Layers[bore.Layers.Count - 1].H + bore.DZ;
                bore.Layers[bore.Layers.Count - 1].Z = bore.Layers[bore.Layers.Count - 1].Z - bore.DZ;
                return bore;
            }
        }

        internal void CalcDL(Structure str)
        {
            if (str == null || str.RedPlanning == null || str.RedPlanning.Count == 0) return;

            List<Vertex> vrtxs = new List<Vertex>();
            Vertex vrtx;
            int i = 1;
            foreach (PlanningVertex item in str.RedPlanning)
            {
                vrtx = new Vertex(item.X, item.Y, item.Number, 2);
                vrtx.Attributes[0] = item.Red;
                vrtx.Attributes[1] = item.Black;
                vrtxs.Add(vrtx);
                i++;
            }

            Contour cnt = new Contour(vrtxs);
            TriangleNet.Geometry.Polygon polygon = new TriangleNet.Geometry.Polygon();
            polygon.Add(cnt);
            GenericMesher mesher = new GenericMesher();
            ConstraintOptions constraint = new ConstraintOptions();
            constraint.Convex = true;
            Mesh meshPlanning = (Mesh)mesher.Triangulate(polygon, constraint);
            TriangleQuadTree meshTree = new TriangleQuadTree(meshPlanning);
            TriangleNet.Topology.Triangle trgl = (TriangleNet.Topology.Triangle)meshTree.Query(X, Y);
            if (trgl == null)
            {
                Dictionary<Vertex, double> valuePairs = new Dictionary<Vertex, double>();
                Line2d ln;
                foreach (Vertex item in meshPlanning.Vertices)
                {
                    ln = new Line2d(new Point2d(X, Y), new Point2d(item.X, item.Y));
                    valuePairs.Add(item, ln.Length);
                }
                IOrderedEnumerable<KeyValuePair<Vertex, double>> selected = from v in valuePairs // определяем каждый объект как
                                                                            orderby v.Value // упорядочиваем по возрастанию
                                                                            select v; // выбираем объект
                List<KeyValuePair<Vertex, double>> lst = selected.ToList(); 
                foreach (TriangleNet.Topology.Triangle item in meshPlanning.Triangles)
                {
                    if (item.Contains(lst[0].Key) && item.Contains(lst[1].Key)) { trgl = item; break; }
                }
            }
            vrtx = new Vertex(X, Y, Number, 2);
            Interpolation.InterpolateAttributes(vrtx, trgl, 1);
            DL = Math.Round(vrtx.Attributes[0], 3);
        }

        internal void CalcDL(Mesh planningMesh)
        {
            if (planningMesh == null) return;

            Vertex vrtx;
            TriangleQuadTree meshTree = new TriangleQuadTree(planningMesh);
            TriangleNet.Topology.Triangle trgl = (TriangleNet.Topology.Triangle)meshTree.Query(X, Y);
            if (trgl == null)
            {
                Dictionary<Vertex, double> valuePairs = new Dictionary<Vertex, double>();
                Line2d ln;
                foreach (Vertex item in planningMesh.Vertices)
                {
                    ln = new Line2d(new Point2d(X, Y), new Point2d(item.X, item.Y));
                    valuePairs.Add(item, ln.Length);
                }
                IOrderedEnumerable<KeyValuePair<Vertex, double>> selected = from v in valuePairs // определяем каждый объект как
                                                                            orderby v.Value // упорядочиваем по возрастанию
                                                                            select v; // выбираем объект
                List<KeyValuePair<Vertex, double>> lst = selected.ToList();
                foreach (TriangleNet.Topology.Triangle item in planningMesh.Triangles)
                {
                    if (item.Contains(lst[0].Key) && item.Contains(lst[1].Key)) { trgl = item; break; }
                }
            }
            vrtx = new Vertex(X, Y, Number, 2);
            Interpolation.InterpolateAttributes(vrtx, trgl, 1);
            DL = Math.Round(vrtx.Attributes[0], 3);
        }

        internal void CalcNL(Mesh blackMesh)
        {
            if (blackMesh == null) return;

            Vertex vrtx;
            TriangleQuadTree meshTree = new TriangleQuadTree(blackMesh);
            TriangleNet.Topology.Triangle trgl = (TriangleNet.Topology.Triangle)meshTree.Query(X, Y);
            if (trgl == null)
            {
                Dictionary<Vertex, double> valuePairs = new Dictionary<Vertex, double>();
                Line2d ln;
                foreach (Vertex item in blackMesh.Vertices)
                {
                    ln = new Line2d(new Point2d(X, Y), new Point2d(item.X, item.Y));
                    valuePairs.Add(item, ln.Length);
                }
                IOrderedEnumerable<KeyValuePair<Vertex, double>> selected = from v in valuePairs // определяем каждый объект как
                                                                            orderby v.Value // упорядочиваем по возрастанию
                                                                            select v; // выбираем объект
                List<KeyValuePair<Vertex, double>> lst = selected.ToList();
                foreach (TriangleNet.Topology.Triangle item in blackMesh.Triangles)
                {
                    if (item.Contains(lst[0].Key) && item.Contains(lst[1].Key)) { trgl = item; break; }
                }
            }
            vrtx = new Vertex(X, Y, Number, 2);
            Interpolation.InterpolateAttributes(vrtx, trgl, 1);
            NL = Math.Round(vrtx.Attributes[0], 3);
        }

        internal void DLtoD1(double nullLevel)
        {
            double dlt = nullLevel - DL;
            D1 = Math.Round(-FL - dlt, 3);
        }

        internal void D1toDL(double nullLevel)
        {
            DL = Math.Round(nullLevel + FL + D1, 3);
        }

        internal void D1toFL(double nullLevel)
        {
            double dlt = nullLevel - DL;
            FL = Math.Round(-dlt - D1, 3);
        }

        internal void FLtoD1(double nullLevel)
        {
            double dlt = nullLevel + FL;
            D1 = Math.Round(DL - dlt, 3);
        }

        internal string PropsToString()
        {
            string s = ";";
            return Number + s + Name + s + (int)Type + s + Basement + s + B + s + L + s + H + s + Db + s + D1 + s + Hs + s + Hcf + s + Ycf + s + X + s + Y + s + FL 
                + s + DL + s + NL + s + AlfaDeg;
        }

        internal List<object> PropsToList()
        {
            return new List<object> { Number, Name, (int)Type, Basement, B, L, H, Db, D1, Hs, Hcf, Ycf, X, Y, FL, DL, NL, AlfaDeg };
        }

        internal void StringToProps(string line, char separator = ';')
        {
            string[] src = line.Split(separator);
            try
            {
                Number = Int32.Parse(src[0]);
                Name = src[1];
                Type = (TypeFound)Int32.Parse(src[2]);
                Basement = bool.Parse(src[3]);
                B = double.Parse(src[4]);
                L = double.Parse(src[5]);
                H = double.Parse(src[6]);
                Db = double.Parse(src[7]);
                D1 = double.Parse(src[8]);
                Hs = double.Parse(src[9]);
                Hcf = double.Parse(src[10]);
                Ycf = double.Parse(src[11]);
                X = double.Parse(src[12]);
                Y = double.Parse(src[13]);
                FL = double.Parse(src[14]);
                DL = double.Parse(src[15]);
                NL = double.Parse(src[16]);
                AlfaDeg = double.Parse(src[17]);
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
                Type = (TypeFound)(int)(double)src[2];
                Basement = (bool)src[3];
                B = (double)src[4];
                L = (double)src[5];
                H = (double)src[6];
                Db = (double)src[7];
                D1 = (double)src[8];
                Hs = (double)src[9];
                Hcf = (double)src[10];
                Ycf = (double)src[11];
                X = (double)src[12];
                Y = (double)src[13];
                FL = (double)src[14];
                DL = (double)src[15];
                NL = (double)src[16];
                AlfaDeg = (double)src[17];
            }
            catch
            {
                return;
            }
        }

        internal void CalcLevels(Mesh planningMesh, Mesh blackMesh)
        {
            CalcDL(planningMesh);
            CalcNL(blackMesh);
        }

        public void CopyProps(IEnumerable<Foundation> founds)
        {
            foreach (Foundation item in founds)
            {
                if (this.Equals(item)) continue;
                item.Name = Name;
                item.B = B;
                item.L = L;
                item.H = H;
                item.FL = FL;
                item.DL = DL;
                item.D1 = D1;
                item.Loads = Loads;
            }
        }

        internal void CalcContour()
        {
            if (Type == TypeFound.Круглый) return;
            Point2d v1 = new Point2d(-0.5 * B, -0.5 * L);
            Point2d v2 = new Point2d(-0.5 * B, 0.5 * L);
            Point2d v3 = new Point2d(0.5 * B, 0.5 * L);
            Point2d v4 = new Point2d(0.5 * B, -0.5 * L);
            double alfa = DegToRad(AlfaDeg);
            double x = v1.X; double y = v1.Y;
            v1.X = x * Math.Cos(alfa) - y * Math.Sin(alfa) + X;
            v1.Y = x * Math.Sin(alfa) + y * Math.Cos(alfa) + Y;
            x = v2.X; y = v2.Y;
            v2.X = x * Math.Cos(alfa) - y * Math.Sin(alfa) + X;
            v2.Y = x * Math.Sin(alfa) + y * Math.Cos(alfa) + Y;
            x = v3.X; y = v3.Y;
            v3.X = x * Math.Cos(alfa) - y * Math.Sin(alfa) + X;
            v3.Y = x * Math.Sin(alfa) + y * Math.Cos(alfa) + Y;
            x = v4.X; y = v4.Y;
            v4.X = x * Math.Cos(alfa) - y * Math.Sin(alfa) + X;
            v4.Y = x * Math.Sin(alfa) + y * Math.Cos(alfa) + Y;

            Contour = new Quadrangle(v1, v2, v3, v4);
        }

        private double RadToDeg(double radians)
        {
            return radians * 180 / Math.PI;
        }

        private double DegToRad(double degries)
        {
            return degries * Math.PI / 180;
        }
    }
}
