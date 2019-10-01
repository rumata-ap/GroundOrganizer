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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls.Ribbon;

namespace GroundOrganizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = DataContext as ViewModel;
            Top = 75;
            Left = 75;
            double ws = SystemParameters.PrimaryScreenWidth;
            double hs = SystemParameters.PrimaryScreenHeight;
            MaxWidth = ws - Left - 5;
            MaxHeight = hs - Top - 40;
            //this.SizeToContent = SizeToContent.Height;
            Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Properties.Settings.Default.BasePath) == true)
            {
                // создаем объект BinaryFormatter
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(Properties.Settings.Default.BasePath, FileMode.OpenOrCreate))
                {
                    ViewModel vm = (ViewModel)DataContext;
                    //ToSerializ ser = (ToSerializ)formatter.Deserialize(fs);
                    vm.ListPlayGround = formatter.Deserialize(fs) as ObservableCollection<PlayGround>;
                    DataContext = vm;
                }
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            vm.ReadDB();
        }

        //private void MainRibbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if(contentFrame!=null) contentFrame.Content = null;
        //    if (HomeTab.IsSelected && contentFrame != null) contentFrame.Content = new MainPage();
        //}
    }
}
