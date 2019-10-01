using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{   [Serializable]
    public struct DataPair
    {
        public string Описание { get; set; }
        public object Параметр { get; set; }
        public UnitsForce ForcesUnits { get; set; }
        public UnitsStress StressUnits { get; set; }
        public UnitsArea AreasUnits { get; set; }
        public UnitsLin LenghtsUnits { get; set; }      
    }

    [Serializable]
    public struct DataPairProps
    {
        public string Описание { get; set; }
        public object Параметр { get; set; }
        public UnitsList Ед_изм { get; set; }
    }
}
