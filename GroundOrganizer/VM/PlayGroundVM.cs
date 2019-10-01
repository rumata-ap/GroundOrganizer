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
        int numPG = 1;
        PlayGround selectedPlayGround;
        string namePlayGround;
        ObservableCollection<PlayGround> listPlayGround = new ObservableCollection<PlayGround>();

        public ObservableCollection<PlayGround> ListPlayGround { get => listPlayGround; set { listPlayGround = value; OnPropertyChanged(); } }
        public string NamePlayGround { get => namePlayGround; set { namePlayGround = value; OnPropertyChanged(); } }
        public PlayGround SelectedPlayGround { get => selectedPlayGround; set { selectedPlayGround = value; OnPropertyChanged(); ChangeSelectedPlayGround(); } }

        private RelayCommand addPlayGround;
        public RelayCommand AddPlayGround
        {
            get { return addPlayGround ?? (addPlayGround = new RelayCommand(obj => { AddPlayGroundToList(); })); }
        }

        private RelayCommand updatePlayGround;
        public RelayCommand UpdatePlayGrounds
        {
            get { return updatePlayGround ?? (updatePlayGround = new RelayCommand(obj => { UpdatePlayGroundInList(); })); }
        }

        private RelayCommand renumPlayGround;
        public RelayCommand RenumPlayGrounds
        {
            get { return renumPlayGround ?? (renumPlayGround = new RelayCommand(obj => { RenumberingPlayGroundInList(); })); }
        }

        void AddPlayGroundToList()
        {
            ListPlayGround.Add(new PlayGround() { Number = numPG }); numPG++;
            //CreateListNumberTypeBet();
            //NamePlayGround = listPlayGround[0].Name;
        }
        void RenumberingPlayGroundInList()
        {
            int j = 1;
            foreach (PlayGround item in ListPlayGround)
            {
                item.Number = j;
                j++;
            }
            numPG = j;
            ListPlayGround = new ObservableCollection<PlayGround>(ListPlayGround);
        }
        void UpdatePlayGroundInList()
        {
            ListPlayGround = new ObservableCollection<PlayGround>(ListPlayGround);
            SaveDB();
        }

        //void ChangeSelClassBet()
        //{
        //    //if (selectedBet==null) return;
        //    CreateListNumberTypeBet();
        //}

        //void CreateListNumberTypeBet()
        //{
        //    //listNumberTypeArm = new List<int>();
        //    ListNumberTypeBet = from bet in listBet select bet.Id;
        //}

        void ChangeSelectedPlayGround()
        {
            if (listPlayGround.Count == 0 || selectedPlayGround == null) return;
            NamePlayGround = SelectedPlayGround.Name;
            ListIGE = selectedPlayGround.IGEs;
            ListBore = selectedPlayGround.Bores;
            ListStructure = selectedPlayGround.Structures;
            CreateListNumIGEs();
            CreateListShemas();
            CreateListTypeFoundations();
        }
    }
}
