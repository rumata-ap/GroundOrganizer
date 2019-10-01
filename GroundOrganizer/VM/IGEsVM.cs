using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        int numIGE = 1;
        ObservableCollection<IGE> listIGE = new ObservableCollection<IGE>();

        public ObservableCollection<IGE> ListIGE { get => listIGE; set { listIGE = value; OnPropertyChanged(); } }

        private RelayCommand addIGE;
        public RelayCommand AddIGE
        {
            get { return addIGE ?? (addIGE = new RelayCommand(obj => { AddIGEToList(); })); }
        }

        private RelayCommand updateIGE;
        public RelayCommand UpdateIGEs
        {
            get { return updateIGE ?? (updateIGE = new RelayCommand(obj => { UpdateIGEInList(); })); }
        }

        private RelayCommand renumIGE;
        public RelayCommand RenumIGEs
        {
            get { return renumIGE ?? (renumIGE = new RelayCommand(obj => { RenumberingIGEInList(); })); }
        }

        private RelayCommand exportIGEtoCSV;
        public RelayCommand ExportIGEtoCSV
        {
            get { return exportIGEtoCSV ?? (exportIGEtoCSV = new RelayCommand(obj => { SaveIGEsInCsv(); })); }
        }

        private RelayCommand importIGEfromCSV;
        public RelayCommand ImportIGEfromCSV
        {
            get { return importIGEfromCSV ?? (importIGEfromCSV = new RelayCommand(obj => { ReadIGEsFromCsv(); })); }
        }

        private RelayCommand saveIGEsTable;
        public RelayCommand SaveIGEsTable
        {
            get { return saveIGEsTable ?? (saveIGEsTable = new RelayCommand(obj => { SaveIGEs(); })); }
        }

        private IEnumerable<String> listNumIGEs;
        public IEnumerable<String> ListNumIGEs { get => listNumIGEs; set { listNumIGEs = value; OnPropertyChanged(); } }

        void AddIGEToList()
        {
            ListIGE.Add(new IGE() { Number = numIGE }); numIGE++;
            CreateListNumIGEs();
            //CreateListNumberTypeBet();
            //NameIGE = listIGE[0].Name;
        }
        void RenumberingIGEInList()
        {
            int j = 1;
            foreach (IGE item in ListIGE)
            {
                item.Number = j;
                j++;
            }
            numIGE = j;
            ListIGE = new ObservableCollection<IGE>(ListIGE);
            CreateListNumIGEs();
        }
        void UpdateIGEInList()
        {
            ListIGE = new ObservableCollection<IGE>(ListIGE);
            SaveDB();
        }

        void CreateListNumIGEs()
        {
            if (listIGE == null || listPlayGround == null) return;
            ListNumIGEs = from bet in listIGE select bet.NumIGE;
            try
            {
                BoresPage boresPage = (BoresPage)MW.BoresFrame.Content;
                boresPage.IGEsListCbxCol.ItemsSource = listNumIGEs;
            }
            catch { return; }
        }

        //Сохранение таблицы набора ИГЭ в файл *.csv
        internal void SaveIGEsInCsv()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.xlsx";
            sfd.Filter = "Файл Excel (*.xlsx)|*.xlsx";
            sfd.Title = "Сохранение таблицы";
            //sfd.AddExtension = true;
            sfd.OverwritePrompt = false;
            sfd.ShowDialog();

            if (sfd.FileName == null || sfd.FileName == "") return;

            string path = "ИГЭ.xlsx";
            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists != true)
            {
                using (FileStream fstream = new FileStream("ИГЭ.xlsx", FileMode.OpenOrCreate))
                {
                    byte[] array = Properties.Resources.IGE;
                    fstream.Write(array, 0, array.Length);
                }
            }
            List<object[]> content = new List<object[]>();
            foreach (IGE item in listIGE)
            {
                content.Add(item.PropsToList().ToArray());
            }

            using (var p = new ExcelPackage(new FileInfo(sfd.FileName)))
            {
                try
                {
                    var ws = p.Workbook.Worksheets["ИГЭ"];

                    //To set values in the spreadsheet use the Cells indexer.
                    ws.Cells["A2"].LoadFromArrays(content);

                    //Save the new workbook. We haven't specified the filename so use the Save as method.
                    p.SaveAs(new FileInfo(sfd.FileName));
                }
                catch
                {                                      
                    ExcelPackage p1 = new ExcelPackage(new FileInfo("ИГЭ.xlsx"));
                    ExcelWorksheet ws1 = p1.Workbook.Worksheets["ИГЭ"];
                    p.Workbook.Worksheets.Add("ИГЭ", ws1);
                    ExcelWorksheet ws = p.Workbook.Worksheets["ИГЭ"];

                    //Save the new workbook. We haven't specified the filename so use the Save as method.
                    ws.Cells["A2"].LoadFromArrays(content);
                    ws.Select();
                    p.SaveAs(new FileInfo(sfd.FileName));
                }
            }

            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.Default))
            //    {
            //        foreach (IGE item in listIGE)
            //        {
            //            sw.WriteLine(item.PropsToString());
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Alert(e.Message);
            //}
        }

        //Чтение таблицы набора ИГЭ из файла *.csv
        internal void ReadIGEsFromCsv()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.xlsx";
            ofd.Filter = "Файл Excel (*.xlsx)|*.xlsx";
            ofd.Title = "Заполнение таблицы";
            ofd.ShowDialog();

            if (ofd.FileName == null || ofd.FileName == "") return;

            //ListIGE.Clear();

            using (var p = new ExcelPackage(new FileInfo(ofd.FileName)))
            {
                //A workbook must have at least on cell, so lets add one... 
                var ws = p.Workbook.Worksheets["ИГЭ"];

                object[,] content = ws.Cells.Value as object[,];
                List<object> source; IGE item;
                for (int i = 1; i < content.GetLength(0); i++)
                {
                    source = new List<object>();
                    for (int j = 0; j < content.GetLength(1); j++)
                    {
                        source.Add(content[i, j]);
                    }
                    item = new IGE(); item.ListToProps(source);
                    ListIGE.Add(item);
                }
            }

            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.DefaultExt = "*.*";
            //ofd.Filter = "Текстовый файл (*.csv)|*.csv|Все файлы (*.*)|*.*";
            //ofd.Title = "Заполнение таблицы";
            //ofd.ShowDialog();

            //if (ofd.FileName == null || ofd.FileName == "") return;

            //ListIGE.Clear();
            //try
            //{
            //    using (StreamReader sr = new StreamReader(ofd.FileName, System.Text.Encoding.Default))
            //    {
            //        string line;
            //        IGE ige;
            //        while ((line = sr.ReadLine()) != null)
            //        {                       
            //            ige = new IGE();
            //            ige.StringToProps(line);
            //            ListIGE.Add(ige);
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Alert(e.Message);
            //}
        }

        internal void SaveIGEs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.igedb";
            sfd.Filter = "Таблица ИГЭ (*.igedb)|*.igedb";
            sfd.Title = "Сохранение таблицы";
            //sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.ShowDialog();

            if (sfd.FileName == null || sfd.FileName == "") return;

            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(sfd.FileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, ListIGE);
            }
            //Alert("Введенные данные успешно сохранены");
        }

        internal void ReadIGEs()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.igedb";
            ofd.Filter = "Таблица ИГЭ (*.igedb)|*.igedb";
            ofd.Title = "Открытие таблицы";
            ofd.ShowDialog();

            if (ofd.FileName == null || ofd.FileName == "") return;

            if (File.Exists(ofd.FileName) == true)
            {
                // создаем объект BinaryFormatter
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate))
                {
                    List<IGE> tmp = new List<IGE>(ListIGE);
                    tmp.AddRange(formatter.Deserialize(fs) as ObservableCollection<IGE>);
                    ListIGE = new ObservableCollection<IGE>(tmp);
                }
            }
        }
    }
}
