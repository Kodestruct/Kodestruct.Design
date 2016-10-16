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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.Properties;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SnowLoads
{
    public partial class SnowStructure
    {
        public double GetThermalFactor(string CaseId)
        {
            double Ct = 1.0;
            try
            {
                var Tv11 = new { CaseId = "",  Ct = 0.0 , CaseDescription=""}; // sample
                var ValueList = ListFactory.MakeList(Tv11);

                using (StringReader reader = new StringReader(Resources.ASCE7_10T7_3SnowCt))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] Vals = line.Split(',');
                        if (Vals.Count() == 3)
                        {
                            string _CaseId =Vals[0];
                            double _Ct = double.Parse(Vals[1], CultureInfo.InvariantCulture);
                            string _CaseDescription = Vals[2];
                            ValueList.Add(new { CaseId = _CaseId, Ct = _Ct, CaseDescription = _CaseDescription });
                        }
                    }

                }

                var CtValues = from sc in ValueList where (sc.CaseId == CaseId) select sc;
                Ct = CtValues.ToList().FirstOrDefault().Ct;
                string Description = CtValues.ToList().FirstOrDefault().CaseDescription;


                #region Ct
                ICalcLogEntry CtEntry = new CalcLogEntry();
                CtEntry.ValueName = "Ct";
                CtEntry.AddDependencyValue("StructureType ", " "+ Description);
                CtEntry.Reference = "";
                CtEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowThermalFactor.docx";
                CtEntry.FormulaID = null; //reference to formula from code
                CtEntry.VariableValue = Math.Round(Ct, 3).ToString();
                #endregion
                this.AddToLog(CtEntry);
            }
            catch (Exception)
            {
                Ct = 1.0;
            }
            return Ct;
        }
    }
}
