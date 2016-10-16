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
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindStructure : AnalyticalElement
    {
        public WindStructureDynamicResponseType GetDynamicClassification(double n1)
        {
            WindStructureDynamicResponseType classification = WindStructureDynamicResponseType.Flexible;
          
            if (n1>1.0)
            {
                classification = WindStructureDynamicResponseType.Rigid;
                
                #region Rigid
                ICalcLogEntry RigidEntry = new CalcLogEntry();
                RigidEntry.ValueName = "n1";
                RigidEntry.Reference = "";
                RigidEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindDynamicClassificationRigid.docx";
                RigidEntry.VariableValue = Math.Round(n1, 3).ToString();
                #endregion
                this.AddToLog(RigidEntry);
            }
            else
            {
                classification = WindStructureDynamicResponseType.Flexible;

                #region Flexible
                ICalcLogEntry FlexibleEntry = new CalcLogEntry();
                FlexibleEntry.ValueName = "n1";
                FlexibleEntry.Reference = "";
                FlexibleEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindDynamicClassificationFlexible.docx";
                FlexibleEntry.VariableValue = Math.Round(n1, 3).ToString();
                #endregion
                this.AddToLog(FlexibleEntry);
            }
            
            return classification;
        }

    }
}
