using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    public class Structure
    {
        public TypeFlexStructure flexStructure { get; set; }
        public double L { get; set; }
        public double H { get; set; }
        public double Null { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<Foundation> Foundations { get; set; }
        public ObservableCollection<PlanningVertex> RedPlanning { get; set; }
        public ObservableCollection<PlanningVertex> BlackPlanning { get; set; }
        public Structure()
        {
            Foundations = new ObservableCollection<Foundation>();
            RedPlanning = new ObservableCollection<PlanningVertex>();
            BlackPlanning = new ObservableCollection<PlanningVertex>();
        }
    }
}
