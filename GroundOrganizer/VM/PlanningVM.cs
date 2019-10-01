using Microsoft.Win32;
using netDxf;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Tools;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        private RelayCommand addRedVertex;
        private RelayCommand addBlackVertex;
        private RelayCommand updateRedPlanning;
        private RelayCommand updateBlackPlanning;
        private RelayCommand renumRedVertexes;
        private RelayCommand renumBlackVertexes;
        private RelayCommand importPlanningsFromDXF;
        private RelayCommand importRedPlanningFromCSV;
        private RelayCommand importBlackPlanningFromCSV;
        private RelayCommand exportRedPlanningToCSV;
        private RelayCommand exportBlackPlanningToCSV;
        private RelayCommand createRedPlanningMesh;
        private RelayCommand createBlackPlanningMesh;

        int numVertex = 1;
        PlanningVertex selectedRedVertex;
        PlanningVertex selectedBlackVertex;
        ObservableCollection<PlanningVertex> redPlanning = new ObservableCollection<PlanningVertex>();
        ObservableCollection<PlanningVertex> blackPlanning = new ObservableCollection<PlanningVertex>();

        public ObservableCollection<PlanningVertex> RedPlanning { get => redPlanning; set { redPlanning = value; OnPropertyChanged(); } }
        public ObservableCollection<PlanningVertex> BlackPlanning { get => blackPlanning; set { blackPlanning = value; OnPropertyChanged(); } }
        public PlanningVertex SelectedRedVertex { get => selectedRedVertex; set { selectedRedVertex = value; OnPropertyChanged(); } }
        public PlanningVertex SelectedBlackVertex { get => selectedBlackVertex; set { selectedBlackVertex = value; OnPropertyChanged(); } }
        public TriangleNet.Mesh MeshRedPlanning { get; private set; }
        public TriangleNet.Mesh MeshBlackPlanning { get; private set; }


        public RelayCommand AddRedVertex
        {
            get { return addRedVertex ?? (addRedVertex = new RelayCommand(obj => { AddRedVertexToList(); })); }
        }

        public RelayCommand UpdateRedPlanning
        {
            get { return updateRedPlanning ?? (updateRedPlanning = new RelayCommand(obj => { UpdateRedVertexesInList(); })); }
        }

        public RelayCommand RenumRedVertexes
        {
            get { return renumRedVertexes ?? (renumRedVertexes = new RelayCommand(obj => { RenumberingRedVertexesInList(); })); }
        }

        public RelayCommand AddBlackVertex
        {
            get { return addBlackVertex ?? (addBlackVertex = new RelayCommand(obj => { AddBlackVertexToList(); })); }
        }

        public RelayCommand UpdateBlackPlanning
        {
            get { return updateBlackPlanning ?? (updateBlackPlanning = new RelayCommand(obj => { UpdateBlackVertexesInList(); })); }
        }

        public RelayCommand RenumBlackVertexes
        {
            get { return renumBlackVertexes ?? (renumBlackVertexes = new RelayCommand(obj => { RenumberingBlackVertexesInList(); })); }
        }

        public RelayCommand ImportPlanningsFromDXF
        {
            get { return importPlanningsFromDXF ?? (importPlanningsFromDXF = new RelayCommand(obj => { ReadPlanningsDXF(); })); }
        }

        public RelayCommand ImportRedPlanningFromCSV
        {
            get { return importRedPlanningFromCSV ?? (importRedPlanningFromCSV = new RelayCommand(obj => { ReadRedPlanningCSV(); })); }
        }

        public RelayCommand ExportRedPlanningToCSV
        {
            get { return exportRedPlanningToCSV ?? (exportRedPlanningToCSV = new RelayCommand(obj => { SaveRedPlanningInCsv(); })); }
        }


        public RelayCommand ImportBlackPlanningFromCSV
        {
            get { return importBlackPlanningFromCSV ?? (importBlackPlanningFromCSV = new RelayCommand(obj => { ReadBlackPlanningCSV(); })); }
        }

        public RelayCommand ExportBlackPlanningToCSV
        {
            get { return exportBlackPlanningToCSV ?? (exportBlackPlanningToCSV = new RelayCommand(obj => { SaveBlackPlanningInCsv(); })); }
        }

        public RelayCommand CreateRedPlanningMesh
        {
            get { return createRedPlanningMesh ?? (createRedPlanningMesh = new RelayCommand(obj => { CreateRedMesh(); })); }
        }

        public RelayCommand CreateBlackPlanningMesh
        {
            get { return createBlackPlanningMesh ?? (createBlackPlanningMesh = new RelayCommand(obj => { CreateBlackMesh(); })); }
        }

        private void ReadRedPlanningCSV()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Текстовый файл (*.csv)|*.csv|Все файлы (*.*)|*.*";
            ofd.Title = "Заполнение таблицы";
            ofd.ShowDialog();

            if (ofd.FileName == null || ofd.FileName == "") return;

            RedPlanning.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(ofd.FileName, System.Text.Encoding.Default))
                {
                    string line;
                    PlanningVertex vert;
                    while ((line = sr.ReadLine()) != null)
                    {
                        vert = new PlanningVertex();
                        vert.StringToProps(line);
                        RedPlanning.Add(vert);
                    }
                }
            }
            catch (Exception e)
            {
                Alert(e.Message);
            }
        }

        internal void SaveRedPlanningInCsv()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.csv";
            sfd.Filter = "Текстовый файл (*.csv)|*.csv";
            sfd.Title = "Сохранение таблицы";
            //sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.ShowDialog();

            if (sfd.FileName == null || sfd.FileName == "") return;

            try
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.Default))
                {
                    foreach (PlanningVertex item in RedPlanning)
                    {
                        sw.WriteLine(item.PropsToString());
                    }
                }
            }
            catch (Exception e)
            {
                Alert(e.Message);
            }
        }

        private void ReadBlackPlanningCSV()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Текстовый файл (*.csv)|*.csv|Все файлы (*.*)|*.*";
            ofd.Title = "Заполнение таблицы";
            ofd.ShowDialog();

            if (ofd.FileName == null || ofd.FileName == "") return;

            BlackPlanning.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(ofd.FileName, System.Text.Encoding.Default))
                {
                    string line;
                    PlanningVertex vert;
                    while ((line = sr.ReadLine()) != null)
                    {
                        vert = new PlanningVertex();
                        vert.StringToProps(line);
                        BlackPlanning.Add(vert);
                    }
                }
            }
            catch (Exception e)
            {
                Alert(e.Message);
            }
        }

        internal void SaveBlackPlanningInCsv()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.csv";
            sfd.Filter = "Текстовый файл (*.csv)|*.csv";
            sfd.Title = "Сохранение таблицы";
            //sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.ShowDialog();

            if (sfd.FileName == null || sfd.FileName == "") return;

            try
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.Default))
                {
                    foreach (PlanningVertex item in BlackPlanning)
                    {
                        sw.WriteLine(item.PropsToString());
                    }
                }
            }
            catch (Exception e)
            {
                Alert(e.Message);
            }
        }

        private void ReadPlanningsDXF()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.dxf";
            ofd.Filter = "Чертеж (*.dxf)|*.dxf";
            ofd.Title = "Импорт чертежа";
            ofd.ShowDialog();

            if (ofd.FileName == null || ofd.FileName == "") return;
            if (selectedStructure == null) return;
            RedPlanning.Clear(); BlackPlanning.Clear();
            DxfDocument dxfDocument = DxfDocument.Load(ofd.FileName);
            IEnumerable<Text> dxfTexts = dxfDocument.Texts;
            PlanningVertex vert; int j = 1; int k = 1;
            foreach (Text item in dxfTexts)
            {
                if (item.Layer.Name == "Red")
                {
                    vert = new PlanningVertex
                    {
                        X = Math.Round(item.Position.X * 0.001, 3),
                        Y = Math.Round(item.Position.Y * 0.001, 3),
                        Number = j,
                        Red = double.Parse(item.Value)
                    };
                    RedPlanning.Add(vert);
                    j++;
                }
                else if (item.Layer.Name == "Black")
                {
                    vert = new PlanningVertex
                    {
                        X = Math.Round(item.Position.X * 0.001, 3),
                        Y = Math.Round(item.Position.Y * 0.001, 3),
                        Number = k,
                        Black = double.Parse(item.Value)
                    };
                    BlackPlanning.Add(vert);
                    k++;
                }
            }
        }

        void AddRedVertexToList()
        {
            RedPlanning.Add(new PlanningVertex() { Number = numVertex }); numVertex++;
            //NameStructure = listStructure[0].Name;
        }
        void RenumberingRedVertexesInList()
        {
            int j = 1;
            foreach (PlanningVertex item in RedPlanning)
            {
                item.Number = j;
                j++;
            }
            numVertex = j;
            RedPlanning = new ObservableCollection<PlanningVertex>(RedPlanning);
        }
        void UpdateRedVertexesInList()
        {
            RedPlanning = new ObservableCollection<PlanningVertex>(RedPlanning);
            SaveDB();
        }

        void AddBlackVertexToList()
        {
            BlackPlanning.Add(new PlanningVertex() { Number = numVertex }); numVertex++;
            //NameStructure = listStructure[0].Name;
        }
        void RenumberingBlackVertexesInList()
        {
            int j = 1;
            foreach (PlanningVertex item in BlackPlanning)
            {
                item.Number = j;
                j++;
            }
            numVertex = j;
            BlackPlanning = new ObservableCollection<PlanningVertex>(BlackPlanning);
        }
        void UpdateBlackVertexesInList()
        {
            BlackPlanning = new ObservableCollection<PlanningVertex>(BlackPlanning);
            SaveDB();
        }

        internal void CreateRedMesh()
        {
            if (selectedStructure == null || selectedStructure.RedPlanning == null || selectedStructure.RedPlanning.Count == 0) return;

            Polygon polygon = new Polygon();
            Vertex vrtx;
            int i = 1;
            foreach (PlanningVertex item in selectedStructure.RedPlanning)
            {
                vrtx = new Vertex(item.X, item.Y, item.Number, 1);
                vrtx.Attributes[0] = item.Red;
                polygon.Add(vrtx);
                i++;
            }                  
            GenericMesher mesher = new GenericMesher();
            ConstraintOptions constraint = new ConstraintOptions();
            constraint.Convex = true;
            MeshRedPlanning = (TriangleNet.Mesh)mesher.Triangulate(polygon, constraint);
            Alert("\"Красная\" триангуляционная сеть создана");
        }

        internal void CreateBlackMesh()
        {
            if (selectedStructure == null || selectedStructure.BlackPlanning == null || selectedStructure.BlackPlanning.Count == 0) return;

            Polygon polygon = new Polygon();
            Vertex vrtx;
            int i = 1;
            foreach (PlanningVertex item in selectedStructure.BlackPlanning)
            {
                vrtx = new Vertex(item.X, item.Y, item.Number, 1);
                vrtx.Attributes[0] = item.Black;
                polygon.Add(vrtx);
                i++;
            }
            GenericMesher mesher = new GenericMesher();
            ConstraintOptions constraint = new ConstraintOptions();
            constraint.Convex = true;
            MeshBlackPlanning = (TriangleNet.Mesh)mesher.Triangulate(polygon, constraint);
            Alert("\"Черная\" триангуляционная сеть создана");
        }
    }
}
