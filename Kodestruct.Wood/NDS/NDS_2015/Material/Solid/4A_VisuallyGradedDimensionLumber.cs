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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Kodestruct.Wood.NDS.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Wood.NDS.Material;
using Kodestruct.Wood.Properties;
using Kodestruct.Wood.NDS.NDS2015.Material;

namespace Kodestruct.Wood.NDS.NDS2015.Material
{
    public class VisuallyGradedDimensionLumber : WoodSolidMaterial
    {


        public VisuallyGradedDimensionLumber(string Species, string CommercialGrade, string SizeClass, ICalcLog CalcLog)
            : base(Species, CommercialGrade, SizeClass, CalcLog)
        { 

        }




        protected override string GetResource()
        {
            return "NDS2015_Suppl_Table4A"; 
        }


        /// <summary>
        /// Example of how to extract reference values
        /// </summary>
        //protected void ReadReferenceValues()
        //{
            
        //      double Fb = F_b;
        //      double Ft = F_t;
        //      double Fv = F_v;
        //      double FcPerp = F_cPerp;
        //      double Fc = F_cParal;
        //      double E = E;
        //      double Emin = E_min;
        //      string CommercialGradeString = CommercialGradeStringConverter.GetCommercialGradeString(Grade);

              

        //}
    }
}
