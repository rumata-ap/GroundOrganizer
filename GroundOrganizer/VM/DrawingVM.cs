using netDxf;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        private RelayCommand drowFoundations;
        private RelayCommand showFoundProps;

        CanwasWindow cw;

        public RelayCommand DrowFoundations
        {
            get { return drowFoundations ?? (drowFoundations = new RelayCommand(obj => { DrowFounds(); })); }
        }

        public RelayCommand ShowFoundProps
        {
            get { return showFoundProps ?? (showFoundProps = new RelayCommand(obj => { ShowFoundPropsWindow(); })); }
        }

        internal void ShowFoundPropsWindow()
        {
            if (listFoundation.Count == 0) return;
            if (listFoundation == null) return;

            FoundPropWindow fpw = new FoundPropWindow();
            fpw.Owner = MW;
            fpw.DataContext = MW.DataContext;
            fpw.Top = 75;
            fpw.Left = 75;
            fpw.ShowDialog();
        }

        internal void DrowFounds()
        {
            if (listFoundation.Count == 0) return;
            if (listFoundation == null) return;

            CalculateContoursFounds();
            CreateSmallPropsFounds();
            int t = 75;
            int l = 75;
            double ws = SystemParameters.PrimaryScreenWidth;
            double hs = SystemParameters.PrimaryScreenHeight;
            cw = new CanwasWindow();
            cw.Top = t;
            cw.Left = l;
            cw.MaxHeight= hs - t - 37;
            cw.MaxWidth= ws - l - 5;
            cw.CanvasScrollViewer.MaxWidth = cw.MaxWidth;
            cw.CanvasScrollViewer.MaxHeight = cw.MaxHeight-60;
            cw.Owner = MW;
            CanvasDrafter.DrawFoundations(listFoundation, cw.drawArea, cw.MaxHeight - 120);
            cw.drawArea.MinHeight = cw.drawArea.Height;
            cw.drawArea.MinWidth = cw.drawArea.Width;
            cw.EventMouseEnter();
            cw.Show();
        }

        internal void DrowFoundsNums()
        {
            if (cw == null || cw.IsActive == false) return;
            CanvasDrafter.DrawFoundationsNumbers(listFoundation, cw.drawArea);
        }
    }
}
