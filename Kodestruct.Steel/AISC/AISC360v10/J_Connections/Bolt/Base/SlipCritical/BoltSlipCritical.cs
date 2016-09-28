#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion

using System;
using Kodestruct.Common.CalculationLogger.Interfaces;

using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public abstract partial class BoltSlipCritical : Bolt, IBoltSlipCritical
    {
        public BoltSlipCritical(double Diameter, BoltThreadCase ThreadType,
            BoltFayingSurfaceClass FayingSurface, BoltHoleType HoleType,
            BoltFillerCase Fillers, int NumberOfSlipPlanes, ICalcLog log, double PretensionMultiplier=1.13)
            : base(Diameter, ThreadType, log)
        {
            this.fayingSurface = FayingSurface;
            this.holeType = HoleType;
            this.fillers = Fillers;
            this.pretensionMultiplier = PretensionMultiplier;
            this.numberOfSlipPlanes = NumberOfSlipPlanes;
        }

        private BoltFayingSurfaceClass fayingSurface;

        public BoltFayingSurfaceClass FayingSurface
        {
            get { return fayingSurface; }
        }

        private BoltHoleType holeType;

        public BoltHoleType HoleType
        {
            get { return holeType; }
        }

        private double _T_b;

        public double T_b
        {
            get {
                if (_T_b==0)
                {
                    _T_b=GetMinimumPretension();
                }
                return _T_b; }
        }

        private double GetMinimumPretension()
        {
            _T_b = Math.Round(0.7 * Area * NominalTensileStress, 0); //Per footnote to Table J3.1
            return _T_b;
        }

        private BoltFillerCase fillers;

        public BoltFillerCase Fillers
        {
            get { return fillers; }
            set { Fillers = value; }
        }

        private double pretensionMultiplier;

        public double PretensionMultiplier
        {
            get { return pretensionMultiplier; }
            set { pretensionMultiplier = value; }
        }

        private int numberOfSlipPlanes;

        public int NumberOfSlipPlanes
        {
            get { return numberOfSlipPlanes; }
            set { numberOfSlipPlanes = value; }
        }
        
        
    }
}
