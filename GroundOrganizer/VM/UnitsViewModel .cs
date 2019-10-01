using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GroundOrganizer
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        UnitsLin[] unitsL = { UnitsLin.м, UnitsLin.см, UnitsLin.мм };
        UnitsArea[] unitsA = { UnitsArea.м, UnitsArea.см, UnitsArea.мм };
        UnitsForce[] unitsF = { UnitsForce.т, UnitsForce.кН, UnitsForce.Н };
        UnitsStress[] unitsS = { UnitsStress.т, UnitsStress.кН, UnitsStress.Н, UnitsStress.МПа, UnitsStress.кПа };
        UnitsLin unitsLId = UnitsLin.м;
        UnitsLin unitsSectId = UnitsLin.см;
        UnitsLin unitsDarmId = UnitsLin.мм;
        UnitsArea unitsAarmId = UnitsArea.см;
        UnitsArea unitsAMatId = UnitsArea._;
        UnitsLin unitsLForcId = UnitsLin.м;
        UnitsArea unitsAStressId = UnitsArea._;
        UnitsLin unitsCrackId = UnitsLin.мм;
        UnitsStress unitsSMatId = UnitsStress.МПа;
        UnitsStress unitsSStressId = UnitsStress.МПа;
        UnitsForce unitsFForceId = UnitsForce.кН;
        internal double scaleDimLength = 1;
        internal double scaleDimSect = 0.01;
        internal double scaleDimDarm = 0.001;
        internal double scaleDimAarm = 0.0001;
        internal double scaleDimLfrc = 1;
        internal double scaleDimFfrc = 1;
        internal double scaleDimStress = 0.001;
        internal double scaleDimMat = 0.001;
        internal double scaleDimCrack = 0.001;

        public UnitsLin[] UnitsL { get => unitsL; set => unitsL = value; }
        public UnitsArea[] UnitsA { get => unitsA; set => unitsA = value; }
        public UnitsForce[] UnitsF { get => unitsF; set => unitsF = value; }
        public UnitsStress[] UnitsS { get => unitsS; set => unitsS = value; }

        public UnitsLin UnitsLId { get => unitsLId; set { unitsLId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsLin UnitsSectId { get => unitsSectId; set { unitsSectId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsLin UnitsDarmId { get => unitsDarmId; set { unitsDarmId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsArea UnitsAarmId { get => unitsAarmId; set { unitsAarmId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsArea UnitsAMatId { get => unitsAMatId; set { unitsAMatId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsLin UnitsLForcId { get => unitsLForcId; set { unitsLForcId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsArea UnitsAStressId { get => unitsAStressId; set { unitsAStressId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsLin UnitsCrackId { get => unitsCrackId; set { unitsCrackId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsForce UnitsFForceId { get => unitsFForceId; set { unitsFForceId = value; OnPropertyChanged(); ChangeUnits(); } }
        public UnitsStress UnitsSMatId { get => unitsSMatId; set { unitsSMatId = value; OnPropertyChanged(); ChangeUnits();
                if (value == UnitsStress.МПа || value == UnitsStress.кПа) UnitsAMatId = UnitsArea._; } }
        public UnitsStress UnitsSStressId { get => unitsSStressId; set { unitsSStressId = value; OnPropertyChanged(); ChangeUnits();
                if (value == UnitsStress.МПа || value == UnitsStress.кПа) UnitsAStressId = UnitsArea._; } }


        void ChangeUnits()
        {
            scaleDimAarm = ScaleAUnits(unitsAarmId);
            scaleDimCrack = ScaleLUnits(unitsCrackId);
            scaleDimDarm = ScaleLUnits(unitsDarmId);
            scaleDimFfrc = ScaleFUnits(unitsFForceId);
            scaleDimLength = ScaleLUnits(unitsLId);
            scaleDimLfrc = ScaleLUnits(unitsLForcId);
            scaleDimSect = ScaleLUnits(unitsSectId);
            scaleDimMat = ScaleSUnits(unitsSMatId,unitsAMatId);
            scaleDimStress = ScaleSUnits(unitsSStressId, unitsAStressId);
            //Bfloat = B / scaleDimSect;
            //Hfloat = H / scaleDimSect;
            //Dfloat = D / scaleDimSect;
            //D1float = D1 / scaleDimSect;
        }

        void ChangeValues()
        {
            //B = bfloat * scaleDimSect;
            //H = hfloat * scaleDimSect;
            //D = dfloat * scaleDimSect;
            //D1 = d1float * scaleDimSect;
        }

        double ScaleLUnits(UnitsLin unit)
        {
            double res = 1;
            switch (unit)
            {
                case UnitsLin.мм: res = 1E-3;  break;
                case UnitsLin.см: res = 1E-2; break;
            }
            return res;
        }

        double ScaleAUnits(UnitsArea unit)
        {
            double res = 1;
            switch (unit)
            {
                case UnitsArea.мм: res = 1E-6; break;
                case UnitsArea.см: res = 1E-4; break;
            }
            return res;
        }

        double ScaleFUnits(UnitsForce unit)
        {
            double res = 1;
            switch (unit)
            {
                case UnitsForce.Н: res = 1E+3; break;
                case UnitsForce.т: res = 0.10194; break;
            }
            return res;
        }

        double ScaleSUnits(UnitsStress unitS, UnitsArea unitA)
        {
            double res = 1;
            if (unitS == UnitsStress.кН && unitA == UnitsArea.см) res = 1E-4;
            if (unitS == UnitsStress.кН && unitA == UnitsArea.мм) res = 1E-6;
            if (unitS == UnitsStress.Н && unitA == UnitsArea.м) res = 1E+3;
            if (unitS == UnitsStress.Н && unitA == UnitsArea.см) res = 0.1;
            if (unitS == UnitsStress.Н && unitA == UnitsArea.мм) res = 1E-3;
            if (unitS == UnitsStress.т && unitA == UnitsArea.м) res = 0.10194;
            if (unitS == UnitsStress.т && unitA == UnitsArea.см) res = 1E-5;
            if (unitS == UnitsStress.т && unitA == UnitsArea.мм) res = 1.01937E-7;
            if (unitS == UnitsStress.МПа) res = 1E-3;
            return res;
        }
    }
}
