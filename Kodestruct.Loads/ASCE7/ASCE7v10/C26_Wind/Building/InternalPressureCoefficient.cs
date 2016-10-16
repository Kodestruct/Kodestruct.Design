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
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building
{
    public partial class WindBuilding : WindStructure
    {

        public double GetInternalPressureCoefficient(WindEnclosureType EnclosureType)
        {
            double GCpi = 0.0;


            switch (EnclosureType)
            {
                case WindEnclosureType.Open:
                    GCpi= 0.0;
       
                    break;
                case WindEnclosureType.PartiallyEnclosed:
                    GCpi= 0.55;
                   
                    break;
                case WindEnclosureType.Enclosed:
                    GCpi= 0.18;
                
                    break;
            }

            return GCpi;
        }

        public double GetInternalPressureCoefficient(WindEnclosureType EnclosureType, double OpeningArea,
             double InternalVolume)
        {
            double GCpi = this.GetInternalPressureCoefficient(EnclosureType);
            double Vi = InternalVolume;
            double Aog = OpeningArea;
            double Ri = 1.0;
            double GCpiR = GCpi * Ri;

            //WindInternalPressureCoefficientLargeVolumeRi.docx

            
            #region GCpiR
            ICalcLogEntry GCpiREntry = new CalcLogEntry();
            GCpiREntry.ValueName = "GCpiR";
            GCpiREntry.AddDependencyValue("GCpi", Math.Round(GCpi, 3));
            GCpiREntry.AddDependencyValue("Vi", Math.Round(Vi, 3));
            GCpiREntry.Reference = "";
            GCpiREntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindInternalPressureCoefficientLargeVolumeRi.docx";
            GCpiREntry.FormulaID = null; //reference to formula from code
            GCpiREntry.VariableValue = Math.Round(GCpiR, 3).ToString();
            #endregion
            this.AddToLog(GCpiREntry);
            
            return GCpiR;
        }
    }
}

