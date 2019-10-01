using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        int numStructure = 1;
        string nameStructure;
        private string structureNote;
        Structure selectedStructure;
        ObservableCollection<Structure> listStructure = new ObservableCollection<Structure>();

        public ObservableCollection<Structure> ListStructure { get => listStructure; set { listStructure = value; OnPropertyChanged(); } }

        private IEnumerable<TypeFlexStructure> listShemas;
        public IEnumerable<TypeFlexStructure> ListShemas { get => listShemas; set { listShemas = value; OnPropertyChanged(); } }
        public string StructureNote { get => structureNote; set { structureNote = value; OnPropertyChanged(); } }

        private RelayCommand addStructure;
        public RelayCommand AddStructure
        {
            get { return addStructure ?? (addStructure = new RelayCommand(obj => { AddStructureToList(); })); }
        }

        private RelayCommand updateStructure;
        public RelayCommand UpdateStructures
        {
            get { return updateStructure ?? (updateStructure = new RelayCommand(obj => { UpdateStructureInList(); })); }
        }

        private RelayCommand renumStructure;

        public RelayCommand RenumStructures
        {
            get { return renumStructure ?? (renumStructure = new RelayCommand(obj => { RenumberingStructureInList(); })); }
        }

        public Structure SelectedStructure { get => selectedStructure; set { selectedStructure = value; OnPropertyChanged(); ChangeSelectedStructure(); } }

        public string NameStructure { get => nameStructure; set { nameStructure = value; OnPropertyChanged(); } }

        void AddStructureToList()
        {
            ListStructure.Add(new Structure() { Number = numStructure }); numStructure++;
            //NameStructure = listStructure[0].Name;
        }
        void RenumberingStructureInList()
        {
            int j = 1;
            foreach (Structure item in ListStructure)
            {
                item.Number = j;
                j++;
            }
            numStructure = j;
            ListStructure = new ObservableCollection<Structure>(ListStructure);
        }
        void UpdateStructureInList()
        {
            ListStructure = new ObservableCollection<Structure>(ListStructure);
            SaveDB();
        }

        void ChangeSelectedStructure()
        {
            if (listStructure.Count == 0 || selectedStructure == null) return;
            NameStructure = selectedStructure.Name;
            ListFoundation = selectedStructure.Foundations;
            RedPlanning = selectedStructure.RedPlanning;
            BlackPlanning = selectedStructure.BlackPlanning;
            if (redPlanning == null) RedPlanning = new ObservableCollection<PlanningVertex>();
            if (blackPlanning == null) BlackPlanning = new ObservableCollection<PlanningVertex>();
        }

        void CreateListShemas()
        {
            if (listStructure == null || listPlayGround == null) return;
            ListShemas = new List<TypeFlexStructure>() { TypeFlexStructure.Гибкая, TypeFlexStructure.Жесткая };
            try
            {
                StructuresPage structuresPage = (StructuresPage)MW.StructuresFrame.Content;
                structuresPage.flexListCbxCol.ItemsSource = listShemas;
            }
            catch
            {
                return;
            }
        }
    }
}
