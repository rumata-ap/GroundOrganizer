using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        string title;
        ToSerializ ser = new ToSerializ();
        MainWindow MW;
        //MainPage MP;
        private string basePath;

        public string BasePath { get => basePath; set { basePath = value; OnPropertyChanged(); } }

        public ViewModel()
        {
            title = "GroundOrganizer";
            MW = App.Current.MainWindow as MainWindow;           
            //MW.contentFrame.Content = MP;
            MW.Title = title;
            BasePath = Properties.Settings.Default.BasePath;
            StructureNote = "к числу зданий и сооружений с жесткой конструктивной схемой относятся:" +
                "\n- здания панельные, блочные и кирпичные, в которых междуэтажные" +
                "\nперекрытия опираются по всему контуру на поперечные и продольные" +
                "\nстены или только на поперечные несущие стены при малом их шаге;" +
                "\n- сооружения типа башен, силосных корпусов, дымовых труб, домен и др.";
            BoresNote = "* положительное значение отметки уровня грунтовых вод относительно устья скважины";
            
        }

        private RelayCommand openGroundBase;
        public RelayCommand OpenGroundBase
        {
            get { return openGroundBase ?? (openGroundBase = new RelayCommand(obj => { ReadDB(); })); }
        }

        public string Title { get => title; set => title = value; }

        string GetGroundBaseFile()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = "*.*";
            ofd.Filter = "База данных инженерно-геологических площадок (*.grndb)|*.grndb|Все файлы (*.*)|*.*";
            ofd.Title = "Выбор файла с базой данных инженерно-геологических площадок";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        internal void SaveDB()
        {
            //ser = new ToSerializ { PlayGroundList = ListPlayGround };
            // создаем объект BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(Properties.Settings.Default.BasePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, ListPlayGround);
            }
            //Alert("Введенные данные успешно сохранены");
        }

        internal void ReadDB()
        {
            Properties.Settings.Default.BasePath = GetGroundBaseFile();
            Properties.Settings.Default.Save();
            BasePath = Properties.Settings.Default.BasePath;
            if (File.Exists(Properties.Settings.Default.BasePath) == true)
            {
                // создаем объект BinaryFormatter
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fs = new FileStream(Properties.Settings.Default.BasePath, FileMode.OpenOrCreate))
                {
                    //ToSerializ ser = (ToSerializ)formatter.Deserialize(fs);
                    ListPlayGround = formatter.Deserialize(fs) as ObservableCollection<PlayGround>;
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
