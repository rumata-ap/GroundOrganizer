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
        private RelayCommand addLayer;
        private RelayCommand updateLayer;
        private RelayCommand renumLayer;
        private RelayCommand recalcLayerH;
        private RelayCommand recalcLayerDown;

        int numLayer = 1;
        ObservableCollection<Layer> listLayer = new ObservableCollection<Layer>();

        public ObservableCollection<Layer> ListLayer { get => listLayer; set { listLayer = value; OnPropertyChanged(); } }
        
        public RelayCommand AddLayer
        {
            get { return addLayer ?? (addLayer = new RelayCommand(obj => { AddLayerToList(); })); }
        }
       
        public RelayCommand UpdateLayers
        {
            get { return updateLayer ?? (updateLayer = new RelayCommand(obj => { UpdateLayerInList(); })); }
        }
       
        public RelayCommand RenumLayers
        {
            get { return renumLayer ?? (renumLayer = new RelayCommand(obj => { RenumberingLayerInList(); })); }
        }
        
        public RelayCommand RecalcLayerH
        {
            get { return recalcLayerH ?? (recalcLayerH = new RelayCommand(obj => { RecalcInLayersH(); })); }
        }
       
        public RelayCommand RecalcLayerDown
        {
            get { return recalcLayerDown ?? (recalcLayerDown = new RelayCommand(obj => { RecalcInLayersDown(); })); }
        }

        void AddLayerToList()
        {
            ListLayer.Add(new Layer() { Number = numLayer }); numLayer++;
            //CreateListNumberTypeBet();
            //NameLayer = listLayer[0].Name;
        }
        void RenumberingLayerInList()
        {
            int j = 1;
            foreach (Layer item in listLayer)
            {
                item.Number = j;
                j++;
            }
            numLayer = j;
            ListLayer = new ObservableCollection<Layer>(listLayer);
        }
        void UpdateLayerInList()
        {
            foreach (Layer lay in listLayer)
            {
                foreach (IGE ige in listIGE)
                {
                    if (lay.NumIGE==ige.NumIGE)
                    {
                        lay.IGE = ige.Clone();
                        lay.Description = ige.Description;
                        break;
                    }
                }
            }
            ListLayer = new ObservableCollection<Layer>(listLayer);
            selectedBore.Layers = listLayer;
            SaveDB();
        }

        void RecalcInLayersH()
        {
            if (listLayer == null) return;
            if (listLayer.Count == 0) return;
            if (selectedBore == null) return;

            UpdateLayerInList();

            double down = 0;
            double up = 0;
            double z = selectedBore.Z;
            foreach (Layer item in listLayer)
            {
                z -= item.H;
                down += item.H;
                item.Down = down;
                item.Z = z;
                item.Up = up;                                             
                up += item.H;
            }
            ListLayer = new ObservableCollection<Layer>(listLayer);
        }

        void RecalcInLayersDown()
        {
            if (listLayer == null) return;
            if (listLayer.Count == 0) return;
            if (selectedBore == null) return;

            UpdateLayerInList();

            double up = 0;
            double z = selectedBore.Z;
            foreach (Layer item in listLayer)
            {
                item.Up = up;
                item.H = item.Down - item.Up;
                z -= item.H;
                item.Z = z;               
                up += item.H;              
            }
            ListLayer = new ObservableCollection<Layer>(listLayer);
        }

        //Сохранение таблиц наборов слоев в файл *.xlsx
        internal void SaveLayersInXlsx()
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
                        foreach (Layer fli in listBore[i].Layers)
                        {
                            content.Add(fli.PropsToList().ToArray());
                        }
                        ExcelPackage p1 = new ExcelPackage(new FileInfo("ИГЭ.xlsx"));
                        //A workbook must have at least on cell, so lets add one... 
                        var ws = p1.Workbook.Worksheets["Слои"];
                        p.Workbook.Worksheets.Add((i + 1).ToString(), ws);
                        var wsi = p.Workbook.Worksheets[i + 1];
                        //To set values in the spreadsheet use the Cells indexer.
                        wsi.Cells["D1"].Value = listBore[i].Name;
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

        //Чтение наборов слоев из файла *.xlsx
        internal void ReadLayersFromXlsx()
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
                    foreach (ExcelWorksheet wsi in p.Workbook.Worksheets)
                    {
                        listBore[k].Layers = new ObservableCollection<Layer>();

                        object[,] content = wsi.Cells.Value as object[,];
                        List<object> source; Layer item;
                        for (int i = 2; i < content.GetLength(0); i++)
                        {
                            source = new List<object>();
                            for (int j = 0; j < content.GetLength(1); j++)
                            {
                                source.Add(content[i, j]);
                            }
                            item = new Layer(); item.ListToProps(source);
                            listBore[k].Layers.Add(item);
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
