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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class DevelopmentTension:Development
    {

        public DevelopmentTension(
            IConcreteMaterial Concrete, 
            Rebar Rebar, 
            double clearSpacing, 
            double ClearCover,
            bool IsTopRebar, 
            double ExcessReinforcementRatio, 
            bool CheckMinimumLength,  
            ICalcLog log)
            : base (Concrete, Rebar, ExcessReinforcementRatio,log)
        {
            this.isTopRebar = IsTopRebar;
            this.ClearCover = ClearCover;
            this.clearSpacing = clearSpacing;
            this.CheckMinimumLength = CheckMinimumLength;
            db = Rebar.Diameter;
        }

            public DevelopmentTension(
            IConcreteMaterial Concrete,
            Rebar Rebar,
            bool MeetsSpacingCritera,
            bool IsTopRebar,
            double ExcessReinforcementRatio,
            bool CheckMinimumLength,
            ICalcLog log)
            : base(Concrete, Rebar, ExcessReinforcementRatio, log)
            {
            this.isTopRebar = IsTopRebar;
            this.MeetsSpacingCritera = MeetsSpacingCritera;
            this.CheckMinimumLength = CheckMinimumLength;
            db = Rebar.Diameter;
            }

        double db;

        private double clearSpacing;

        public double ClearSpacing
        {
            get { return clearSpacing; }
            set { clearSpacing = value; }
        }

        private double clearCover;

        public double ClearCover
        {
            get { return clearCover; }
            set { clearCover = value; }
        }

        private bool isTopRebar;

        public bool IsTopRebar
        {
            get { return isTopRebar; }
            set { isTopRebar = value; }
        }

        private bool meetsSpacingCritera;

        public bool MeetsSpacingCritera
        {
            get { return meetsSpacingCritera; }
            set { meetsSpacingCritera = value; }
        }

        public bool CheckMinimumLength { get; set; }

    }
}
