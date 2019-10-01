using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    [Serializable]
    internal class ToSerializ
    {
        public ObservableCollection<PlayGround> PlayGroundList { get; set; }
    }
}
