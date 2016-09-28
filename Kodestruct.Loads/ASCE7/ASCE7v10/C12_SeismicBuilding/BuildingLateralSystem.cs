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
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Data;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.Properties;
using Kodestruct.Loads.ASCE7.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities.Exceptions;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public class BuildingLateralSystem: LateralSystem
    {

        public BuildingLateralSystem(string SystemId, SeismicDesignCategory Sdc, ICalcLog CalcLog)
            : base(CalcLog)
        {
            this.SeismicLateralSystemId = SystemId;
            this.SeismicDesignCategory = Sdc;
            if (SystemId!=null)
            {
                this.ReadTableData(); 
            }
        }

        public BuildingLateralSystem(string SystemId, ICalcLog CalcLog)
            : this(SystemId, SeismicDesignCategory.None, CalcLog)
        {
        }

        string SeismicLateralSystemId { get; set; }
        SeismicDesignCategory SeismicDesignCategory { get; set; }
        string SystemDescription { get; set; }
        string DetailingSection { get; set; }
        //string ApplicabilityLimit { get; set; }

        private double _R;

        public override double R
        {
            get { return _R; }
            set { _R = value; }
        }

        private double _Omega_0;

        public override double Omega_0
        {
            get { return _Omega_0; }
            set { _Omega_0 = value; }
        }

        private double _Cd;

        public override double Cd
        {
            get { return _Cd; }
            set { _Cd = value; }
        }

        string systemApplicability;

        public override string SystemApplicability
        {
            get { return systemApplicability; }
            set { systemApplicability = value; }
        }

        

        private void ReadTableData()
        {

            #region Read Coefficient Data

            var SampleValue = new { SystemMark = "", SystemDescription = "", DetailingSection = "", R = "", Omega = "", Cd = "", LimitB = "", LimitC = "", LimitD = "", LimitE = "", LimitF = "", Notes = "" }; // sample
            var DesignCoefficientList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.ASCE7_10T12_2_1))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 12)
                    {
                        string sysmark = (string)Vals[0];
                        string SystemDescription = (string)Vals[1];
                        string DetailingSection = (string)Vals[2];
                        string R = (string)Vals[3];
                        string Omega = (string)Vals[4];
                        string Cd = (string)Vals[5];
                        string LimitB = (string)Vals[6];
                        string LimitC = (string)Vals[7];
                        string LimitD = (string)Vals[8];
                        string LimitE = (string)Vals[9];
                        string LimitF = (string)Vals[10];
                        string Notes = (string)Vals[11];

                        DesignCoefficientList.Add(new
                        {
                            SystemMark = sysmark,
                            SystemDescription = SystemDescription,
                            DetailingSection = DetailingSection,
                            R = R,
                            Omega = Omega,
                            Cd = Cd,
                            LimitB = LimitB,
                            LimitC = LimitC,
                            LimitD = LimitD,
                            LimitE = LimitE,
                            LimitF = LimitF,
                            Notes = Notes
                        });
                    }
                }

            }

            #endregion

            var DataValues = from coeff in DesignCoefficientList where (coeff.SystemMark == SeismicLateralSystemId) select coeff;
            var ResultList = (DataValues.ToList());

            try
            {

                if (ResultList.First() != null)
                {
                    this.SystemDescription = ResultList.First().SystemDescription;
                    this.DetailingSection = ResultList.First().DetailingSection;
                    this.R = double.Parse(ResultList.First().R, CultureInfo.InvariantCulture);
                    this.Cd = double.Parse(ResultList.First().Cd, CultureInfo.InvariantCulture);
                    this.Omega_0 = double.Parse(ResultList.First().Omega, CultureInfo.InvariantCulture);

                    
                    #region R
                    ICalcLogEntry REntry = new CalcLogEntry();
                    REntry.ValueName = "R";
                    REntry.AddDependencyValue("SeismicLateralSystemId", SeismicLateralSystemId);
                    REntry.AddDependencyValue("SystemDescription", SystemDescription);
                    REntry.Reference = "Seismic Response Coefficient";
                    REntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficentR.docx";
                    REntry.FormulaID = "Table 12.2-1"; //reference to formula from code
                    REntry.VariableValue = R.ToString();
                    #endregion
                    this.AddToLog(REntry);
                    
                    #region Cd
                    ICalcLogEntry CdEntry = new CalcLogEntry();
                    CdEntry.ValueName = "Cd";
                    CdEntry.AddDependencyValue("SeismicLateralSystemId", SeismicLateralSystemId);
                    CdEntry.AddDependencyValue("SystemDescription", SystemDescription);
                    CdEntry.Reference = "Deflection Amplification Factor";
                    CdEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicDeflectionAmplificationFactorCd.docx";
                    CdEntry.FormulaID = "Table 12.2-1"; //reference to formula from code
                    CdEntry.VariableValue = Cd.ToString();
                    #endregion
                    this.AddToLog(CdEntry);

                    
                    #region Omega_0
                    ICalcLogEntry Omega_0Entry = new CalcLogEntry();
                    Omega_0Entry.ValueName = "Omega_0";
                    Omega_0Entry.AddDependencyValue("SeismicLateralSystemId", SeismicLateralSystemId);
                    Omega_0Entry.AddDependencyValue("SystemDescription", SystemDescription);
                    Omega_0Entry.Reference = "";
                    Omega_0Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicOverstrengthFactorOmega_0.docx";
                    Omega_0Entry.FormulaID = null; //reference to formula from code
                    Omega_0Entry.VariableValue = Omega_0.ToString();
                    #endregion
                    this.AddToLog(Omega_0Entry);
                   

                    if (SeismicDesignCategory != SeismicDesignCategory.None)
                    {
                        switch (SeismicDesignCategory)
                        {
                            case SeismicDesignCategory.A:
                                this.SystemApplicability = "NL";
                                break;
                            case SeismicDesignCategory.B:
                                this.SystemApplicability = ResultList.First().LimitB;
                                break;
                            case SeismicDesignCategory.C:
                                this.SystemApplicability = ResultList.First().LimitC;
                                break;
                            case SeismicDesignCategory.D:
                                this.SystemApplicability = ResultList.First().LimitD;
                                break;
                            case SeismicDesignCategory.E:
                                this.SystemApplicability = ResultList.First().LimitE;
                                break;
                            case SeismicDesignCategory.F:
                                this.SystemApplicability = ResultList.First().LimitF;
                                break;
                            default:
                                break;
                        }

                        
                        #region ApplicabilityLimit
                        ICalcLogEntry ApplicabilityLimitEntry = new CalcLogEntry();
                        ApplicabilityLimitEntry.AddDependencyValue("SeismicLateralSystemId", SeismicLateralSystemId);
                        ApplicabilityLimitEntry.ValueName = "ApplicabilityLimit";
                        ApplicabilityLimitEntry.AddDependencyValue("SystemDescription", SystemDescription);
                        ApplicabilityLimitEntry.Reference = "Lateral system applicability limit";
                        ApplicabilityLimitEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicLateralSytemApplicabilityLimit.docx";
                        ApplicabilityLimitEntry.FormulaID = null; //reference to formula from code
                        ApplicabilityLimitEntry.VariableValue = SystemApplicability.ToString();
                        #endregion
                        this.AddToLog(ApplicabilityLimitEntry);
                    }
                }

            }
            catch
            {
                throw new ElementNotFoundInResourceTableException(this.SeismicLateralSystemId);
            }

        }

    }
}
