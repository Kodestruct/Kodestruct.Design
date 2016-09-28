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
    public abstract partial class ChsKConnection : ChsNonOverlapConnection
    {

        

        internal void DetermineTensionCompressionBranches(List<string> loadCaseList)
        {
            //prior to calculation of connection capacity need to determine
            //which branches are nesion and which are compression

            foreach (var caseName in loadCaseList)
            {
                HssTrussConnectionBranch compBranch = null;
                HssTrussConnectionBranch tensBranch = null;

                foreach (var b in Branches)
                {
                    //chnage this!!!!
                    double Fx = b.Forces.Where(f => f.LoadCaseName == caseName).ToList()[0].Fx; // first item in found forces
                    if (Fx<=0.0)
                    {
                        compBranch = b;
                    }
                    else
                    {
                        tensBranch = b;
                    }
                }

                HssKConnectionLoadCaseData data=null;
                
                if (compBranch!=null && tensBranch!=null)
                {
                     data = new HssKConnectionLoadCaseData(caseName, compBranch, tensBranch);
                }
                else
                {
                    throw new Exception("Failed to identify tension and compression branches in the connection");
                }

                

                if (this.loadCases == null)
	            {
                    loadCases = new Dictionary<string, HssKConnectionLoadCaseData>();
	            }
                loadCases.Add(caseName,data);
            }

        }

        //List<string> loadCaseList; 

        internal List<string> GetLoadCaseList()
        {
            List<string> CaseNamesB1;
            List<string> CaseNamesB2;

            if (Branches.Count==2)
	            {
		            CaseNamesB1 = GetUniqueCaseNames(Branches[0]);
                    CaseNamesB2 = GetUniqueCaseNames(Branches[1]);
                    if (CaseNamesB1!=CaseNamesB2)
                    {
                        throw new Exception("Case names are not same in branch 1 and 2");
                    }
                    
	            }
            else
	            {
                    throw new Exception ("Need exactly 2 branches for a K connection");
	            }
            return CaseNamesB1; 

        }

        internal List<string> GetUniqueCaseNames(HssTrussConnectionBranch branch)
        {
            List<string> caseNames = new List<string>();
            foreach (var f in branch.Forces)
            {
                if (caseNames.Contains(f.LoadCaseName)!=true)
                {
                    caseNames.Add(f.LoadCaseName);
                }
            }
            return caseNames;
        }

        //HssTrussConnectionBranch compressionBranch;
        //HssTrussConnectionBranch tensionBranch;

        //internal HssTrussConnectionBranch GetCompressionBranch(List<HssTrussConnectionBranch> branches, string LoadCaseName, bool JointAtMemberStart)
        //{
        //    //this method calculates P*sin(theta) term for compression branch
        //    // tension branch vertical component can then be compared to this term
        //    List<HssTrussConnectionBranch> compressionBranches = branches.Where(b =>
        //    {
        //        var thisCaseForces = b.Forces.Where(f => f.LoadCaseName == LoadCaseName).ToList();
        //        double FxJoint;
        //        if (JointAtMemberStart == true)
        //        {
        //            FxJoint = thisCaseForces.ElementAt(0).Fx;
        //        }
        //        else
        //        {
        //            FxJoint = thisCaseForces.ElementAt(thisCaseForces.Count - 1).Fx;
        //        }
        //        if (FxJoint < 0.0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }).ToList();

        //    HssTrussConnectionBranch compressionBranch;

        //    if (compressionBranches != null)
        //    {
        //        //if (compressionBranches.Count > 1)
        //        //{
        //        //    // warn user that more than one compression branch is in compression
        //        //    throw new Exception("More than one branch is in compression");
        //        //}
        //        //else
        //        //{
        //        //    compressionBranch = compressionBranches[0];
        //        //} 
        //        compressionBranch = compressionBranches.Min(b =>
        //        {
        //            ISectionPipe pipe = b.Section as ISectionPipe;
        //            if (pipe != null)
        //            {
        //                return pipe.D;
        //            }
        //            else
        //            {
        //                return 0.0;
        //            }
        //        });
        //    }
        //    else
        //    {
        //        throw new Exception("No compression branches were found");
        //    }

        //    this.compressionBranch = compressionBranch;

        //    return compressionBranch;
        //}

        //internal bool BranchIsTension(HssTrussConnectionBranch branch, string LoadCaseName)
        //{

        //    List<IMemberForce> thisLoadCaseForces = branch.GetForce(LoadCaseName);
        //    double FxMax = thisLoadCaseForces.Max(v => v.Fx);
        //    double FxMin = thisLoadCaseForces.Min(v => v.Fx);

        //    double Fx = Math.Abs(FxMax) > Math.Abs(FxMin) ? FxMax : FxMin; // this approach here finds the maximum value in the branch
        //    // the correct logic would be to find the force at the start / end  in other words at the connection

        //    if (Fx >= 0.0)
        //    {
        //        //this is tension branch
        //        return true;
        //    }
        //    else
        //    {
        //        //this is compression branch
        //        return false;
        //    }

        //}
    }
}
