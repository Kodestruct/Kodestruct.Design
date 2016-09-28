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
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;


namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public abstract partial class RhsToPlateConnection :HssToPlateConnection //: IHssConcentratedForceConnection
    {


        private SteelPlateSection plate;

        public SteelPlateSection Plate
        {
            get { return plate; }
            set { plate = value; }
        }

        private SteelRhsSection hss;

        public SteelRhsSection Hss
        {
            get { return hss; }
            set { hss = value; }
        }



        public RhsToPlateConnection(SteelRhsSection Hss, SteelPlateSection Plate, ICalcLog CalcLog, bool IsTensionHss,double P_uHss, double M_uHss)
            :base(IsTensionHss,P_uHss,M_uHss, CalcLog)
        {
            this.hss = Hss;
            this.plate = Plate;
        }

        public ISteelSection GetHssSteelSection()
        {
            ISteelSection s = hss as ISteelSection;
            if (s==null)
            {
                throw new Exception("Hss member must implement ISteelSection interface");
            }
            return s;
        }

        protected override double GetSectionModulus()
        {
            return hss.Section.S_xBot;
        }

        protected override double GetArea()
        {
            ISectionTube tb = hss.Section;
            return hss.Section.A;
        }

        protected override double GetF_y()
        {
            return hss.Material.YieldStress;
        }
    }
}
