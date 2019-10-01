using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        private RelayCommand helpFoundation;
        private RelayCommand helpLoads;
        public RelayCommand HelpFoundation
        {
            get { return helpFoundation ?? (helpFoundation = new RelayCommand(obj => { HelpFoundAlert(); })); }
        }
        public RelayCommand HelpLoads
        {
            get { return helpLoads ?? (helpLoads = new RelayCommand(obj => { HelpLoadsAlert(); })); }
        }

        void HelpFoundAlert()
        {
            Alert(Properties.Resources.ShemaFound, 500);
            if (MW.OwnedWindows.Count > 0)
            {
                foreach (Window item in MW.OwnedWindows) item.Close();
                Alert(Properties.Resources.ShemaFound, 500);
            }
        }

        void HelpLoadsAlert()
        {
            Alert(@"\Images\ShemaLoads.png", 200);
            if (MW.OwnedWindows.Count > 0)
            {
                foreach (Window item in MW.OwnedWindows) item.Close();
                Alert(@"\Images\ShemaLoads.png", 200);
            }
        }

        void Alert(string alert)
        {
            AlertWindow aw = new AlertWindow();
            aw.Owner = MW;
            aw.alertLabel.Content = alert;
            //aw.WindowStyle = WindowStyle.None;
            aw.ShowDialog();
        }
        void Alert(string alert, string title)
        {
            AlertWindow aw = new AlertWindow();
            aw.Owner = MW;
            aw.alertLabel.Content = alert;
            aw.Title = title;
            aw.ShowDialog();
        }
        void Alert(Bitmap resource, int h = 250)
        {
            ImageWindow aw = new ImageWindow();
            //BitmapImage b =new BitmapImage(new Uri("pack://application:,,,/Resources/ShemaFound.png"));
            Bitmap br = resource;
            BitmapSource b = Imaging.CreateBitmapSourceFromHBitmap(br.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            aw.alertImage.Source = b;
            aw.alertImage.Height = h;
            aw.Owner = MW;
            aw.WindowStartupLocation = WindowStartupLocation.Manual;
            aw.Top = MW.Top;
            aw.Left = MW.Left + MW.Width;
            aw.Show();
        }

        void Alert(string filepath, int h = 250)
        {
            Uri uri = new Uri(filepath, UriKind.RelativeOrAbsolute);
            BitmapImage b = new BitmapImage(uri);
            ImageWindow aw = new ImageWindow();
            aw.alertImage.Source = b;
            aw.alertImage.Height = h;
            aw.Owner = MW;
            aw.WindowStartupLocation = WindowStartupLocation.Manual;
            aw.Top = MW.Top;
            aw.Left = MW.Left + MW.Width;
            aw.Show();
        }
    }
}
