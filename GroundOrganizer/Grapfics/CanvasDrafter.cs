using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AForge.Math.Geometry;
using AForge;
using netDxf.Entities;
using Point = System.Windows.Point;

namespace GroundOrganizer
{
    public partial class CanvasDrafter
    {
        //const double WXMIN = -300;
        //const double WXMAX = 300;
        //const double WYMIN = -300;
        //const double WYMAX = 300;
        const double DMARGIN = 0;

        static Matrix WtoDMatrix;
        static Matrix DtoWMatrix;

        static public void DrawPolygonsFromDxfLwPolyline(IEnumerable<LwPolyline> dxfEntitys,Canvas canvas, double h)
        {
            double dxmin = DMARGIN;           
            double dymin = DMARGIN;
            double dymax = h - DMARGIN;

            double wxmin;
            double wxmax;
            double wymin;
            double wymax;

            List<LwPolylineVertex> dxfVertices = new List<LwPolylineVertex>();
            foreach (LwPolyline item in dxfEntitys)
            {
                dxfVertices.AddRange(item.Vertexes);
            }

            IOrderedEnumerable<LwPolylineVertex> selected = from x in dxfVertices // определяем каждый объект как x
                                                             orderby x.Position.X  // упорядочиваем по возрастанию
                                                             select x; // выбираем объект
            wxmin = selected.First().Position.X;
            wxmax = selected.Last().Position.X;

            selected = from y in dxfVertices orderby y.Position.Y select y; 

            wymin = selected.First().Position.Y;
            wymax = selected.Last().Position.Y;

            double w = (wxmax - wxmin) / (wymax - wymin)*h;
            double dxmax = w - DMARGIN;

            PrepareTransformations(wxmin, wxmax, wymin, wymax, dxmin, dxmax, dymax, dymin);

            Canvas da = canvas;
            da.Width = w;
            da.Height = h;

            Polygon contour;
            foreach (LwPolyline item in dxfEntitys)
            {
                dxfVertices = item.Vertexes;
                Point pt;
                contour = new Polygon
                {
                    Points = new PointCollection(),
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Fill = Brushes.White
                };
                foreach (LwPolylineVertex v in dxfVertices)
                {
                    pt = WtoDMatrix.Transform(new Point(v.Position.X, v.Position.Y));
                    contour.Points.Add(pt);
                }
                da.Children.Add(contour);
                //da.Children.Add(new Rectangle() { Width = 80, Height = 80,Stroke=Brushes.Black,StrokeThickness=1,Fill=Brushes.White });
            }
        }

        static public void DrawFoundations(ObservableCollection<Foundation> founds, Canvas canvas, double h)
        {
            double dxmin = DMARGIN;
            double dymin = DMARGIN;
            double dymax = h - DMARGIN;

            double wxmin;
            double wxmax;
            double wymin;
            double wymax;

            List<Geo.Point3d> Vertices = new List<Geo.Point3d>();
            foreach (Foundation item in founds)
            {
                if (item.Contour != null) Vertices.AddRange(item.Contour.Vertices);
            }

            if (Vertices.Count < 4) return;
            IOrderedEnumerable<Geo.Point3d> selected = from x in Vertices // определяем каждый объект как x
                                                            orderby x.X  // упорядочиваем по возрастанию
                                                            select x; // выбираем объект

            wxmin = selected.First().X;
            wxmax = selected.Last().X;

            selected = from y in Vertices orderby y.Y select y;

            wymin = selected.First().Y;
            wymax = selected.Last().Y;

            double w = (wxmax - wxmin) / (wymax - wymin) * h;
            double dxmax = w - DMARGIN;

            PrepareTransformations(wxmin, wxmax, wymin, wymax, dxmin, dxmax, dymax, dymin);

            Canvas da = canvas;
            da.Width = w;
            da.Height = h;

            Polygon contour;
            foreach (Foundation item in founds)
            {
                if (item.Contour == null) continue;
                ToolTip toolTip = new ToolTip();               
                StackPanel toolTipPanel = new StackPanel();
                DataGrid props = new DataGrid() { ItemsSource = item.SmallProps, FontSize = 11 };
                //DataGrid results = new DataGrid() { ItemsSource = item.SmallResults};
                toolTipPanel.Children.Add(props);
                //toolTipPanel.Children.Add(results);
                toolTip.Content = toolTipPanel;
                Vertices = item.Contour.Vertices;
                Point pt;
                contour = new Polygon
                {
                    Points = new PointCollection(),
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Fill = Brushes.White                  
                };
                ToolTipService.SetToolTip(contour, toolTip);
                ToolTipService.SetShowDuration(contour, int.MaxValue);

                foreach (Geo.Point3d v in Vertices)
                {
                    pt = WtoDMatrix.Transform(new Point(v.X, v.Y));
                    contour.Points.Add(pt);
                }
                da.Children.Add(contour);
                
                //da.Children.Add(new Rectangle() { Width = 80, Height = 80,Stroke=Brushes.Black,StrokeThickness=1,Fill=Brushes.White });
            }
        }
        static public void DrawFoundationsNumbers(ObservableCollection<Foundation> founds, Canvas canvas)
        {
            double dxmin = DMARGIN;
            double dymin = DMARGIN;
            double dymax = canvas.ActualHeight - DMARGIN;
            double dxmax = canvas.ActualWidth - DMARGIN;

            double wxmin;
            double wxmax;
            double wymin;
            double wymax;

            List<Geo.Point3d> Vertices = new List<Geo.Point3d>();
            foreach (Foundation item in founds)
            {
                if (item.Contour != null) Vertices.AddRange(item.Contour.Vertices);
            }

            if (Vertices.Count < 4) return;
            IOrderedEnumerable<Geo.Point3d> selected = from x in Vertices // определяем каждый объект как x
                                                       orderby x.X  // упорядочиваем по возрастанию
                                                       select x; // выбираем объект

            wxmin = selected.First().X;
            wxmax = selected.Last().X;

            selected = from y in Vertices orderby y.Y select y;

            wymin = selected.First().Y;
            wymax = selected.Last().Y;            

            PrepareTransformations(wxmin, wxmax, wymin, wymax, dxmin, dxmax, dymax, dymin);

            Canvas da = canvas;

            Label label;
            foreach (Foundation item in founds)
            {
                Point pt = WtoDMatrix.Transform(new Point(item.X, item.Y));
                label = new Label
                {
                    Content = item.Number.ToString(),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };
                Canvas.SetTop(label, pt.Y);
                Canvas.SetLeft(label, pt.X);
                da.Children.Add(label);

                //da.Children.Add(new Rectangle() { Width = 80, Height = 80,Stroke=Brushes.Black,StrokeThickness=1,Fill=Brushes.White });
            }
        }

        static void PrepareTransformations(double wxmin, double wxmax, double wymin, double wymax, double dxmin, double dxmax, double dymin, double dymax)
        {
            // Make WtoD.
            WtoDMatrix = Matrix.Identity;
            WtoDMatrix.Translate(-wxmin, -wymin);

            double xscale = (dxmax - dxmin) / (wxmax - wxmin);
            double yscale = (dymax - dymin) / (wymax - wymin);
            WtoDMatrix.Scale(xscale, yscale);

            WtoDMatrix.Translate(dxmin, dymin);

            // Make DtoW.
            DtoWMatrix = WtoDMatrix;
            DtoWMatrix.Invert();
        }


    }
}
