#region Copyright
   /*Copyright (C) 2015 Konstantin Udilovich

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
//using Kodestruct.Analytics.Section;
 
 
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.Exceptions;


namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public abstract partial class ChsToPlateConnection :HssToPlateConnection, IHssConcentratedForceConnection
    {

        private SteelPlateSection plate;

        public SteelPlateSection Plate
        {
            get { return plate; }
            set { plate = value; }
        }

        private SteelChsSection hss;

        public SteelChsSection Hss
        {
            get { return hss; }
            set { hss = value; }
        }


        public ChsToPlateConnection(SteelChsSection Hss, SteelPlateSection Plate, ICalcLog CalcLog, bool IsTensionHss,double P_uHss, double M_uHss)
            :base(IsTensionHss,P_uHss, M_uHss, CalcLog)
        {
            this.hss = Hss;
            this.plate = Plate;
        }

        protected void GetTypicalParameters(ref double Fy, ref double t, ref double Bp, ref double D, ref double tp )
        {
            Fy = Hss.Material.YieldStress;
            t = Hss.Section.t_des;
            Bp = Plate.Section.H;
            tp = Plate.Section.B;

            ISectionPipe pipe = Hss.Section as ISectionPipe;
            if (pipe != null)
            {
                throw new SectionWrongTypeException(typeof(ISectionPipe));
            }
            D = pipe.D;

        }

        protected override double GetSectionModulus()
        {
            return hss.Section.S_xBot;
        }

        protected override double GetArea()
        {
            return hss.Section.A;
        }

        protected override double GetF_y()
        {
            return hss.Material.YieldStress;
        }
    }
}
