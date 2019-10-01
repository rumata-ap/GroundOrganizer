using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    [Serializable]
    public class Quadrangle : Polygon
    {
        private Point3d vertex1 = new Point3d();
        private Point3d vertex2 = new Point3d();
        private Point3d vertex3 = new Point3d();
        private Point3d vertex4 = new Point3d();

        public Point3d Vertex1 { get => vertex1; set { vertex1 = value; Vertices[0] = vertex1; CalcQuadrangle(); } }
        public Point3d Vertex2 { get => vertex2; set { vertex2 = value; Vertices[1] = vertex2; CalcQuadrangle(); } }
        public Point3d Vertex3 { get => vertex3; set { vertex3 = value; Vertices[2] = vertex3; CalcQuadrangle(); } }
        public Point3d Vertex4 { get => vertex4; set { vertex4 = value; Vertices[3] = vertex4; CalcQuadrangle(); } }
        public double MaxAngleDeg { get; private set; }
        public bool IsValid { get; private set; }
        public UnitsLin Units { get; set; }

        public Quadrangle(Triangle trg1, Triangle trg2)
        {
            List<Point3d> vrt = new List<Point3d>();
            List<Point3d> dgn = new List<Point3d>();
            vrt.Add(trg1.Vertex1);
            vrt.Add(trg1.Vertex2);
            vrt.Add(trg1.Vertex3);
            vrt.Add(trg2.Vertex1);
            vrt.Add(trg2.Vertex2);
            vrt.Add(trg2.Vertex3);
            for (int i = 0; i < vrt.Count; i++)
            {
                for (int j = 0; j < vrt.Count; j++)
                {
                    if (i == j) continue;
                    if (Math.Round(vrt[i].X, 2) == Math.Round(vrt[j].X,2) && Math.Round(vrt[i].Y,2) == Math.Round(vrt[j].Y,2))
                    {
                        dgn.Add(vrt[i]);
                        vrt.RemoveAt(j);
                        break;
                    }
                }
            }

            if (vrt.Count == 4)
            {
                if (vrt[0].Equals(dgn[0]) && vrt[1].Equals(dgn[1]))
                {
                    vertex1 = vrt[0];
                    AddVertice(Vertex1);
                    vertex2 = vrt[2];
                    AddVertice(Vertex2);
                    vertex3 = vrt[1];
                    AddVertice(Vertex3);
                    vertex4 = vrt[3];
                    AddVertice(Vertex4);
                }
                if (vrt[1].Equals(dgn[0]) && vrt[2].Equals(dgn[1]))
                {
                    vertex1 = vrt[0];
                    AddVertice(Vertex1);
                    vertex2 = vrt[2];
                    AddVertice(Vertex2);
                    vertex3 = vrt[3];
                    AddVertice(Vertex3);
                    vertex4 = vrt[1];
                    AddVertice(Vertex4);
                }
                if (vrt[0].Equals(dgn[0]) && vrt[2].Equals(dgn[1]))
                {
                    vertex1 = vrt[0];
                    AddVertice(Vertex1);
                    vertex2 = vrt[1];
                    AddVertice(Vertex2);
                    vertex3 = vrt[2];
                    AddVertice(Vertex3);
                    vertex4 = vrt[3];
                    AddVertice(Vertex4);
                }
                if (vrt[1].Equals(dgn[0]) && vrt[3].Equals(dgn[1]))
                {
                    vertex1 = vrt[0];
                    AddVertice(Vertex1);
                    vertex2 = vrt[1];
                    AddVertice(Vertex2);
                    vertex3 = vrt[2];
                    AddVertice(Vertex3);
                    vertex4 = vrt[3];
                    AddVertice(Vertex4);
                }
                IsValid = true;
                CalcQuadrangle();
            }
            else
            {
                IsValid = false;
                Vertices.Add(new Point3d());
                Vertices.Add(new Point3d());
                Vertices.Add(new Point3d());
                Vertices.Add(new Point3d());
            }
        }

        public Quadrangle(Point3d node1, Point3d node2, Point3d node3, Point3d node4)
        {
            vertex1 = node1;
            AddVertice(node1);
            vertex2 = node2;
            AddVertice(node2);
            vertex3 = node3;
            AddVertice(node3);
            vertex4 = node4;
            AddVertice(node4);
            CalcQuadrangle();
            IsValid = true;
        }

        public Quadrangle(Point2d node1, Point2d node2, Point2d node3, Point2d node4)
        {
            vertex1 = node1.ToPoint3d();
            AddVertice(node1);
            vertex2 = node2.ToPoint3d();
            AddVertice(node2);
            vertex3 = node3.ToPoint3d();
            AddVertice(node3);
            vertex4 = node4.ToPoint3d();
            AddVertice(node4);
            CalcQuadrangle();
            IsValid = true;
        }

        public void CalcQuadrangle()
        {
            double ang1, ang2, ang3, ang4;
            ang1 = vertex1.AngleTo(vertex4, vertex2);
            ang2 = vertex2.AngleTo(vertex1, vertex3);
            ang3 = vertex3.AngleTo(vertex2, vertex4);
            ang4 = vertex4.AngleTo(vertex3, vertex1);
            MaxAngleDeg = Math.Max(Math.Max(Math.Max(ang1, ang2), ang3), ang4);
            CalcPolygon();
        }
    }
}
