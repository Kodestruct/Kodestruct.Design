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

namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads.Components
{
    public class ComponentPrecastPanel : BuildingComponentBase
    {
        public ComponentPrecastPanel(int Option1, int Option2, double NumericValue)
            : base(Option1, Option2, NumericValue)
        {
            switch (Option1)
            {
                case 0:  Thickness = PrecastThickness.t4; break;  //4 in.
                case 1:  Thickness = PrecastThickness.t6; break;  //6 in.
                case 2:  Thickness = PrecastThickness.t8; break;  //8 in.
                default: Thickness = PrecastThickness.t8; break;
             }
        }

        double q_precast;
        PrecastThickness Thickness { get; set; }

        protected override void Calculate()
        {
            string ThicknessString = null;
            switch (Thickness)
	        {
		       case PrecastThickness.t4: q_precast= 55.0 ;  ThicknessString = "4 in."; break;  //4 in.
               case PrecastThickness.t6: q_precast= 80.0 ;  ThicknessString = "6 in."; break;  //6 in.
               case PrecastThickness.t8: q_precast = 105.0; ThicknessString = "8 in."; break;  //8 in.
	        }

            base.Weight = q_precast;
            base.Notes = string.Format
                ("{0} thick panel",
                ThicknessString);
        }

        enum PrecastThickness
        {
            t4,
            t6,
            t8
        }
    }
}
