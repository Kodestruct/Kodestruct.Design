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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;


namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads
{
    //multiple components
    public class ComponentAssembly : AnalyticalElement
    {
       

        public List<BuildingElementComponent> Components { get; set; }

        public ComponentAssembly(ICalcLog CalcLog)
            : base(CalcLog)
        {
            Components = new List<BuildingElementComponent>();
        }

        public double CalculateAssemblyWeight()
        {
            double qd=0.0;
            if (Components.Count<2)
            {
                //use single value template
                if (Components.Count==1)
                {
                    ComponentReportEntry entry= Components[0].GetComponentWeight();
                    //add to calc log entry
                    
                    #region qd
                    ICalcLogEntry qdEntry = new CalcLogEntry();
                    qdEntry.AddDependencyValue("ComponentDescription", entry.Description);
                    

                    qdEntry.Reference = "";
                    if (entry.ReferenceNotes!="")
                    {
                        qdEntry.AddDependencyValue("Reference", entry.ReferenceNotes);
                        if ( entry.LoadNotes != "")
                        {
                            qdEntry.AddDependencyValue("LoadNotes", entry.LoadNotes);
                            qdEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Dead/DeadComponentWeightSingle.docx";
                        }
                        else
                        {
                            qdEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Dead/DeadComponentWeightSingleNoNotes.docx";
                        }
                    }
                    else
                    {
                        if (entry.LoadNotes != "")
                        {
                            qdEntry.AddDependencyValue("LoadNotes", entry.LoadNotes);
                            qdEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Dead/DeadComponentWeightSingleNoRef.docx";
                        }
                        else
                        {
                            qdEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Dead/DeadComponentWeightSingleNoNotesNoRef.docx";
                        }
                        
                    }
                    
                    qdEntry.FormulaID = null; //reference to formula from code
                    qdEntry.ValueName = "q";
                    qdEntry.VariableValue = Math.Round(entry.LoadValue, 1).ToString();
                    #endregion
                    this.AddToLog(qdEntry);
                    qd = entry.LoadValue;
                }
            }
            else //if multiple building components are present
            {
                List<ComponentReportEntry> reportEntries = new List<ComponentReportEntry>();
                Components.ForEach(re => reportEntries.Add(re.GetComponentWeight()));


                List<List<string>> ReportTableData = new List<List<string>>();
                foreach (var en in reportEntries)
                {
                    List<string> row = new List<string>()
                    {
                        en.Description,
                        Math.Round(en.LoadValue,1).ToString(),
                        en.LoadNotes,
                        en.ReferenceNotes
                    };
                    ReportTableData.Add(row);
                    qd = qd + en.LoadValue;
                }
                
                #region qd
                ICalcLogEntry qdEntry = new CalcLogEntry();
                qdEntry.ValueName = "q";
                qdEntry.Reference = "";
                qdEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Dead/DeadComponentWeightTable.docx";
                qdEntry.FormulaID = null; //reference to formula from code
                qdEntry.VariableValue = Math.Round(qd, 3).ToString();
                qdEntry.TableData = ReportTableData;
                qdEntry.TemplateTableTitle = "s";
                #endregion
 
                this.AddToLog(qdEntry);
            }
            return qd;
        }
    }
}
