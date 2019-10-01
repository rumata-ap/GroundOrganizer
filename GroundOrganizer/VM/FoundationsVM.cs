using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Geo;
using Microsoft.Win32;
using netDxf;
using netDxf.Entities;
using OfficeOpenXml;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        private RelayCommand addFoundation;
        private RelayCommand updateFoundation;
        private RelayCommand renumFoundation;
        private RelayCommand importFoundationsFromDXF;
        private RelayCommand replaceD1Found;
        private RelayCommand replaceDLFound;
        private RelayCommand replaceFLFound;
        private RelayCommand replaceDLFoundfromNULL;
        private RelayCommand calculateDLFound;
        private RelayCommand calculateLevelsFounds;
        private RelayCommand exportFoundationsToCSV;
        private RelayCommand importFoundationsFromCSV;

        private double d1;
        private double dL;
        private double fL;

        int numFoundation = 1;
        string nameFoundation;
        Foundation selectedFoundation;
        ObservableCollection<Foundation> listFoundation = new ObservableCollection<Foundation>();
        List<Foundation> bufferFoundations = new List<Foundation>();

        public double D1 { get => d1; set { d1 = value; OnPropertyChanged(); /*ChangeD1();*/ } }
        public double DL { get => dL; set { dL = value; OnPropertyChanged(); /*ChangeDL();*/ } }
        public double FL { get => fL; set { fL = value; OnPropertyChanged(); /*ChangeFL();*/ } }


        public ObservableCollection<Foundation> ListFoundation { get => listFoundation; set { listFoundation = value; OnPropertyChanged(); } }
        public Foundation SelectedFoundation { get => selectedFoundation; set { selectedFoundation = value; OnPropertyChanged(); ChangeSelectedFoundation(); } }
        public string NameFoundation { get => nameFoundation; set { nameFoundation = value; OnPropertyChanged(); ChangeMark(); } }
        public List<TypeFound> ListTypeFounds { get => listTypeFounds; set { listTypeFounds = value; OnPropertyChanged(); } }
        public List<Foundation> BufferFoundations { get => bufferFoundations; set => bufferFoundations = value; }

        #region Команды

        public RelayCommand AddFoundation
        {
            get { return addFoundation ?? (addFoundation = new RelayCommand(obj => { AddFoundationToList(); })); }
        }
                
        public RelayCommand UpdateFoundations
        {
            get { return updateFoundation ?? (updateFoundation = new RelayCommand(obj => { UpdateFoundationInList(); })); }
        }

        public RelayCommand RenumFoundations
        {
            get { return renumFoundation ?? (renumFoundation = new RelayCommand(obj => { RenumberingFoundationInList(); })); }
        }

        public RelayCommand ImportFoundationsFromDXF
        {
            get { return importFoundationsFromDXF ?? (importFoundationsFromDXF = new RelayCommand(obj => { ReadFondationsFromDXF(); })); }
        }

        public RelayCommand ReplaceD1Found
        {
            get { return replaceD1Found ?? (replaceD1Found = new RelayCommand(obj => { ChangeD1(); })); }
        }

        public RelayCommand ReplaceDLFound
        {
            get { return replaceDLFound ?? (replaceDLFound = new RelayCommand(obj => { ChangeDL(); })); }
        }
        
        public RelayCommand ReplaceFLFound
        {
            get { return replaceFLFound ?? (replaceFLFound = new RelayCommand(obj => { ChangeFL(); })); }
        }


        public RelayCommand ReplaceDLFoundfromNULL
        {
            get { return replaceDLFoundfromNULL ?? (replaceDLFoundfromNULL = new RelayCommand(obj => { ChangeDLfromNULL(); })); }
        }
        
        public RelayCommand CalculateDLFound
        {
            get { return calculateDLFound ?? (calculateDLFound = new RelayCommand(obj => { CalcDLFound(); })); }
        }
                
        public RelayCommand CalculateLevelsFounds
        {
            get { return calculateLevelsFounds ?? (calculateLevelsFounds = new RelayCommand(obj => { CalcDLFoundsAll(); CalcNLFoundsAll(); })); }
        }

        public RelayCommand ExportFoundationsToCSV
        {
            get { return exportFoundationsToCSV ?? (exportFoundationsToCSV = new RelayCommand(obj => { SaveFoundationsInCsv(); })); }
        }

        public RelayCommand ImportFoundationsFromCSV
        {
            get { return importFoundationsFromCSV ?? (importFoundationsFromCSV = new RelayCommand(obj => { ReadFoundationsFromCsv(); })); }
        }

        #endregion

        void AddFoundationToList()
        {
            ListFoundation.Add(new Foundation() { Number = numFoundation }); numFoundation++;
            //CreateListNumberTypeBet();
            //NameFoundation = listFoundation[0].Name;
        }
        void RenumberingFoundationInList()
        {
            int j = 1;
            foreach (Foundation item in ListFoundation)
            {
                item.Number = j;
                j++;
            }
            numFoundation = j;
            ListFoundation = new ObservableCollection<Foundation>(ListFoundation);
        }
        internal void UpdateFoundationInList()
        {
            ListFoundation = new ObservableCollection<Foundation>(ListFoundation);
            SaveDB();
        }

        void ChangeSelectedFoundation()
        {
            if (listFoundation.Count == 0 || selectedFoundation == null) return;
            NameFoundation = selectedFoundation.Name;
            ListFoundLoad = selectedFoundation.Loads;
            D1 = selectedFoundation.D1;
            FL = selectedFoundation.FL;
            DL = selectedFoundation.DL;
        }

        void CreateListTypeFoundations()
        {
            if (listFoundation == null || listPlayGround == null) return;
            ListTypeFounds = new List<TypeFound> { TypeFound.Прямоугольный, TypeFound.Круглый, TypeFound.Ленточный };
            try
            {
                FoundationsPage foundationsPage = (FoundationsPage)MW.FoundationsFrame.Content;
                foundationsPage.typeFondsListCbxCol.ItemsSource = ListTypeFounds;
            }
            catch (Exception)
            {
                return;
            }
        }
        private void ReadFondationsFromDXF()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.dxf";
            ofd.Filter = "Чертеж (*.dxf)|*.dxf";
            ofd.Title = "Импорт чертежа";
            ofd.ShowDialog();

            if (ofd.FileName == null || ofd.FileName == "") return;

            DxfDocument dxfDocument = DxfDocument.Load(ofd.FileName);
            IEnumerable<LwPolyline> dxfLwPolylines = dxfDocument.LwPolylines;
            IEnumerable<LwPolylineVertex> dxfLwPolylinesVertexes = null;
            Foundation fnd;
            Quadrangle countFound; int i = 0;
            Point2d[] ptColl = new Point2d[4];
            foreach (LwPolyline item in dxfLwPolylines)
            {
                if (item.Layer.Name == "Foundations")
                {
                    dxfLwPolylinesVertexes = item.Vertexes;
                    int j = 0;
                    foreach (LwPolylineVertex itemVrtx in dxfLwPolylinesVertexes)
                    {
                        ptColl[j] = new Point2d(itemVrtx.Position.X * 0.001, itemVrtx.Position.Y * 0.001);
                        j++; if (j == 4) break;                       
                    }
                    countFound = new Quadrangle(ptColl[0], ptColl[1], ptColl[2], ptColl[3]);
                    countFound.Units = UnitsLin.м;
                    fnd = new Foundation(countFound) { Number = i + 1 };
                    ListFoundation.Add(fnd);
                    i++;
                }              
            }

            if (dxfLwPolylinesVertexes == null) return;
        }

        //Сохранение таблицы фундаментов в файл *.csv
        internal void SaveFoundationsInCsv()
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
            foreach (Foundation item in listFoundation)
            {
                content.Add(item.PropsToList().ToArray());
            }

            using (var p = new ExcelPackage(new FileInfo(sfd.FileName)))
            {
                try
                {
                    //A workbook must have at least on cell, so lets add one... 
                    var ws = p.Workbook.Worksheets["Фундаменты"];

                    //To set values in the spreadsheet use the Cells indexer.
                    ws.Cells["A2"].LoadFromArrays(content);

                    //Save the new workbook. We haven't specified the filename so use the Save as method.
                    p.SaveAs(new FileInfo(sfd.FileName));
                }
                catch 
                {
                    ExcelPackage p1 = new ExcelPackage(new FileInfo("ИГЭ.xlsx"));
                    ExcelWorksheet ws1 = p1.Workbook.Worksheets["Фундаменты"];
                    p.Workbook.Worksheets.Add("Фундаменты", ws1);

                    //A workbook must have at least on cell, so lets add one... 
                    var ws = p.Workbook.Worksheets["Фундаменты"];

                    //To set values in the spreadsheet use the Cells indexer.
                    ws.Cells["A2"].LoadFromArrays(content);
                    ws.Select();
                    //Save the new workbook. We haven't specified the filename so use the Save as method.
                    p.SaveAs(new FileInfo(sfd.FileName));
                }
            }
        }

        //Чтение таблицы фундаментов из файла *.xlsx
        internal void ReadFoundationsFromCsv()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.xlsx";
            ofd.Filter = "Файл Excel (*.xlsx)|*.xlsx";
            ofd.Title = "Заполнение таблицы";
            ofd.ShowDialog();

            if (ofd.FileName == null || ofd.FileName == "") return;

            //ListIGE.Clear();

            try
            {
                using (var p = new ExcelPackage(new FileInfo(ofd.FileName)))
                {
                    //A workbook must have at least on cell, so lets add one... 
                    var ws = p.Workbook.Worksheets["Фундаменты"];

                    object[,] content = ws.Cells.Value as object[,];
                    List<object> source; Foundation item;
                    for (int i = 1; i < content.GetLength(0); i++)
                    {
                        source = new List<object>();
                        for (int j = 0; j < content.GetLength(1); j++)
                        {
                            source.Add(content[i, j]);
                        }
                        item = new Foundation(); item.ListToProps(source);
                        ListFoundation.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                Alert(e.Message);
            }
        }

        private void ChangeMark()
        {
            if (selectedFoundation == null) return;
            SelectedFoundation.Name = nameFoundation;
        }

        void ChangeD1()
        {
            if (selectedFoundation == null || selectedStructure == null) return;
            SelectedFoundation.D1 = d1;
            SelectedFoundation.D1toFL(selectedStructure.Null);
            DL = selectedFoundation.DL;
            //FL = selectedFoundation.FL;
        }

        void ChangeDL()
        {
            if (selectedFoundation == null || selectedStructure == null) return;
            SelectedFoundation.DL = dL;
            SelectedFoundation.DLtoD1(selectedStructure.Null);
            D1 = selectedFoundation.D1;
            //FL = selectedFoundation.FL;
        }

        void ChangeFL()
        {
            if (selectedFoundation == null || selectedStructure == null) return;
            SelectedFoundation.FL = fL;
            SelectedFoundation.FLtoD1(selectedStructure.Null);
            D1 = selectedFoundation.D1;
            //DL = selectedFoundation.DL;
        }

        private void ChangeDLfromNULL()
        {
            if (selectedFoundation == null || selectedStructure == null) return;
            SelectedFoundation.DL = selectedStructure.Null;
            DL = SelectedFoundation.DL;
            SelectedFoundation.DLtoD1(selectedStructure.Null);
            D1 = selectedFoundation.D1;
        }

        private void CalcDLFound()
        {
            if (selectedFoundation == null || selectedStructure == null) return;
            if (MeshRedPlanning == null) CreateRedMesh();
            selectedFoundation.CalcDL(MeshRedPlanning);
            DL = SelectedFoundation.DL;
            SelectedFoundation.DLtoD1(selectedStructure.Null);
            D1 = selectedFoundation.D1;
        }

        internal void CalcDLFoundsAll()
        {
            if (listFoundation == null || listFoundation.Count == 0) return;
            if (MeshRedPlanning == null) CreateRedMesh();
            if (MeshRedPlanning == null)
            {
                Alert("Сооружение не содержит планировочных отметок");
                return;
            }
            
            foreach (Foundation item in ListFoundation)
            {
                item.CalcDL(MeshRedPlanning);
                item.DLtoD1(selectedStructure.Null);
            }

            Alert("Уровни планировки для всех фундаментов \nвычислены по триангуляционной сети");
        }

        internal void CalcNLFoundsAll()
        {
            if (listFoundation == null || listFoundation.Count == 0) return;
            if (MeshBlackPlanning == null) CreateBlackMesh();
            if (MeshBlackPlanning == null)
            {
                Alert("Сооружение не содержит планировочных отметок");
                return;
            }
            foreach (Foundation item in ListFoundation)
            {
                item.CalcNL(MeshBlackPlanning);
            }

            Alert("Отметки естественного рельефа для всех фундаментов \nвычислены по триангуляционной сети");
        }

        internal void CreateSmallPropsFounds()
        {
            foreach (Foundation item in ListFoundation) item.CreateSmallPropsList();
        }
    }
}
