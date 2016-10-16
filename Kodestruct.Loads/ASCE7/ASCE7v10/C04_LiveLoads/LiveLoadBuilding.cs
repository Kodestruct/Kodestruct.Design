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
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Loads.ASCE.ASCE7_10.LiveLoads
{
    public class LiveLoadBuilding : AnalyticalElement
    {
        public LiveLoadBuilding()
        {
                
        }
        public LiveLoadBuilding(ICalcLog CalcLog)
            : base(CalcLog)
        {

        }

        private double _q_L;

        public double q_L
        {
            get { return _q_L; }
            set { _q_L = value; }
        }
        
        public double GetLiveLoad(string AreaOccupancyId, bool IncludePartitions = false, double qp = 0.0)
        {

            // Calculation

            #region Read Occupancy Data

            var SampleValue = new { OccId = "", OccDescription = "", LoadString = "", Reduction = "", Notes = "" }; // sample
            var LiveLoadTableVals = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.ASCE7_10T4_1LiveLoads))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Length == 4 || Vals.Length == 5)
                    {
                        string _OccId = (string)Vals[0];
                        string _OccDescription = (string)Vals[1];
                        string _LoadString = (string)Vals[2];
                        string _Reduction = (string)Vals[3];
                        string _Notes = null;
                        if (Vals.Length == 5)
                        {
                            _Notes = (string)Vals[4];
                        }
                        LiveLoadTableVals.Add
                        (new
                        {
                            OccId = _OccId,
                            OccDescription = _OccDescription,
                            LoadString = _LoadString,
                            Reduction = _Reduction,
                            Notes = _Notes
                        }

                        );
                    }
                }

            }

            #endregion

            var LiveLoadEntryData = LiveLoadTableVals.First(l => l.OccId == AreaOccupancyId);

            double q = 0.0;
            double ql = 0.0;

            if (LiveLoadEntryData != null)
            {
                double LoadVal;
                if (LiveLoadEntryData.Notes=="AASHTO")
                {
                    q = 0.0;
                    
                    #region q
                    ICalcLogEntry qEntry = new CalcLogEntry();
                    qEntry.ValueName = "q";
                    qEntry.Reference = "";
                    qEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Live/LiveLoadAASHTO.docx";
                    qEntry.FormulaID = null; //reference to formula from code
                    qEntry.VariableValue = Math.Round(q, 3).ToString();
                    #endregion
                    this.AddToLog(qEntry);
                }
                else
                {
                    if (Double.TryParse(LiveLoadEntryData.LoadString, out LoadVal))
                    {
                        ql = LoadVal;
                        if (IncludePartitions == true)
                        {
                            q = ql + qp;
                        }
                        else
                        {
                            q = ql;
                        }
                        #region q
                        ICalcLogEntry qEntry = new CalcLogEntry();
                        qEntry.ValueName = "q";
                        qEntry.AddDependencyValue("OccupancyDescription", LiveLoadEntryData.OccDescription);
                        qEntry.FormulaID = null; //reference to formula from code
                        qEntry.VariableValue = Math.Round(q, 3).ToString();

                        string templatePath = null;

                        if (LiveLoadEntryData.Reduction == "P") //live load reduction permitted
                        {
                            templatePath = "/Templates/Loads/ASCE7_10/Live/LiveLoadUniform";
                        }
                        else
                        {
                            templatePath = "/Templates/Loads/ASCE7_10/Live/LiveLoadUniformReductionNotPermitted";
                        }
                        if (LiveLoadEntryData.Notes != null)
                        {
                            templatePath = templatePath + "WithNote";
                            qEntry.AddDependencyValue("OccupancyNotes", LiveLoadEntryData.Notes);
                        }
                        if (IncludePartitions == true)
                        {
                            templatePath = templatePath + "WithPartitions";
                            qEntry.AddDependencyValue("ql", Math.Round(ql, 3).ToString());
                            qEntry.AddDependencyValue("qp", Math.Round(qp, 3).ToString());
                        }

                        #endregion
                        qEntry.DescriptionReference = templatePath + ".docx";
                        this.AddToLog(qEntry);
                    }
                    else
                    {
                        q = 0;
                    } 
                }
            }
            _q_L = q;
            return q;
        }
    }
}
