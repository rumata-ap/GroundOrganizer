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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroundOrganizer
{
    /// <summary>
    /// Логика взаимодействия для BoresPage.xaml
    /// </summary>
    public partial class BoresPage : Page
    {
        private ViewModel vm;
        public BoresPage()
        {
            InitializeComponent();
            DataContext = App.Current.MainWindow.DataContext;
            vm = DataContext as ViewModel;
        }
    }
}
