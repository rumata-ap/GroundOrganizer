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
using TriangleNet.Geometry;
using TriangleNet.Meshing;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        private RelayCommand addBore;
        private RelayCommand renumBore;
        private RelayCommand updateBore;
        private RelayCommand createBoresMesh;
        private RelayCommand importBoresFromXLSX;
        private RelayCommand exportBoresToXLSX;

        int numBore = 1;
        string nameBore;
        private string boresNote;
        Bore selectedBore;
        ObservableCollection<Bore> listBore = new ObservableCollection<Bore>();
        public string BoresNote { get => boresNote; set { boresNote = value; OnPropertyChanged(); } }

        public Bore SelectedBore { get => selectedBore; set { selectedBore = value; OnPropertyChanged(); ChangeSelectedBore(); } }

        public string NameBore { get => nameBore; set { nameBore = value; OnPropertyChanged(); } }

        public ObservableCollection<Bore> ListBore { get => listBore; set { listBore = value; OnPropertyChanged(); } }
        public TriangleNet.Mesh MeshBores { get; private set; }

        
        public RelayCommand AddBore
        {
            get { return addBore ?? (addBore = new RelayCommand(obj => { AddBoreToList(); })); }
        }
        
        public RelayCommand UpdateBores
        {
            get { return updateBore ?? (updateBore = new RelayCommand(obj => { UpdateBoreInList(); })); }
        }             

        public RelayCommand RenumBores
        {
            get { return renumBore ?? (renumBore = new RelayCommand(obj => { RenumberingBoreInList(); })); }
        }

        public RelayCommand CreateBoresMesh
        {
            get { return createBoresMesh ?? (createBoresMesh = new RelayCommand(obj => { CreateMeshBores(); })); }
        }
      
        public RelayCommand ExportBoresToXLSX
        {
            get { return exportBoresToXLSX ?? (exportBoresToXLSX = new RelayCommand(obj => { SaveBoresInXlsx(); })); }
        }
       
        public RelayCommand ImportBoresFromXLSX
        {
            get { return importBoresFromXLSX ?? (importBoresFromXLSX = new RelayCommand(obj => { ReadBoresFromXlsx(); })); }
        }

        //Чтение таблицы набора скважин из файла *.xlsx
        internal void ReadBoresFromXlsx()
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
                var ws = p.Workbook.Worksheets["Скважины"];

                object[,] content = ws.Cells.Value as object[,];
                List<object> source; Bore item;
                for (int i = 1; i < content.GetLength(0); i++)
                {
                    source = new List<object>();
                    for (int j = 0; j < content.GetLength(1); j++)
                    {
                        source.Add(content[i, j]);
                    }
                    item = new Bore(); item.ListToProps(source);
                    ListBore.Add(item);
                }
            }
        }

        //Сохранение таблицы набора ИГЭ в файл *.csv
        internal void SaveBoresInXlsx()
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
            foreach (Bore item in listBore)
            {
                content.Add(item.PropsToList().ToArray());
            }

            using (var p = new ExcelPackage(new FileInfo(sfd.FileName)))
            {
                try
                {
                    var ws = p.Workbook.Worksheets["Скважины"];

                    //To set values in the spreadsheet use the Cells indexer.
                    ws.Cells["A2"].LoadFromArrays(content);

                    //Save the new workbook. We haven't specified the filename so use the Save as method.
                    p.SaveAs(new FileInfo(sfd.FileName));
                }
                catch
                {
                    ExcelPackage p1 = new ExcelPackage(new FileInfo("ИГЭ.xlsx"));
                    ExcelWorksheet ws1 = p1.Workbook.Worksheets["Скважины"];
                    p.Workbook.Worksheets.Add("Скважины", ws1);
                    ExcelWorksheet ws = p.Workbook.Worksheets["Скважины"];

                    //Save the new workbook. We haven't specified the filename so use the Save as method.
                    ws.Cells["A2"].LoadFromArrays(content);
                    ws.Select();
                    p.SaveAs(new FileInfo(sfd.FileName));
                }
            }
        }

        private void CreateMeshBores()
        {
            if (listBore == null || listBore.Count == 0) return;

            Polygon polygon = new Polygon();
            Vertex vrtx;
            int i = 1;
            foreach (Bore item in listBore)
            {
                vrtx = new Vertex(item.X, item.Y, item.Number, 2);
                polygon.Add(vrtx);
                i++;
            }
            
            ConstraintOptions constraint = new ConstraintOptions() { Convex = true };            
            MeshBores = (TriangleNet.Mesh)polygon.Triangulate(constraint);
            Alert("Триангуляционная сеть скважин создана");
        }

        void AddBoreToList()
        {
            ListBore.Add(new Bore() { Number = numBore }); numBore++;
            //CreateListNumberTypeBet();
            //NameBore = listBore[0].Name;
        }
        void RenumberingBoreInList()
        {
            int j = 1;
            foreach (Bore item in ListBore)
            {
                item.Number = j;
                j++;
            }
            numBore = j;
            ListBore = new ObservableCollection<Bore>(ListBore);
        }
        void UpdateBoreInList()
        {
            ListBore = new ObservableCollection<Bore>(ListBore);
            SaveDB();
        }

        void ChangeSelectedBore()
        {
            if (listBore.Count == 0 || selectedBore == null) return;
            NameBore = selectedBore.Name;
            ListLayer = selectedBore.Layers;
        }
    }
}
