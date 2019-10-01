using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    [Serializable]
    public class Vector : IVector
    {
        double[] arr;
        int n;

        public int N { get => n; }
        public double this[int i] { get => arr[i]; set => arr[i] = value; }

        public Vector(int N)
        {
            n = N;
            arr = new double[N];
        }

        public Vector(Vector source)
        {
            n = source.N;
            arr = new double[source.N];
            arr = (double[])source.arr.Clone();
        }

        public Vector(Vector3d source)
        {
            n = 3;
            arr = new double[n];
            arr[0] = source.Vx;
            arr[1] = source.Vy;
            arr[2] = source.Vz;
        }

        public Vector(double[] source)
        {
            n = source.Length;
            arr = new double[source.Length];
            arr = (double[])source.Clone();
        }

        public double[] ToArray()
        {
            return arr;
        }

        public Vector3d ToVector3d()
        {
            return new Vector3d { Vx = arr[0], Vy = arr[1], Vz = arr[2] };
        }
    }
}
