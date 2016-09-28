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
using System.Linq;
using System.Text; 
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
 using Kodestruct.Common.Mathematics;
 
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public abstract partial  class ChsKConnection : ChsNonOverlapConnection
    {
        public ChsKConnection(HssTrussConnectionChord chord, double Gap, List<HssTrussConnectionBranch> branches,  ICalcLog CalcLog)
            :base(chord, branches, CalcLog)
        {
            this.gap = Gap;
            if (branches.Count!=2)
            {
                throw new Exception("K connections other than ones with 2 members are not supported");
            }
            List<string> loadCaseNames = GetLoadCaseList();
            DetermineTensionCompressionBranches(loadCaseNames);
            
        }

        private Dictionary<string, HssKConnectionLoadCaseData> loadCases;

        public Dictionary<string, HssKConnectionLoadCaseData> LoadCases
        {
            get { return loadCases; }
            set { loadCases = value; }
        }

        private double gap;

        public double Gap
        {
            get { return gap; }
            set { gap = value; }
        }


        internal double GetVerticalComponentOfCompressionBranchChordPlastificationForce(string LoadCaseName, double D, double Qf, double Qg)
        {
            double PnSinTheta = 0;

            //var thisLoadCaseData = loadCases.Where(c => c.LoadCaseName == LoadCaseName).ToList()[0];
            HssKConnectionLoadCaseData thisLoadCaseData;
            loadCases.TryGetValue(LoadCaseName, out thisLoadCaseData);

            if (thisLoadCaseData != null)
            {
                HssTrussConnectionBranch branch = thisLoadCaseData.CompressionBranch;
                ISectionPipe section = GetBranchSection(branch);
                double Fy = branch.Section.Material.YieldStress;
                double t = section.t_des;
                double Db = section.D;

                //(K2-4)
                PnSinTheta = Fy*Math.Pow(t,2)*(2.0+11.33*Db/D)*Qg*Qf;
            }
            else
            {
                throw new Exception("Could not find branch data for the given load case name");
            }
            return PnSinTheta;
        }

        public double CheckChordPlastificationForCompressionBranch(string LoadCaseName, double D, double Qf, double Qg)
        {
            double P = 0.0;
            HssKConnectionLoadCaseData thisLoadCaseData;
            loadCases.TryGetValue(LoadCaseName, out thisLoadCaseData);
            if (thisLoadCaseData!=null)
            {
                HssTrussConnectionBranch branch = thisLoadCaseData.CompressionBranch;
                P= this.CheckChordPlastificationForBranch(branch, LoadCaseName, D, Qf, Qg);
            }
            else
            {
                throw new Exception("Could not find branch data for the given load case name");
            }

            return P;
        }

        public double CheckChordPlastificationForTensionBranch(string LoadCaseName, double D, double Qf, double Qg)
        {
            double P = 0.0;
            HssKConnectionLoadCaseData thisLoadCaseData;
            loadCases.TryGetValue(LoadCaseName, out thisLoadCaseData);
            if (thisLoadCaseData != null)
            {
                HssTrussConnectionBranch branch = thisLoadCaseData.TensionBranch;
                P = this.CheckChordPlastificationForBranch(branch, LoadCaseName, D, Qf, Qg);
            }
            else
            {
                throw new Exception("Could not find branch data for the given load case name");
            }

            return P;
        }

        internal double CheckChordPlastificationForBranch(HssTrussConnectionBranch branch, string LoadCaseName, double D, double Qf, double Qg)
        {
            double P = 0;
            double Pn;



            //var thisLoadCaseData = loadCases.Where(c => c.LoadCaseName == LoadCaseName).ToList()[0];

                
                double theta = branch.Angle;
                double sinTheta = Math.Sin(theta.ToRadians());
                double PnSinTheta = GetVerticalComponentOfCompressionBranchChordPlastificationForce(
                    LoadCaseName,D,Qf,Qg);

                Pn = PnSinTheta / sinTheta;
            


                P = 0.9 * Pn;

            return P;
        }



    }
}
