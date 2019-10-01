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
    /// Логика взаимодействия для IGEsPage.xaml
    /// </summary>
    public partial class IGEsPage : Page
    {
        private ViewModel vm;
        public IGEsPage()
        {
            InitializeComponent();
            vm = App.Current.MainWindow.DataContext as ViewModel;
            DataContext = vm;
        }

        private void ToCSV_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveIGEsInCsv();
        }

        private void FromCSV_Click(object sender, RoutedEventArgs e)
        {
            vm.ReadIGEsFromCsv();
        }
    }
}
