using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;

namespace GroundOrganizer
{
    public  struct My_Mq_Mc
    {
        public double My;
        public double Mq;
        public double Mc;
        Dictionary<double, double[]> table_5_5;

        public My_Mq_Mc(double fi)
        {
            table_5_5 = new Dictionary<double, double[]>
            {
                {0,new double[]{0,1,3.14}},
                {1,new double[]{0.01,1.06,3.23}},
                {2,new double[]{0.03,1.12,3.32}},
                {3,new double[]{0.04,1.18,3.41}},
                {4,new double[]{0.06,1.25,3.51}},
                {5,new double[]{0.08,1.32,3.61}},
                {6,new double[]{0.1,1.39,3.71}},
                {7,new double[]{0.12,1.47,3.82}},
                {8,new double[]{0.14,1.55,3.93}},
                {9,new double[]{0.16,1.64,4.05}},
                {10,new double[]{0.18,1.73,4.17}},
                {11,new double[]{0.21,1.83,4.29}},
                {12,new double[]{0.23,1.94,4.42}},
                {13,new double[]{0.26,2.05,4.55}},
                {14,new double[]{0.29,2.17,4.69}},
                {15,new double[]{0.32,2.3,4.84}},
                {16,new double[]{0.36,2.43,4.99}},
                {17,new double[]{0.39,2.57,5.15}},
                {18,new double[]{0.43,2.73,5.31}},
                {19,new double[]{0.47,2.89,5.48}},
                {20,new double[]{0.51,3.06,5.66}},
                {21,new double[]{0.56,3.24,5.84}},
                {22,new double[]{0.61,3.44,6.04}},
                {23,new double[]{0.66,3.65,6.24}},
                {24,new double[]{0.72,3.87,6.45}},
                {25,new double[]{0.78,4.11,6.67}},
                {26,new double[]{0.84,4.37,6.9}},
                {27,new double[]{0.91,4.64,7.14}},
                {28,new double[]{0.98,4.93,7.4}},
                {29,new double[]{1.06,5.25,7.67}},
                {30,new double[]{1.15,5.59,7.95}},
                {31,new double[]{1.24,5.95,8.24}},
                {32,new double[]{1.34,6.34,8.55}},
                {33,new double[]{1.44,6.76,8.88}},
                {34,new double[]{1.55,7.22,9.22}},
                {35,new double[]{1.68,7.71,9.58}},
                {36,new double[]{1.81,8.24,9.97}},
                {37,new double[]{1.95,8.81,10.37}},
                {38,new double[]{2.11,9.44,10.8}},
                {39,new double[]{2.28,10.11,11.25}},
                {40,new double[]{2.46,10.85,11.73}},
                {41,new double[]{2.66,11.64,12.24}},
                {42,new double[]{2.88,12.51,12.79}},
                {43,new double[]{3.12,13.46,13.37}},
                {44,new double[]{3.38,14.5,13.98}},
                {45,new double[]{3.66,15.64,14.64}}
            };
            My= table_5_5[fi][0];
            Mq = table_5_5[fi][1];
            Mc = table_5_5[fi][2];
        }
    }
    public class TablesInterolator
    {
        static readonly Dictionary<double, double[]> table_5_5 = new Dictionary<double, double[]>
            {
                {0,new double[]{0,1,3.14}},
                {1,new double[]{0.01,1.06,3.23}},
                {2,new double[]{0.03,1.12,3.32}},
                {3,new double[]{0.04,1.18,3.41}},
                {4,new double[]{0.06,1.25,3.51}},
                {5,new double[]{0.08,1.32,3.61}},
                {6,new double[]{0.1,1.39,3.71}},
                {7,new double[]{0.12,1.47,3.82}},
                {8,new double[]{0.14,1.55,3.93}},
                {9,new double[]{0.16,1.64,4.05}},
                {10,new double[]{0.18,1.73,4.17}},
                {11,new double[]{0.21,1.83,4.29}},
                {12,new double[]{0.23,1.94,4.42}},
                {13,new double[]{0.26,2.05,4.55}},
                {14,new double[]{0.29,2.17,4.69}},
                {15,new double[]{0.32,2.3,4.84}},
                {16,new double[]{0.36,2.43,4.99}},
                {17,new double[]{0.39,2.57,5.15}},
                {18,new double[]{0.43,2.73,5.31}},
                {19,new double[]{0.47,2.89,5.48}},
                {20,new double[]{0.51,3.06,5.66}},
                {21,new double[]{0.56,3.24,5.84}},
                {22,new double[]{0.61,3.44,6.04}},
                {23,new double[]{0.66,3.65,6.24}},
                {24,new double[]{0.72,3.87,6.45}},
                {25,new double[]{0.78,4.11,6.67}},
                {26,new double[]{0.84,4.37,6.9}},
                {27,new double[]{0.91,4.64,7.14}},
                {28,new double[]{0.98,4.93,7.4}},
                {29,new double[]{1.06,5.25,7.67}},
                {30,new double[]{1.15,5.59,7.95}},
                {31,new double[]{1.24,5.95,8.24}},
                {32,new double[]{1.34,6.34,8.55}},
                {33,new double[]{1.44,6.76,8.88}},
                {34,new double[]{1.55,7.22,9.22}},
                {35,new double[]{1.68,7.71,9.58}},
                {36,new double[]{1.81,8.24,9.97}},
                {37,new double[]{1.95,8.81,10.37}},
                {38,new double[]{2.11,9.44,10.8}},
                {39,new double[]{2.28,10.11,11.25}},
                {40,new double[]{2.46,10.85,11.73}},
                {41,new double[]{2.66,11.64,12.24}},
                {42,new double[]{2.88,12.51,12.79}},
                {43,new double[]{3.12,13.46,13.37}},
                {44,new double[]{3.38,14.5,13.98}},
                {45,new double[]{3.66,15.64,14.64}}
            };
        static readonly Dictionary<string, double[]> table_5_4 = new Dictionary<string, double[]>
        {
            {"Крупнообломочные с песчаным заполнителем и пески кроме мелких и пылеватых", new double[]{1.4, 1.2, 1.4} },
            {"Пески мелкие", new double[]{1.3, 1.1, 1.3} },
            {"Пески пылеватые маловлажные", new double[]{1.25, 1.0, 1.2} },
            {"Пески пылеватые влажные насыщенные водой", new double[]{1.1, 1.0, 1.2} },
            {"Пески рыхлые", new double[]{1.0, 1.0, 1.0} },
            {"Глинистые, а также крупнообломочные с глинистым заполнителем при IL<=0.25", new double[]{1.25, 1.0, 1.1} },
            {"Глинистые, а также крупнообломочные с глинистым заполнителем при 0.25<IL<=0.5", new double[]{1.2, 1.0, 1.1} },
            {"Глинистые, а также крупнообломочные с глинистым заполнителем при 0.5<IL", new double[]{1.1, 1.0, 1.0} },
        };
        static readonly double[,] table_5_8_R =
        {
            { 1, 1, 1, 1, 1, 1, 1 },
            { 0.96, 0.972, 0.975, 0.976, 0.977, 0.977, 0.977 },
            { 0.8, 0.848, 0.866, 0.876, 0.879, 0.881, 0.881 },
            { 0.606, 0.682, 0.717, 0.739, 0.749, 0.754, 0.755 },
            { 0.449, 0.532, 0.578, 0.612, 0.629, 0.639, 0.642 },
            { 0.336, 0.414, 0.463, 0.505, 0.53, 0.545, 0.55 },
            { 0.257, 0.325, 0.374, 0.419, 0.449, 0.47, 0.477 },
            { 0.201, 0.26, 0.304, 0.349, 0.383, 0.41, 0.42 },
            { 0.16, 0.21, 0.251, 0.294, 0.329, 0.36, 0.374 },
            { 0.131, 0.173, 0.209, 0.25, 0.285, 0.319, 0.337 },
            { 0.108, 0.145, 0.176, 0.214, 0.248, 0.285, 0.306 },
            { 0.091, 0.123, 0.15, 0.185, 0.218, 0.255, 0.28 },
            { 0.077, 0.105, 0.13, 0.161, 0.192, 0.23, 0.258 },
            { 0.067, 0.091, 0.113, 0.141, 0.17, 0.208, 0.239 },
            { 0.058, 0.079, 0.099, 0.124, 0.152, 0.189, 0.223 },
            { 0.051, 0.07, 0.087, 0.11, 0.136, 0.173, 0.208 },
            { 0.045, 0.062, 0.077, 0.099, 0.122, 0.158, 0.196 },
            { 0.04, 0.055, 0.069, 0.088, 0.11, 0.145, 0.185 },
            { 0.036, 0.049, 0.062, 0.08, 0.1, 0.133, 0.175 },
            { 0.032, 0.044, 0.056, 0.072, 0.091, 0.123, 0.166 },
            { 0.029, 0.04, 0.051, 0.066, 0.084, 0.113, 0.158 },
            { 0.026, 0.037, 0.046, 0.06, 0.077, 0.105, 0.15 },
            { 0.024, 0.033, 0.042, 0.055, 0.071, 0.098, 0.143 },
            { 0.022, 0.031, 0.039, 0.051, 0.065, 0.091, 0.137 },
            { 0.02, 0.028, 0.036, 0.047, 0.06, 0.085, 0.132 },
            { 0.019, 0.026, 0.033, 0.043, 0.056, 0.079, 0.126 },
            { 0.017, 0.024, 0.031, 0.04, 0.052, 0.074, 0.122 },
            { 0.016, 0.022, 0.029, 0.037, 0.049, 0.069, 0.117 },
            { 0.015, 0.021, 0.027, 0.035, 0.045, 0.065, 0.113 },
            { 0.014, 0.02, 0.025, 0.033, 0.042, 0.061, 0.109 },
            { 0.013, 0.018, 0.023, 0.031, 0.04, 0.058, 0.106 },
        };
        static readonly double[] table_5_8_C = 
        {
            1, 0.949, 0.756, 0.547, 0.39, 0.285, 0.214, 0.165, 0.13,
            0.106, 0.087, 0.073, 0.062, 0.053, 0.046, 0.04, 0.036,
            0.031, 0.028, 0.024, 0.022, 0.021, 0.019, 0.017,
            0.016, 0.015, 0.014, 0.013, 0.012, 0.011, 0.01
        };
        static readonly double[] table_5_8_L =
        {
            1,0.977,0.881,0.755,0.642,0.55,0.477,0.42,0.374,0.337,
            0.306,0.28,0.258,0.239,0.223,0.208,0.196,0.185,0.175,
            0.166,0.158,0.15,0.143,0.137,0.132,0.126,0.122,0.117,0.113,0.109,0.106
        };
        static readonly double[] table_5_8_ksi =
        {
            0,0.4,0.8,1.2,1.6,2,2.4,2.8,3.2,3.6,4,
            4.4,4.8,5.2,5.6,6,6.4,6.8,7.2,7.6,8,8.4,
            8.8,9.2,9.6,10,10.4,10.8,11.2,11.6,12
        };
        static readonly double[] table_5_8_nu = { 1, 1.4, 1.8, 2.4, 3.2, 5, 10 };
        public static double[] My_Mq_Mc(double fi)
        {         
            return table_5_5[fi];
        }

        public static double Yc1(string ground)
        {
            return table_5_4[ground][0];
        }

        public static double Yc2(string ground, TypeFlexStructure flex = TypeFlexStructure.Гибкая, double L = 0, double H = 1)
        {
            if (flex == TypeFlexStructure.Гибкая) return 1;

            double Yc21 = table_5_4[ground][1];
            double Yc22 = table_5_4[ground][2];
            Geo.Line2d interpolant = new Geo.Line2d(new Geo.Point2d(4, Yc21), new Geo.Point2d(1.5, Yc22));
            if (L / H < 4 && L / H > 1.5) return interpolant.Interpolation(L / H);
            else if (L / H >= 4) return Yc21;
            else return Yc22;
        }

        public static double? table_5_8(double z, double b, double l, TypeFound typeFound = TypeFound.Прямоугольный, PointFound ptFound = PointFound.Центр)
        {
            double nu, ksi;
            if (ptFound == PointFound.Центр) ksi = 2 * z / b;
            else ksi = z / b;

            if (typeFound == TypeFound.Прямоугольный && l / b < 10)
            {
                nu = l / b;
                return BilinearInterpolation(table_5_8_ksi, table_5_8_nu, table_5_8_R, ksi, nu);
            }
            if (typeFound == TypeFound.Ленточный || l / b >= 10)
            {
                return LinearInterpolation(table_5_8_ksi, table_5_8_L, ksi);
            }
            if (typeFound == TypeFound.Круглый)
            {
                return LinearInterpolation(table_5_8_ksi, table_5_8_C, ksi);
            }

            return null;
        }

        public static List<double?> Table_5_8(List<double> z, double b, double l, TypeFound typeFound = TypeFound.Прямоугольный, PointFound ptFound = PointFound.Центр)
        {
            List<double?> res = new List<double?>();
            double nu, ksi;
            
            foreach (double item in z)
            {
                if (ptFound == PointFound.Центр) ksi = 2 * item / b;
                else ksi = item / b;

                if (typeFound == TypeFound.Прямоугольный && l / b < 10)
                {
                    nu = l / b;
                    res.Add(BilinearInterpolation(table_5_8_ksi, table_5_8_nu, table_5_8_R, ksi, nu));
                }
                if (typeFound == TypeFound.Ленточный || l / b >= 10)
                {
                    res.Add(LinearInterpolation(table_5_8_ksi, table_5_8_L, ksi));
                }
                if (typeFound == TypeFound.Круглый)
                {
                    res.Add(LinearInterpolation(table_5_8_ksi, table_5_8_C, ksi));
                }
            }

            return res;
        }

        static double? LinearInterpolation(double[] x, double[] y, double xval)
        {
            double? zval = null;
            Line2d interpolant;
            int lx = x.Length;
            int ly = y.Length;

            if (lx < 2 || ly < 2) return zval;
            if (lx != ly) return zval;

            for (int i = 0; i < lx - 1; i++)
            {
                if (xval >= x[i] && xval < x[i + 1])
                {
                    interpolant = new Line2d(new Point2d(x[i], y[i]), new Point2d(x[i + 1], y[i + 1]));
                    return interpolant.Interpolation(xval);
                }

                if (xval < x[0])
                {
                    interpolant = new Line2d(new Point2d(x[0], y[0]), new Point2d(x[1], y[1]));
                    return interpolant.Interpolation(xval);
                }

                if (xval >= x[lx - 1])
                {
                    interpolant = new Line2d(new Point2d(x[lx - 2], y[lx - 2]), new Point2d(x[lx - 1], y[lx - 1]));
                    return interpolant.Interpolation(xval);
                }
            }

            return zval;
        }

        static double? BilinearInterpolation(double[] x, double[] y, double[,] z, double xval, double yval)
        {
            //calculates single point bilinear interpolation
            double? zval = null;
            Plane interpolant;
            int lx = x.Length;
            int ly = y.Length;

            if (lx < 2 || ly < 2 || z.Length < 4) return zval;
            if (lx !=z.GetLength(0)) return zval;
            if (ly !=z.GetLength(1)) return zval;

            for (int i = 0; i < lx - 1; i++)
            {
                for (int j = 0; j < ly - 1; j++)
                {
                    if (xval >= x[i] && xval < x[i + 1] && yval >= y[j] && yval < y[j + 1])
                    {
                        interpolant = new Plane(
                            new Point3d(x[i], y[j], z[i, j]), 
                            new Point3d(x[i + 1], y[j], z[i + 1, j]), 
                            new Point3d(x[i], y[j + 1], z[i, j + 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }

                    if (xval >= x[lx-1] && yval >= y[j] && yval < y[j + 1])
                    {
                        interpolant = new Plane(
                            new Point3d(x[lx - 2], y[j], z[lx - 2, j]),
                            new Point3d(x[lx - 1], y[j], z[lx - 1, j]),
                            new Point3d(x[lx - 2], y[j + 1], z[lx - 2, j + 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }
                    
                    if (xval < x[0] && yval >= y[j] && yval < y[j + 1])
                    {
                        interpolant = new Plane(
                            new Point3d(x[0], y[j], z[0, j]),
                            new Point3d(x[1], y[j], z[1, j]),
                            new Point3d(x[0], y[j + 1], z[0, j + 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }

                    if (xval >= x[i] && xval < x[i + 1] && yval >= y[ly-1])
                    {
                        interpolant = new Plane(
                            new Point3d(x[i], y[ly - 2], z[i, ly - 2]),
                            new Point3d(x[i + 1], y[ly - 2], z[i + 1, ly - 2]),
                            new Point3d(x[i], y[ly - 1], z[i, ly - 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }

                    if (xval >= x[i] && xval < x[i + 1] && yval < y[0])
                    {
                        interpolant = new Plane(
                            new Point3d(x[i], y[0], z[i, 0]),
                            new Point3d(x[i + 1], y[0], z[i + 1, 0]),
                            new Point3d(x[i], y[1], z[i, 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }

                    if (xval < x[0] && yval < y[0])
                    {
                        interpolant = new Plane(
                            new Point3d(x[0], y[0], z[0, 0]),
                            new Point3d(x[1], y[0], z[1, 0]),
                            new Point3d(x[0], y[1], z[0, 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }

                    if (xval >= x[lx-1] && yval < y[0])
                    {
                        interpolant = new Plane(
                            new Point3d(x[lx - 2], y[0], z[lx - 2, 0]),
                            new Point3d(x[lx - 1], y[0], z[lx - 1, 0]),
                            new Point3d(x[lx - 2], y[1], z[lx - 2, 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }

                    if (xval < x[0] && yval >= y[ly-1])
                    {
                        interpolant = new Plane(
                            new Point3d(x[0], y[ly - 2], z[0, ly - 2]),
                            new Point3d(x[0], y[ly - 1], z[0, ly - 1]),
                            new Point3d(x[1], y[ly - 1], z[1, ly - 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }

                    if (xval >= x[lx-1] && yval >= y[ly - 1])
                    {
                        interpolant = new Plane(
                            new Point3d(x[lx - 2], y[ly - 2], z[lx - 2, ly - 2]),
                            new Point3d(x[lx - 1], y[ly - 2], z[lx - 1, ly - 2]),
                            new Point3d(x[lx - 2], y[ly - 1], z[lx - 2, ly - 1])
                            );
                        return interpolant.Interpolation(xval, yval);
                    }
                }
            }
            return zval;
        }

        //double[] BilinearInterpolation(double[] x, double[] y, double[,] z, double[] xvals, double[] yvals)
        //{
        //    //calculates multiple point bilinear interpolation
        //    double[] zvals = new double[xvals.Length];
        //    for (int i = 0; i < xvals.Length; i++)
        //        zvals[i] = BilinearInterpolation(x, y, z, xvals[i], yvals[i]);
        //    return zvals;
        //}
    }
}
