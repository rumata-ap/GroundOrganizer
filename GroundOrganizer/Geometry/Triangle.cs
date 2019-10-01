using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    [Serializable]
    public class Triangle : Polygon
    {
        private Point3d vertex1;
        private Point3d vertex2;
        private Point3d vertex3;

        public Point3d Vertex1 { get => vertex1; set { vertex1 = value; Vertices[0] = vertex1; CalcTriangle(); } }
        public Point3d Vertex2 { get => vertex2; set { vertex2 = value; Vertices[1] = vertex2; CalcTriangle(); } }
        public Point3d Vertex3 { get => vertex3; set { vertex3 = value; Vertices[2] = vertex3; CalcTriangle(); } }
        public double MaxAngleDeg { get; private set; }

        public Triangle(Point3d node1, Point3d node2, Point3d node3)
        {
            vertex1 = node1;
            AddVertice(vertex1);
            vertex2 = node2;
            AddVertice(vertex2);
            vertex3 = node3;
            AddVertice(vertex3);
            CalcTriangle();
        }

        public Triangle(Point2d node1, Point2d node2, Point2d node3)
        {
            vertex1 = node1.ToPoint3d();
            AddVertice(vertex1);
            vertex2 = node2.ToPoint3d();
            AddVertice(vertex2);
            vertex3 = node3.ToPoint3d();
            AddVertice(vertex3);
            CalcTriangle();
        }

        public void CalcTriangle()
        {
            //Edge1 = new Line3d(vertex1, vertex2);
            //Edge2 = new Line3d(vertex2, vertex3);
            //Edge3 = new Line3d(vertex3, vertex1);
            //Centroid = new Point3d((vertex1.X + vertex2.X + vertex3.X) / 3, (vertex1.Y + vertex2.Y + vertex3.Y) / 3, (vertex1.Z + vertex2.Z + vertex3.Z) / 3);
            //Point3d minPt, maxPt;
            //minPt = new Point3d(
            //    Math.Min(Math.Min(vertex1.X, vertex2.X), vertex3.X), 
            //    Math.Min(Math.Min(vertex1.Y, vertex2.Y), vertex3.Y), 
            //    Math.Min(Math.Min(vertex1.Z, vertex2.Z), vertex3.Z));
            //maxPt = new Point3d(
            //    Math.Max(Math.Max(vertex1.X, vertex2.X), vertex3.X), 
            //    Math.Max(Math.Max(vertex1.Y, vertex2.Y), vertex3.Y), 
            //    Math.Max(Math.Max(vertex1.Z, vertex2.Z), vertex3.Z));
            //BoundingBox = new BoundingBox2d(minPt.ToPoint2d(), maxPt.ToPoint2d());
            double ang1, ang2, ang3;
            ang1 = vertex1.AngleTo(vertex3, vertex2);
            ang2 = vertex2.AngleTo(vertex1, vertex3);
            ang3 = vertex3.AngleTo(vertex2, vertex1);
            MaxAngleDeg = Math.Max(Math.Max(ang1, ang2), ang3);
            CalcPolygon();
        }
    }
}
