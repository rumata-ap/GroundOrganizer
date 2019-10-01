using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    [Serializable]
    public class Polygon
    {
        private double area;
        private double ix;
        private double iy;
        private List<Point3d> vertices = new List<Point3d>();

        public bool Ckw { get; internal set; }
        //public bool? IsSelfItersected { get; private set; }
        public double Area { get => Math.Abs(area); }
        public double Perimeter { get; internal set; }
        public Point3d Centroid { get; internal set; }
        public double Ix { get => Math.Abs(ix); }
        public double Iy { get => Math.Abs(iy); }
        public List<Point3d> Vertices { get => vertices; set { vertices = value; CalcPolygon(); } }
        public BoundingBox2d BoundingBox { get; internal set; }
        public List<Line2d> Segments { get; internal set; }

        public Polygon()
        {

        }

        public Polygon(List<Point3d> vrts)
        {
            vertices = vrts;
            if (vertices[0].X != vertices[vrts.Count - 1].X && vertices[0].Y != vertices[vrts.Count - 1].Y) vertices.Add(vertices[0]);
            CalcPolygon();
        }

        public Polygon(List<Point2d> vrts)
        {
            foreach (Point2d item in vrts) vertices.Add(item.ToPoint3d());
            if (vertices[0].X != vertices[vrts.Count - 1].X && vertices[0].Y != vertices[vrts.Count - 1].Y) vertices.Add(vertices[0]);
            CalcPolygon();
        }

        public virtual void AddVertice(Point2d pt)
        {
            if (vertices.Count == 0)
            {
                vertices.Add(pt.ToPoint3d());
                return;
            }
            if (vertices.Count == 1)
            {
                vertices.Add(pt.ToPoint3d());
                vertices.Add(vertices[0]);
                return;
            }
            if (vertices.Count >= 3)
            {
                vertices.RemoveAt(vertices.Count - 1);
                vertices.Add(pt.ToPoint3d());
                vertices.Add(vertices[0]);
                CalcPolygon();
            }
        }

        public virtual void AddVertice(Point3d pt)
        {
            if (vertices.Count == 0)
            {
                vertices.Add(pt);
                return;
            }
            if (vertices.Count == 1)
            {
                vertices.Add(pt);
                vertices.Add(vertices[0]);
                return;
            }
            if (vertices.Count >= 3)
            {
                vertices.RemoveAt(vertices.Count - 1);
                vertices.Add(pt);
                vertices.Add(vertices[0]);
                CalcPolygon();
            }
        }

        private void CalcBB()
        {
            double minX = 1000000000;
            double minY = 1000000000;
            double maxX = -1000000000;
            double maxY = -1000000000;

            if (vertices.Count > 0)
            {
                foreach (Point3d item in vertices)
                {
                    if (item.X < minX) { minX = item.X; }
                    if (item.Y < minY) { minY = item.Y; }
                    if (item.X > maxX) { maxX = item.X; }
                    if (item.Y > maxY) { maxY = item.Y; }
                }

                BoundingBox = new BoundingBox2d(new Point2d(minX, minY), new Point2d(maxX, maxY));
            }
        }

        private void CalcSegs()
        {
            if (vertices.Count > 3)
            {
                Segments = new List<Line2d>();
                for (int i = 0; i < vertices.Count - 1; i++)
                {
                    Segments.Add(new Line2d(vertices[i], vertices[i + 1]));
                }
            }
        }

        protected void CalcI()
        {
            if (Segments.Count > 2)
            {
                double tempX = 0;
                double tempY = 0;
                for (int i = 0; i < Segments.Count; i++)
                {
                    Point3d arrTemp = Segments[i].StartPoint; Point3d arrTemp1 = Segments[i].EndPoint;
                    tempX += (Math.Pow(arrTemp.X, 2) + arrTemp.X * arrTemp1.X + Math.Pow(arrTemp1.X, 2)) * (arrTemp.X * arrTemp1.Y - arrTemp.Y * arrTemp1.X);
                    tempY += (Math.Pow(arrTemp.Y, 2) + arrTemp.Y * arrTemp1.Y + Math.Pow(arrTemp1.Y, 2)) * (arrTemp.X * arrTemp1.Y - arrTemp.Y * arrTemp1.X);
                }
                ix = tempX / 12;
                iy = tempY / 12;
            }
        }

        protected void CalcCentroid()
        {
            if (Segments.Count > 2)
            {
                Point3d temp = new Point3d();
                for (int i = 0; i < Segments.Count; i++)
                {
                    Point3d arrTemp = Segments[i].StartPoint; Point3d arrTemp1 = Segments[i].EndPoint;
                    temp.X += 1 / (6 * area) * (arrTemp.X + arrTemp1.X) * (arrTemp.X * arrTemp1.Y - arrTemp.Y * arrTemp1.X);
                    temp.Y += 1 / (6 * area) * (arrTemp.Y + arrTemp1.Y) * (arrTemp.X * arrTemp1.Y - arrTemp.Y * arrTemp1.X);
                }
                Centroid = temp;
            }
        }

        protected void CalcArea()
        {
            double temp = 0;
            if (Segments.Count > 2)
            {
                for (int i = 0; i < Segments.Count; i++)
                {
                    Point3d arrTemp = Segments[i].StartPoint; Point3d arrTemp1 = Segments[i].EndPoint;
                    temp += 0.5 * (arrTemp.X * arrTemp1.Y - arrTemp1.X * arrTemp.Y);
                }
                area = temp;
            }
        }

        protected void CalcPerimeter()
        {
            if (Segments.Count > 2)
            {
                Perimeter = 0;
                foreach (Line2d item in Segments)
                {
                    Perimeter += item.Length;
                }
            }
        }

        protected void CalcCkw()
        {
            if (area < 0) Ckw = true;
            else Ckw = false;
        }

        /// <summary>
        /// Вычисление свойств полигона
        /// </summary>
        public void CalcPolygon()
        {
            if (Vertices.Count > 3)
            {
                if ((Vertices[0].X != Vertices[vertices.Count - 1].X && Vertices[0].Y != Vertices[Vertices.Count - 1].Y)||
                    (Vertices[0].X == Vertices[vertices.Count - 1].X && Vertices[0].Y != Vertices[Vertices.Count - 1].Y)||
                    (Vertices[0].X != Vertices[vertices.Count - 1].X && Vertices[0].Y == Vertices[Vertices.Count - 1].Y))
                    Vertices.Add(Vertices[0]);
            }
            CalcBB();
            CalcSegs();
            CalcPerimeter();
            CalcArea();
            CalcCkw();
            CalcCentroid();
            CalcI();
        }

        /// <summary>
        /// Замена вершины полигона
        /// </summary>
        /// <param name="oldVertIdx">индекс заменяемой вершины</param>
        /// <param name="newVert">объект заменяющей вершины</param>
        public bool ReplaceVertice( int oldVertIdx, Point3d newVert)
        {
            try
            {
                Vertices[oldVertIdx] = newVert;
                if (oldVertIdx == 0 || oldVertIdx == Vertices.Count - 1) { Vertices[0] = newVert; Vertices[Vertices.Count - 1] = newVert; }
                CalcPolygon();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Triangulation(double shag, out List<Point3d> innerNodes, out List<Triangle> triangles)
        {
            innerNodes = new List<Point3d>();
            triangles = new List<Triangle>();
            List<Polygon> polygons = new List<Polygon>();            
            int n = Tesselation(shag);
            Polygon front = new Polygon(Vertices);
            //int p = 1;
            do
            {
                if (polygons.Count != 0) { front = polygons[0]; polygons.RemoveAt(0); }
                for (int j = 0; j < 1000; j++)
                {                   
                    AngleIn3Point minAngle;
                    front.MinAngleDeg(out minAngle);
                    while (minAngle.AngleDeg == 0 && front.Vertices.Count > 3)
                    {
                        if(minAngle.VerticeIdx == 0)
                        {
                            front.Vertices.RemoveRange(Vertices.Count - 2, 2);
                            front.Vertices.RemoveAt(0);                                                       
                        }
                        else front.Vertices.RemoveRange(minAngle.PreviousIdx, 2);
                        front.CalcPolygon();
                        front.MinAngleDeg(out minAngle);
                    }

                    if (Math.Round(minAngle.AngleDeg) < 90)
                    {                                             
                        triangles.Add(new Triangle(front.Vertices[minAngle.PreviousIdx], front.Vertices[minAngle.VerticeIdx], front.Vertices[minAngle.NextIdx]));
                        front.Vertices.RemoveAt(minAngle.VerticeIdx);
                        if (minAngle.VerticeIdx == 0 && front.Vertices.Count > 3) front.Vertices.RemoveAt(front.Vertices.Count - 1);
                        front.CalcPolygon();
                    }

                    else if (Math.Round(minAngle.AngleDeg) == 90 /*&& Math.Round(minAngle.AngleDeg) <= 140*/)
                    {
                        Line2d lin = new Line2d(front.Vertices[minAngle.VerticeIdx], front.Vertices[minAngle.PreviousIdx]);
                        Point3d temp = front.Vertices[minAngle.VerticeIdx];
                        Point3d g = new Point3d(front.Vertices[minAngle.NextIdx].X + lin.Directive.Vx, front.Vertices[minAngle.NextIdx].Y + lin.Directive.Vy, 0);
                        Point3d gNode = new Point3d { X = g.X, Y = g.Y, Z = g.Z/*, Number = n*/ };
                        bool ch = false;
                        int k = -1;
                        for (int i = 0; i < front.Vertices.Count - 1; i++)
                        {
                            //if (i == minAngle.VerticeIdx) continue;
                            double d = g.LengthTo(front.Vertices[i]);
                            if (Math.Round(g.X, 2) == Math.Round(front.Vertices[i].X, 2) && Math.Round(g.Y, 2) == Math.Round(front.Vertices[i].Y, 2))
                            { ch = true; k = i; break; }
                        }

                        Triangle t1, t2;

                        if (ch)
                        {
                            t1 = new Triangle(front.Vertices[minAngle.VerticeIdx], front.Vertices[minAngle.NextIdx], front.Vertices[k]);
                            t2 = new Triangle(front.Vertices[minAngle.PreviousIdx], front.Vertices[minAngle.VerticeIdx], front.Vertices[k]);
                        }
                        else
                        {
                            t1 = new Triangle(front.Vertices[minAngle.VerticeIdx], front.Vertices[minAngle.NextIdx], gNode);
                            t2 = new Triangle(front.Vertices[minAngle.PreviousIdx], front.Vertices[minAngle.VerticeIdx], gNode);
                        }

                        front.ReplaceVertice(minAngle.VerticeIdx, gNode);

                        if (front.SelfItersectCheck())
                        {
                            front.ReplaceVertice(minAngle.VerticeIdx, temp);
                            double dist = 100000000;
                            k = -1;
                            for (int i = 0; i < front.Vertices.Count - 1; i++)
                            {
                                if (i == minAngle.NextIdx || i == minAngle.PreviousIdx || i == minAngle.VerticeIdx) continue;
                                double d = front.Vertices[minAngle.VerticeIdx].LengthTo(front.Vertices[i]);
                                if (d < dist) { dist = d; k = i; }
                            }
                            Polygon sub;
                            front.Bisection(k, minAngle.VerticeIdx, out sub);
                            polygons.Add(sub);
                        }
                        else
                        {
                            if (!ch) { innerNodes.Add(gNode); n++; }
                            triangles.Add(t1); triangles.Add(t2);
                        }

                    }

                    else
                    {
                        Line2d linPrev = new Line2d(front.Vertices[minAngle.VerticeIdx], front.Vertices[minAngle.PreviousIdx]);
                        Line2d linNext = new Line2d(front.Vertices[minAngle.VerticeIdx], front.Vertices[minAngle.NextIdx]);
                        double vx = (linPrev.Directive.Unit[0] + linNext.Directive.Unit[0]) * 0.5;
                        double vy = (linPrev.Directive.Unit[1] + linNext.Directive.Unit[1]) * 0.5;
                        //double lengthBis = (linPrev.Length + linNext.Length) * 0.5;
                        double x = front.Vertices[minAngle.VerticeIdx].X + vx * shag;
                        double y = front.Vertices[minAngle.VerticeIdx].Y + vy * shag;
                        Point3d temp = front.Vertices[minAngle.VerticeIdx];
                        Point3d g = new Point3d(x, y, 0);
                        Line2d linBis = new Line2d(front.Vertices[minAngle.VerticeIdx], g);
                        x = front.Vertices[minAngle.VerticeIdx].X + linBis.Directive.Unit[0] * shag;
                        y = front.Vertices[minAngle.VerticeIdx].Y + linBis.Directive.Unit[1] * shag;
                        g = new Point3d(x, y, 0);
                        Point3d gNode = new Point3d() { X = g.X, Y = g.Y, Z = g.Z/*, Number = n*/ };
                        Triangle t1, t2;
                        t1 = new Triangle(front.Vertices[minAngle.VerticeIdx], front.Vertices[minAngle.NextIdx], gNode);
                        t2 = new Triangle(front.Vertices[minAngle.PreviousIdx], front.Vertices[minAngle.VerticeIdx], gNode);
                        front.ReplaceVertice(minAngle.VerticeIdx, gNode);
                        //if (p == 1) { Shpunt.AcCreator.CreateContour(front); p++; }

                        if (front.SelfItersectCheck())
                        {
                            front.ReplaceVertice(minAngle.VerticeIdx, temp);
                            double dist = 100000000;
                            int k = -1;
                            for (int i = 0; i < front.Vertices.Count - 1; i++)
                            {
                                if (i == minAngle.NextIdx || i == minAngle.PreviousIdx || i == minAngle.VerticeIdx) continue;
                                double d = front.Vertices[minAngle.VerticeIdx].LengthTo(front.Vertices[i]);
                                if (d < dist) { dist = d; k = i; }
                            }
                            Polygon sub;
                            front.Bisection(k, minAngle.VerticeIdx, out sub);
                            polygons.Add(sub);
                        }
                        else
                        {
                            triangles.Add(t1);
                            triangles.Add(t2);
                            innerNodes.Add(gNode);
                        }
                        n++;
                    }
                    if (front.Vertices.Count < 4) break;
                }
            } while (polygons.Count != 0);
        }

        /// <summary>
        /// Тесселяция полигона с заданным шагом
        /// </summary>
        /// <param name="shag"> Величина шага тесселяции</param>
        public int Tesselation(double shag)
        {
            List<Point3d> nodes = new List<Point3d>();
            int n = 1;
            int nD = 0;
            foreach (Line2d seg in Segments)
            {
                Point3d start = seg.StartPoint;
                Point3d end = seg.EndPoint;
                nD = (int)Math.Round(seg.Length / shag);
                if (nD > 1)
                {
                    double delta = seg.Length / nD;
                    double deltaX = (end.X - start.X) / nD;
                    double deltaY = (end.Y - start.Y) / nD;
                    double deltaZ = (end.Z - start.Z) / nD;
                    for (int j = 0; j < nD; j++)
                    {
                        Point3d pt = new Point3d(start.X + j * deltaX, start.Y + j * deltaY, start.Z + j * deltaZ);
                        //FEA.Node nd = new FEA.Node(pt) { Number = n };
                        nodes.Add(pt);
                        n++;
                    }
                }
                else
                {
                    Point3d pt = new Point3d(start.X, start.Y, start.Z);
                    //FEA.Node nd = new FEA.Node(pt) { Number = n };
                    nodes.Add(pt);
                    n++;
                }
            }
            Vertices = nodes;
            if (!Ckw) { Vertices.Reverse(); CalcPolygon(); }
            return n;
        }

        /// <summary>
        /// Деление полигона через указание 2-х точек деления
        /// </summary>
        /// <param name="sectPointIdx">Индекс первой точки деления(секущей)</param>
        /// <param name="basePointIdx">Индекс второй точки деления(базовой)</param>
        /// <param name="newPolygon">Возвращаемый отсеченный полигон</param>
        public void Bisection(int sectPointIdx,int basePointIdx, out Polygon newPolygon)
        {
            newPolygon = new Polygon();
            List<Point3d> tmp = new List<Point3d>();
            if (basePointIdx > sectPointIdx)
            {
                for (int i = sectPointIdx; i <= basePointIdx; i++)
                {
                    newPolygon.AddVertice(Vertices[i]);
                }
                newPolygon.CalcPolygon();
                Vertices.RemoveRange(sectPointIdx + 1, basePointIdx - sectPointIdx - 1);
                CalcPolygon();
            }
            else
            {
                for (int i = basePointIdx; i <= sectPointIdx; i++)
                {
                    newPolygon.AddVertice(Vertices[i]);
                }
                newPolygon.CalcPolygon();
                Vertices.RemoveRange(basePointIdx + 1, sectPointIdx - basePointIdx - 1);
                CalcPolygon();
            }
        }

        /// <summary>
        /// Проверка полигона на самопересечение (возвращает булево значение, если true - полигон самопересекающийся)
        /// </summary>
        public bool SelfItersectCheck()
        {
            bool IsSelfItersected = false;
            if (Segments.Count < 4) return IsSelfItersected;
            for (int i = 0; i < Segments.Count; i++)
            {
                for (int j = 0; j < Segments.Count; j++)
                {
                    if (i == j) continue;
                    double a1 = Segments[i].A;
                    double a2 = Segments[j].A;
                    double b1 = Segments[i].B;
                    double b2 = Segments[j].B;
                    double c1 = Segments[i].C;
                    double c2 = Segments[j].C;
                    double p1 = (b2 - b1 * a2 / a1);
                    if (p1 == 0) continue;
                    double y = (a2 * c1 / a1 - c2) / p1;
                    double x = (-b1 * y - c1) / a1;
                    Point3d ptItersect = new Point3d(x, y, 0);
                    Line2d linTmp1 = new Line2d(Segments[i].CenterPoint, ptItersect);
                    Line2d linTmp2 = new Line2d(Segments[j].CenterPoint, ptItersect);
                    double l1 = Math.Round(linTmp1.Length, 2);
                    double l11 = Math.Round(Segments[i].Length / 2, 2);
                    double l2 = Math.Round(linTmp2.Length, 2);
                    double l22 = Math.Round(Segments[j].Length / 2, 2);
                    if (Math.Round(linTmp1.Length, 2) < Math.Round(Segments[i].Length / 2, 2) &&
                        Math.Round(linTmp2.Length, 2) < Math.Round(Segments[j].Length / 2, 2))
                    {
                        IsSelfItersected = true;
                        return IsSelfItersected;
                    }
                }
            }
            return IsSelfItersected;
        }
        /// <summary>
        /// Вычисление минимального угла полигона  в градусах
        /// </summary>
        /// <returns></returns>
        public double MinAngleDeg()
        {
            double minAngle = 360;
            double angleTmp;
            bool type;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                if (i == 0)
                {
                    angleTmp = Vertices[0].AngleTo(Vertices[Vertices.Count - 2], Vertices[1]);
                    if (angleTmp is double.NaN) angleTmp = 180;
                    type = Vertices[0].NormalDirection(Vertices[Vertices.Count - 2], Vertices[1]);
                    if (!type && angleTmp != 0) angleTmp = 360 - angleTmp;
                    if (!type && angleTmp == 0) angleTmp = 0;
                    if (angleTmp < minAngle) minAngle = angleTmp;
                }
                else
                {
                    angleTmp = Vertices[i].AngleTo(Vertices[i - 1], Vertices[i + 1]);
                    if (angleTmp is double.NaN) angleTmp = 180;
                    type = Vertices[i].NormalDirection(Vertices[i - 1], Vertices[i + 1]);
                    if (!type && angleTmp != 0) angleTmp = 360 - angleTmp;
                    if (!type && angleTmp == 0) angleTmp = 0;
                    if (angleTmp < minAngle) minAngle = angleTmp;
                }
            }
            return minAngle;
        }
        /// <summary>
        /// Вычисление минимального угла полигона в градусах (возвращает массив индексов 3-х точек на которых был найден угол)
        /// </summary>
        /// <param name="minAngle"> Возвращаемое значение минимального угла в градусах </param>
        public int[] MinAngleDeg(out double minAngle)
        {
            minAngle = 0;
            if (Vertices.Count < 3) return null;
            int[] res = new int[3];
            minAngle = 360;
            double angleTmp;
            bool type;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                if (i == 0)
                {
                    angleTmp = Vertices[0].AngleTo(Vertices[Vertices.Count - 2], Vertices[i + 1]);
                    if (angleTmp is double.NaN) angleTmp = 180;
                    type = Vertices[i].NormalDirection(Vertices[Vertices.Count - 2], Vertices[i + 1]);
                    if (!type && angleTmp != 0) angleTmp = 360 - angleTmp;
                    if (!type && angleTmp == 0) angleTmp = 0;
                    if (angleTmp < minAngle)
                    {
                        res[0] = Vertices.Count - 2;
                        res[1] = 0;
                        res[2] = 1;
                        minAngle = angleTmp;
                    }
                }
                else
                {
                    angleTmp = Vertices[i].AngleTo(Vertices[i - 1], Vertices[i + 1]);
                    if (angleTmp is double.NaN) angleTmp = 180;
                    type = Vertices[i].NormalDirection(Vertices[i - 1], Vertices[i + 1]);
                    if (!type && angleTmp != 0) angleTmp = 360 - angleTmp;
                    if (!type && angleTmp == 0) angleTmp = 0;
                    if (angleTmp < minAngle)
                    {
                        res[0] = i - 1;
                        res[1] = i;
                        res[2] = i + 1;
                        minAngle = angleTmp;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Вычисление минимального угла полигона в градусах
        /// </summary>
        /// <param name="result">Возвращаемое значение в виде объекта со свойствами индексов трех точек образуюших угол и значения угла в градусах</param>
        public void MinAngleDeg(out AngleIn3Point result)
        {
            result = new AngleIn3Point();
            result.AngleDeg = 0;
            if (Vertices.Count < 3) return;
            result.AngleDeg = 360;
            double angleTmp;
            bool type;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                if (i == 0)
                {
                    angleTmp = Vertices[0].AngleTo(Vertices[Vertices.Count - 2], Vertices[i + 1]);
                    if (angleTmp is double.NaN) angleTmp = 180;
                    type = Vertices[i].NormalDirection(Vertices[Vertices.Count - 2], Vertices[i + 1]);
                    if (!type && angleTmp != 0) angleTmp = 360 - angleTmp;
                    if (!type && angleTmp == 0) angleTmp = 0;
                    if (angleTmp < result.AngleDeg)
                    {
                        result.PreviousIdx = Vertices.Count - 2;
                        result.VerticeIdx = 0;
                        result.NextIdx = 1;
                        result.AngleDeg = angleTmp;
                    }
                }
                else
                {
                    angleTmp = Vertices[i].AngleTo(Vertices[i - 1], Vertices[i + 1]);
                    if (angleTmp is double.NaN) angleTmp = 180;
                    type = Vertices[i].NormalDirection(Vertices[i - 1], Vertices[i + 1]);
                    if (!type && angleTmp != 0) angleTmp = 360 - angleTmp;
                    if (!type && angleTmp == 0) angleTmp = 0;
                    if (angleTmp < result.AngleDeg)
                    {
                        result.PreviousIdx = i - 1;
                        result.VerticeIdx = i;
                        result.NextIdx = i + 1;
                        result.AngleDeg = angleTmp;
                    }
                }
            }
        }
        /// <summary>
        /// Вычисление максимального угла полигона в градусах
        /// </summary>
        /// <returns></returns>
        public double MaxAngleDeg()
        {
            double minAngle = 0;
            double angleTmp;
            bool type;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                if (i == 0)
                {
                    angleTmp = Vertices[0].AngleTo(Vertices[Vertices.Count - 2], Vertices[i + 1]);
                    if (angleTmp is double.NaN) angleTmp = 180;
                    type = Vertices[i].NormalDirection(Vertices[Vertices.Count - 2], Vertices[i + 1]);
                    if (!type && angleTmp != 0) angleTmp = 360 - angleTmp;
                    if (!type && angleTmp == 0) angleTmp = 0;
                    if (angleTmp > minAngle) minAngle = angleTmp;
                }
                else
                {
                    angleTmp = Vertices[i].AngleTo(Vertices[i - 1], Vertices[i + 1]);
                    if (angleTmp is double.NaN) angleTmp = 180;
                    type = Vertices[i].NormalDirection(Vertices[i - 1], Vertices[i + 1]);
                    if (!type && angleTmp != 0) angleTmp = 360 - angleTmp;
                    if (!type && angleTmp == 0) angleTmp = 0;
                    if (angleTmp > minAngle) minAngle = angleTmp;
                }
            }
            
            return minAngle;
        }
    }
    /// <summary>
    /// Объект-структура со свойствами индексов трех точек образуюших угол и значения угла в градусах
    /// </summary>
    public struct AngleIn3Point
    {
        /// <summary>
        /// Индекс предыдущей вершины
        /// </summary>
        public int PreviousIdx { get; set; }
        /// <summary>
        /// Индех следующей вершины
        /// </summary>
        public int NextIdx { get; set; }
        /// <summary>
        /// Индекс вершины, на которой найден угол
        /// </summary>
        public int VerticeIdx { get; set; }
        /// <summary>
        /// Значение найденного угла в градусах
        /// </summary>
        public double AngleDeg { get; set; }

    }
}
