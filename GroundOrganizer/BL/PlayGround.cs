using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public class PlayGround
    {
        public string Description { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public ObservableCollection<IGE> IGEs { get; private set; }
        public ObservableCollection<Bore> Bores { get; private set; }
        public ObservableCollection<Structure> Structures { get; private set; }

        public PlayGround()
        {
            IGEs = new ObservableCollection<IGE>();
            Bores = new ObservableCollection<Bore>();
            Structures = new ObservableCollection<Structure>();
        }

        public void AddIGE(IGE ige)
        {
            if (IGEs == null) IGEs = new ObservableCollection<IGE>();
            IGEs.Add(ige);
        }
        public void AddIGEs(ObservableCollection<IGE> iges)
        {
            IGEs = iges;
        }
        public void AddIGEs(List<IGE> iges)
        {
            if (IGEs == null) IGEs = new ObservableCollection<IGE>();
            foreach (var item in iges) IGEs.Add(item);
        }
        
        public void AddBore(Bore bore)
        {
            if (Bores == null) Bores = new ObservableCollection<Bore>();
            Bores.Add(bore);
        }
        public void AddBores(ObservableCollection<Bore> bores)
        {
            Bores = bores;
        }
        public void AddBores(List<Bore> bores)
        {
            if (Bores == null) Bores = new ObservableCollection<Bore>();
            foreach (var item in bores) Bores.Add(item);
        }

        public void DeleteBores()
        {
            Bores = new ObservableCollection<Bore>();
        }
        
        public void DeleteIGEs()
        {
            IGEs = new ObservableCollection<IGE>();
        }


    }
}
