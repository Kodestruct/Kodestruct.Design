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

namespace Kodestruct.Loads.ASCE.ASCE7_10.IceLoads
{
    public partial class IceStructure : AnalyticalElement
    {

        
        public double GetIceWeightLine( double Ai , double gammai)
        {
            double wi =0.0;
            if (gammai == 0.0)
            {
                 gammai = GetIceDensity();
            }
            wi = Ai/144.0 * gammai * 1.0;
            
            #region wi
            ICalcLogEntry wiEntry = new CalcLogEntry();
            wiEntry.ValueName = "wi";
            wiEntry.AddDependencyValue("Ai", Math.Round(Ai, 3));
            wiEntry.AddDependencyValue("gammai", Math.Round(gammai, 3));
            wiEntry.Reference = "";
            wiEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/IceWeightLine.docx";
            wiEntry.FormulaID = null; //reference to formula from code
            wiEntry.VariableValue = Math.Round(wi, 3).ToString();
            #endregion
            this.AddToLog(wiEntry);

            return wi;
        }




        public double GetIceWeightPlate(double td, IcePlateOrientation plateOrientation, double gammai)
        {
            double qi;
            if (gammai == 0.0)
            {
                gammai = GetIceDensity();
            }

            #region qi
            ICalcLogEntry qiEntry = new CalcLogEntry();
            qiEntry.ValueName = "qi";
            qiEntry.AddDependencyValue("td", Math.Round(td, 3));
            qiEntry.AddDependencyValue("gammai", Math.Round(gammai, 3));
            qiEntry.Reference = "";
            
            qiEntry.FormulaID = null; //reference to formula from code

            #endregion
            switch (plateOrientation)
            {
                case IcePlateOrientation.Vertical:
                    qi = 0.8* Math.PI * td/12.0 * gammai;
                    qiEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/IceWeightPlateVertical.docx";
                    break;
                case IcePlateOrientation.Horizontal:
                    qi = 0.6* Math.PI * td/12.0 * gammai;
                    qiEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/IceWeightPlateHorizontal.docx";
                    break;
                default:
                    qi = Math.PI * td/12.0 * gammai;
                    qiEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/IceWeightPlate.docx";
                    break;
            }

            qiEntry.VariableValue = Math.Round(qi, 3).ToString();
            this.AddToLog(qiEntry);
            return qi;
        }

        private double GetIceDensity()
        {
            double gammai =56.0;
            
            #region gammai
            ICalcLogEntry gammaiEntry = new CalcLogEntry();
            gammaiEntry.ValueName = "gammai";
            gammaiEntry.Reference = "";
            gammaiEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/IceDensity.docx";
            gammaiEntry.FormulaID = null; //reference to formula from code
            gammaiEntry.VariableValue = Math.Round(gammai, 3).ToString();
            #endregion
            this.AddToLog(gammaiEntry);

            return gammai;
        }
    }
}
