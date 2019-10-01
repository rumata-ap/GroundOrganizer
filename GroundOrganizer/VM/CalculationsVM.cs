using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using netDxf;
using netDxf.Entities;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        private RelayCommand calcFoundsContours;
        private RelayCommand calcRinBore;
        private RelayCommand calcRinBores;
        private RelayCommand calcRinCoords;
        private RelayCommand calcSpInBore;
        private RelayCommand calcSpInBores;

        private ObservableCollection<DataR> resR;
        private ObservableCollection<DataS> resS;
        private List<DataPair> resSdata;
        private List<DataPair> resRdata;
        private DataR resRselected;
        private List<TypeFound> listTypeFounds;

        public DataR ResRselected { get => resRselected; set { resRselected = value; OnPropertyChanged(); ChangeSelectedResults(); } }
        public ObservableCollection<DataR> ResR { get => resR; set { resR = value; OnPropertyChanged(); } }
        public ObservableCollection<DataS> ResS { get => resS; set { resS = value; OnPropertyChanged(); } }
        public List<DataPair> ResRdata { get => resRdata; set { resRdata = value; OnPropertyChanged(); } }
        public List<DataPair> ResSdata { get => resSdata; set { resSdata = value; OnPropertyChanged(); } }

        public RelayCommand CalcFoundsContours
        {
            get { return calcFoundsContours ?? (calcFoundsContours = new RelayCommand(obj => { CalculateContoursFounds(); })); }
        }

        public RelayCommand CalcRinBore
        {
            get { return calcRinBore ?? (calcRinBore = new RelayCommand(obj => { CalculateRinBore(); })); }
        }

        public RelayCommand CalcRinBores
        {
            get { return calcRinBores ?? (calcRinBores = new RelayCommand(obj => { CalculateRinBores(); })); }
        }

        public RelayCommand CalcRinCoords
        {
            get { return calcRinCoords ?? (calcRinCoords = new RelayCommand(obj => { CalculateRinCoords(); })); }
        }

        public RelayCommand CalcSpInBore
        {
            get { return calcSpInBore ?? (calcSpInBore = new RelayCommand(obj => { CalculateSpInBore(); })); }
        }

        public RelayCommand CalcSpInBores
        {
            get { return calcSpInBores ?? (calcSpInBores = new RelayCommand(obj => { CalculateSpInBores(); })); }
        }

        private void CalculateContoursFounds()
        {
            if (listFoundation == null || listFoundation.Count == 0) return;

            foreach (Foundation item in listFoundation) item.CalcContour();
            ListFoundation = new ObservableCollection<Foundation>(listFoundation);
        }

        void ChangeSelectedResults()
        {
            if (resR == null || resR.Count == 0) return;
            ResRdata = resRselected.SmallData;
        }

        void CalculateSpInBore()
        {
            if (selectedBore == null) { Alert("Не выбрана расчетная скважина"); return; }
            if (selectedFoundation == null) { Alert("Не выбран фундамент для расчета"); return; }
            if (selectedFoundLoad == null) { Alert("Не выбрана расчетная нагрузка"); return; }
            if (resS == null) ResS = new ObservableCollection<DataS>();
            ResS.Clear();
            DataS s = selectedFoundation.Sp(selectedBore, selectedFoundLoad);
            s.Bore = "Скв. " + nameBore;
            s.Base = nameFoundation;
            ResS.Add(s);
            ResultsPage resPage = new ResultsPage();
            MW.ResulsFrame.Content = resPage;
            resPage.ResultsDataGrid.ItemsSource = ResS;
            MW.MainTabControl.SelectedIndex = 5;
            //FoundationsPage foundPage = (FoundationsPage)MW.FoundationsFrame.Content;
            //resPage.resultExpander.IsExpanded = true;
        }

        void CalculateSpInBores()
        {
            if (listBore == null) { Alert("Площадка не содержит ни одной скважины"); return; }
            if (selectedFoundation == null) { Alert("Не выбран фундамент для расчета"); return; }
            if (selectedFoundLoad == null) { Alert("Не выбрана расчетная нагрузка"); return; }
            if (resS == null) ResS = new ObservableCollection<DataS>();
            ResS.Clear();
            foreach (Bore item in listBore)
            {
                DataS s = selectedFoundation.Sp(item, selectedFoundLoad);
                s.Bore = "Скв. " + item.Name;
                s.Base = nameFoundation;
                ResS.Add(s);
            }
            ResultsPage resPage = new ResultsPage();
            MW.ResulsFrame.Content = resPage;
            resPage.ResultsDataGrid.ItemsSource = ResS;
            MW.MainTabControl.SelectedIndex = 5;
            //FoundationsPage foundPage = (FoundationsPage)MW.FoundationsFrame.Content;
            //foundPage.ResultsDataGrid.ItemsSource = ResS;
            //foundPage.resultExpander.IsExpanded = true;

        }

        void CalculateRinBore()
        {
            if (selectedBore == null) { Alert("Не выбрана расчетная скважина"); return; }
            if (selectedFoundation == null) { Alert("Не выбран фундамент для расчета"); return; }
            if (resR == null) ResR = new ObservableCollection<DataR>();
            ResR.Clear();
            DataR r = selectedFoundation.P(selectedBore, selectedStructure.flexStructure, selectedStructure.L, selectedStructure.H);
            ResR.Add(r);
            ResultsPage resPage = new ResultsPage();
            MW.ResulsFrame.Content = resPage;
            //resPage.ResultsDataGrid.ItemsSource = ResR;
            //resPage.DetailedResultsDataGrid.ItemsSource = ResRdata;
            MW.MainTabControl.SelectedIndex = 5;
            //FoundationsPage foundPage = (FoundationsPage)MW.FoundationsFrame.Content;
            //foundPage.ResultsDataGrid.ItemsSource = ResR;
            //foundPage.resultExpander.IsExpanded = true;
        }

        private void CalculateRinBores()
        {
            if (listBore == null) { Alert("Площадка не содержит ни одной скважины"); return; }
            if (selectedFoundation == null) { Alert("Не выбран фундамент для расчета"); return; }
            ResR = selectedFoundation.P(ListBore, selectedStructure.flexStructure, selectedStructure.L, selectedStructure.H);
            ResultsPage resPage = new ResultsPage();
            MW.ResulsFrame.Content = resPage;
            //resPage.ResultsDataGrid.ItemsSource = ResR;
            //resPage.DetailedResultsDataGrid.ItemsSource = ResRdata;
            MW.MainTabControl.SelectedIndex = 5;
            //FoundationsPage foundPage = (FoundationsPage)MW.FoundationsFrame.Content;
            //foundPage.ResultsDataGrid.ItemsSource = ResR;
            //foundPage.resultExpander.IsExpanded = true;
        }

        private void CalculateRinCoords()
        {
            if (selectedBore == null) return;
            if (resR == null) ResR = new ObservableCollection<DataR>();
            ResR.Clear();
            DataR r = SelectedFoundation.P(selectedBore, selectedStructure.flexStructure, selectedStructure.L, selectedStructure.H);
            r.Bore = "По координатам";
            r.Base = nameFoundation;
            ResR.Add(r);
            //FoundationsPage foundPage = (FoundationsPage)MW.FoundationsFrame.Content;
            //foundPage.ResultsDataGrid.ItemsSource = ResR;
            //foundPage.resultExpander.IsExpanded = true;
        }
    }
}
