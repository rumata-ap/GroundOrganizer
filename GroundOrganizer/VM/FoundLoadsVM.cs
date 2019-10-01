using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        private RelayCommand addFoundLoad;
        private RelayCommand updateFoundLoad;
        private RelayCommand renumFoundLoad;
        private RelayCommand exportFoundLoadsToCSV;
        private RelayCommand importFoundLoadsFromCSV;

        int numFoundLoad = 1;
        string nameFoundLoad;
        FoundLoad selectedFoundLoad;
        ObservableCollection<FoundLoad> listFoundLoad = new ObservableCollection<FoundLoad>();

        public ObservableCollection<FoundLoad> ListFoundLoad { get => listFoundLoad; set { listFoundLoad = value; OnPropertyChanged(); } }

        
        public RelayCommand AddFoundLoad
        {
            get { return addFoundLoad ?? (addFoundLoad = new RelayCommand(obj => { AddFoundLoadToList(); })); }
        }
        
        public RelayCommand UpdateFoundLoads
        {
            get { return updateFoundLoad ?? (updateFoundLoad = new RelayCommand(obj => { UpdateFoundLoadInList(); })); }
        }               

        public RelayCommand RenumFoundLoads
        {
            get { return renumFoundLoad ?? (renumFoundLoad = new RelayCommand(obj => { RenumberingFoundLoadInList(); })); }
        }

        public RelayCommand ExportFoundLoadsToCSV
        {
            get { return exportFoundLoadsToCSV ?? (exportFoundLoadsToCSV = new RelayCommand(obj => { SaveLoadsInXlsx(); })); }
        }

        public RelayCommand ImportFoundLoadsFromCSV
        {
            get { return importFoundLoadsFromCSV ?? (importFoundLoadsFromCSV = new RelayCommand(obj => { ReadLoadsFromXlsx(); })); }
        }
        public FoundLoad SelectedFoundLoad { get => selectedFoundLoad; set { selectedFoundLoad = value; OnPropertyChanged(); ChangeSelectedFoundLoad(); } }

        public string NameFoundLoad { get => nameFoundLoad; set { nameFoundLoad = value; OnPropertyChanged(); } }

        void AddFoundLoadToList()
        {
            ListFoundLoad.Add(new FoundLoad() { Number = numFoundLoad }); numFoundLoad++;
            //CreateListNumberTypeBet();
            //NameFoundLoad = listFoundLoad[0].Name;
        }
        void RenumberingFoundLoadInList()
        {
            int j = 1;
            foreach (FoundLoad item in ListFoundLoad)
            {
                item.Number = j;
                j++;
            }
            numFoundLoad = j;
            ListFoundLoad = new ObservableCollection<FoundLoad>(ListFoundLoad);
            SaveDB();
        }
        internal void UpdateFoundLoadInList()
        {
            ListFoundLoad = new ObservableCollection<FoundLoad>(ListFoundLoad);
            SaveDB();
        }

        void ChangeSelectedFoundLoad()
        {
            if (listFoundLoad.Count == 0 || selectedFoundLoad == null) return;
            NameFoundLoad = SelectedFoundLoad.Number.ToString();
        }

        //Сохранение таблицы нагрузок в файл *.xlsx
        internal void SaveLoadsInXlsx()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.xlsx";
            sfd.Filter = "Файл Excel (*.xlsx)|*.xlsx";
            sfd.Title = "Сохранение таблицы";
            //sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.ShowDialog();

            if (sfd.FileName == null || sfd.FileName == "") return;

            try
            {
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


                using (var p = new ExcelPackage(new FileInfo(sfd.FileName)))
                {
                    for (int i = 0; i < listFoundation.Count; i++)
                    {
                        List<object[]> content = new List<object[]>();
                        foreach (FoundLoad fli in listFoundation[i].Loads)
                        {
                            content.Add(fli.PropsToList().ToArray());
                        }
                        ExcelPackage p1 = new ExcelPackage(new FileInfo("ИГЭ.xlsx"));
                        //A workbook must have at least on cell, so lets add one... 
                        var ws = p1.Workbook.Worksheets["Нагрузки"];
                        p.Workbook.Worksheets.Add((i + 1).ToString(), ws);
                        var wsi = p.Workbook.Worksheets[i + 1];
                        //To set values in the spreadsheet use the Cells indexer.
                        wsi.Cells["D1"].Value = listFoundation[i].Number;
                        wsi.Cells["F1"].Value = listFoundation[i].Name;
                        wsi.Cells["A3"].LoadFromArrays(content);
                    }

                    //Save the new workbook. We haven't specified the filename so use the Save as method.
                    p.SaveAs(new FileInfo(sfd.FileName));
                }                 
            }
            catch (Exception e)
            {
                Alert(e.Message);
            }
        }

        //Чтение нагрузок из файла *.xlsx
        internal void ReadLoadsFromXlsx()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.xlsx";
            ofd.Filter = "Файл Excel (*.xlsx)|*.xlsx";
            ofd.Title = "Заполнение таблиц";
            ofd.ShowDialog();

            if (ofd.FileName == null || ofd.FileName == "") return;

            //ListFoundLoad.Clear();
            try
            {
                using (var p = new ExcelPackage(new FileInfo(ofd.FileName)))
                {
                    int k = 0;
                    foreach  (ExcelWorksheet wsi in p.Workbook.Worksheets)
                    {
                        listFoundation[k].Loads = new ObservableCollection<FoundLoad>();

                        object[,] content = wsi.Cells.Value as object[,];
                        List<object> source; FoundLoad item;
                        for (int i = 2; i < content.GetLength(0); i++)
                        {
                            source = new List<object>();
                            for (int j = 0; j < content.GetLength(1); j++)
                            {
                                source.Add(content[i, j]);
                            }
                            item = new FoundLoad(); item.ListToProps(source);
                            listFoundation[k].Loads.Add(item);
                        }
                        k++;
                    }
                }
            }
            catch (Exception e)
            {
                Alert(e.Message);
            }
        }
    }
}
