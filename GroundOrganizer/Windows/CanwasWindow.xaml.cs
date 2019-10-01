using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroundOrganizer
{
    /// <summary>
    /// Логика взаимодействия для CanwasWindow.xaml
    /// </summary>
    public partial class CanwasWindow : Window
    {
        ViewModel vm;
        object src = new object();
        double d = 1;
        public CanwasWindow()
        {
            InitializeComponent();
            DataContext = App.Current.MainWindow.DataContext;
            vm = DataContext as ViewModel;
            drawArea.MouseDown += DrawArea_MouseDown;
            drawArea.MouseWheel += DrawArea_MouseWheel;
        }

        private void DrawArea_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (!e.Source.Equals(sender)) return;
            Canvas da = drawArea;
            d -= e.Delta * 0.001;
            Point pos = e.GetPosition(drawArea);
            double factor = 1 - d * 0.1;
            //if (drawArea.Height >= drawArea.MinHeight) drawArea.Height = drawArea.MinHeight * factor;
            //if (drawArea.Width >= drawArea.MinWidth) drawArea.Width = drawArea.MinWidth * factor;
            ScaleTransform scale = new ScaleTransform(factor, factor, pos.X, pos.Y);
            
            foreach (UIElement item in da.Children) item.RenderTransform = scale;
            drawArea.RenderTransform = scale;
        }

        private void DrawArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (!e.Source.Equals(sender)) return;

                foreach (Polygon item in drawArea.Children)
                {
                    item.Fill = Brushes.White;
                    item.Stroke = Brushes.Black;
                    item.StrokeThickness = 2;
                }
                vm.BufferFoundations.Clear();
                src = new object();
            }
        }

        public void EventMouseEnter()
        {          
            foreach (UIElement item in drawArea.Children)
            {
                item.MouseEnter += Item_MouseEnter;
                item.MouseLeave += Item_MouseLeave;
                item.MouseDown += Item_MouseDown;
            }
        }

        //private void DrawArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    foreach (Polygon item in drawArea.Children) item.Fill = Brushes.White;
        //    vm.BufferFoundations.Clear();
        //}

        private void Item_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Polygon poly = new Polygon();
            int i = 0;
            if (e.Source.GetType() == poly.GetType())
            {
                src = e.Source;
                i = drawArea.Children.IndexOf(sender as UIElement);
                foreach (Polygon item in drawArea.Children)
                {
                    if (item == src) continue;
                    //item.Fill = Brushes.White;
                    item.Stroke = Brushes.Black;
                    item.StrokeThickness = 2;                   
                }
                poly = (Polygon)src;
                poly.Stroke = Brushes.Red;
                poly.StrokeThickness = 3;
                poly.Fill = Brushes.LightGoldenrodYellow;
                vm.SelectedFoundation = vm.ListFoundation[i];
                vm.BufferFoundations.Add(vm.ListFoundation[i]);
            }
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            if (src.Equals(e.Source)) return;
            Polygon poly = new Polygon();

            if (sender.GetType() == poly.GetType())
            {
                poly = (Polygon)sender;
                poly.Stroke = Brushes.Black;
                poly.StrokeThickness = 2;
            }

            Rectangle rre = new Rectangle();

            if (sender.GetType() == rre.GetType())
            {
                rre = (Rectangle)sender;
                rre.Stroke = Brushes.Black;
                rre.StrokeThickness = 2;
            }

        }

        private void Item_MouseEnter(object sender, MouseEventArgs e)
        {
            if (src.Equals(e.Source)) return;
            Polygon poly = new Polygon();
            
            if (e.Source.GetType() == poly.GetType())
            {
                poly = (Polygon)e.Source;
                poly.Stroke = Brushes.Blue;
                poly.StrokeThickness = 3;

                //if (vm.SelectedFoundation == null) return;
                //StackPanel toolTipPanel = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Horizontal, Background = Brushes.Bisque, Opacity = 0.8 };
                //toolTipPanel.Children.Add(new TextBlock { Text = "Марка ", VerticalAlignment = VerticalAlignment.Center });
                //toolTipPanel.Children.Add(new TextBlock { Text = vm.SelectedFoundation.Name, FontSize = 16, VerticalAlignment = VerticalAlignment.Center });
                //popup1.Child = toolTipPanel;
                //popup1.IsOpen = true;
            }

            Rectangle rre = new Rectangle();

            if (sender.GetType() == rre.GetType())
            {
                rre = (Rectangle)sender;
                rre.Stroke = Brushes.Red;
                rre.StrokeThickness = 4;
            }
        }

        private void MenuItem_DrowNumbers_Click(object sender, RoutedEventArgs e)
        {
            vm.DrowFoundsNums();
        }
    }
}
