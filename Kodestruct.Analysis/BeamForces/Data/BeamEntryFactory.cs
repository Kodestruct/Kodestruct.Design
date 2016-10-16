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

namespace Kodestruct.Analysis
{
    public class BeamEntryFactory
    {
        public static void CreateEntry( string ValueName, double Value, BeamTemplateType TemplateType, int SubTypeIndex,
            Dictionary<string, double> DependencyValues, string CaseId, IAnalysisBeam beam, bool AddMax = false, bool AddMin = false)
        {
                    ICalcLogEntry Entry = new CalcLogEntry();

                    Entry.ValueName = ValueName;
                    Entry.VariableValue = Math.Round(Value, 3).ToString();

                    if (DependencyValues!=null)
                    {
                        foreach (var depVal in DependencyValues)
                        {
                            Entry.AddDependencyValue(depVal.Key, Math.Round(depVal.Value, 3));
                        } 
                    }
                    if (AddMax == true)
                    {
                        Entry.AddDependencyValue("Maximum", "Maximum");
                        Entry.AddDependencyValue("maximum", "Maximum"); 
                    }
                    if (AddMin==true)
                    {
                        Entry.AddDependencyValue("Maximum", "Minimum");
                        Entry.AddDependencyValue("maximum", "Minimum"); 
                    }
                    Entry.AddDependencyValue("CF", "1728");
                    Entry.Reference = "";
                    //Entry.DescriptionReference = beam.ResourceLocator.GetTemplatePath(TemplateType, CaseId, SubTypeIndex);
                    Entry.FormulaID = null; //reference to formula from code

                    //beam.Log.AddEntry(Entry); // this bypasses the check for LogMode
                    beam.AddToLog(Entry);

        }

    }
}
