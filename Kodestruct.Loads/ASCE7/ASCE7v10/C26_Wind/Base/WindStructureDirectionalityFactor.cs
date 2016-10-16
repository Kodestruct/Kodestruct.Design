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
using Kodestruct.Loads.ASCE.ASCE7_10.WindLoads;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindStructure : AnalyticalElement
{
        public double GetKd(string StructureTypeId)
        {

            #region Read Table

            var SampleValue = new { StructureTypeId = "", Description="", Kd = "" }; // sample
            var KdList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.ASCE7_10F26_6_1Kd))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 3)
                    {
                        string thisStructureTypeId = (string)Vals[0];
                        string thisDescription = (string)Vals[1];
                        string thisKd = (string)Vals[2];

                        KdList.Add(new
                        {
                            StructureTypeId = thisStructureTypeId,
                            Description = thisDescription,
                            Kd = thisKd
                        });
                    }
                }

            }

            #endregion

            var tableValues = from KdEntry in KdList
                              where (KdEntry.StructureTypeId == StructureTypeId)
                              select KdEntry;
            var result = (tableValues.ToList()).FirstOrDefault();

            double Kd = 1.0;
            if (result != null)
            {
                Kd = double.Parse(result.Kd, CultureInfo.InvariantCulture);
                string SystemDescription = result.Description;
                
                #region Kd
                ICalcLogEntry KdEntry = new CalcLogEntry();
                KdEntry.ValueName = "Kd";
                KdEntry.AddDependencyValue("SystemDescription", SystemDescription);
                KdEntry.Reference = "";
                KdEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindDirectionalityFactor.docx";
                KdEntry.FormulaID = null; //reference to formula from code
                KdEntry.VariableValue = Math.Round(Kd, 2).ToString();
                #endregion
                this.AddToLog(KdEntry);
            }
            else
            {
                throw new ParameterNotFoundInTableException("Kd");
            }

            return Kd;
        }
    }
}
