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
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Common.Entities
{
    public class AnalyticalElementForceBased: AnalyticalElement, IForceBased
    {
        public AnalyticalElementForceBased(ICalcLog CalcLog): base (CalcLog)
        {

        }

        private List<ICombinationResult> combinationResults;

        public List<ICombinationResult> CombinationResults
        {
            get { return combinationResults; }
        }
        
        public void AddCombinationResult(ICombinationResult CombinationResult)
        {
            if (combinationResults !=null)
            {
                IEnumerable<ICombinationResult> comboQuery =
                combinationResults.Where(cr => cr.CombinationName == CombinationResult.CombinationName); //.OrderBy(n => n);
                if (comboQuery.Count()!=0)
                {
                    throw new Exception("Combination with such name already exists");
                }
                else
                {
                    combinationResults.Add(CombinationResult);
                }
            }
            else
            {
                combinationResults = new List<ICombinationResult>();
                combinationResults.Add(CombinationResult);
            }
        }

        public void AddForce(IForce Force)
        {
            if (CombinationResults == null)
            {
                combinationResults = new List<ICombinationResult>();
                CombinationResult result = new CombinationResult(null);
                combinationResults.Add(result);
            }
            ICombinationResult r = CombinationResults.ElementAt(0);
            r.AddForce(Force);
        }

        public void AddForce(string CombinationName, IForce Force)
        {
            IEnumerable<ICombinationResult> comboQuery =
            combinationResults.Where(cr => cr.CombinationName == CombinationName);
            if (comboQuery.Count() == 0)
            {
                CombinationResult result = new CombinationResult(CombinationName);
                result.Forces.Add(Force);
            }
            else
            {
                if (comboQuery.Count() > 1)
                {
                    throw new Exception("More than 1 combination with that name detected.");
                }
                else
                {
                    ICombinationResult r = comboQuery.ElementAt(0);
                    r.Forces.Add(Force);
                }
            }
        }


        //public double FindMaximumForceValue(ForceType ForceType, bool IgnoreSign)
        //{
        //    double maxValue=0.0;
        //    switch (ForceType)
        //    {
        //        case ForceType.F1:
        //            maxValue = combinationResults.SelectMany(x => x.Forces).Max(y => {y.F1;});
        //           break;
        //        case ForceType.F2:
        //           maxValue = combinationResults.SelectMany(x => x.Forces).Max(y => y.F2);
        //            break;
        //        case ForceType.F3:
        //            maxValue = combinationResults.SelectMany(x => x.Forces).Max(y => y.F3);
        //            break;
        //        case ForceType.M1:
        //            maxValue = combinationResults.SelectMany(x => x.Forces).Max(y => y.M1);
        //            break;
        //        case ForceType.M2:
        //            maxValue = combinationResults.SelectMany(x => x.Forces).Max(y => y.M2);
        //            break;
        //        case ForceType.M3:
        //            maxValue = combinationResults.SelectMany(x => x.Forces).Max(y => y.M3);
        //            break;
        //    }
        //    return maxValue;
        //}

        public IForce FindMaximumForce(ForceType ForceType, bool IgnoreSign)
        {
            string ComboName = null;
            return this.FindMaximumForce(ForceType, IgnoreSign, ref ComboName);
        }


        public IForce FindMinimumForce(ForceType ForceType, bool IgnoreSign)
        {
            string ComboName = null;
            return this.FindMinimumForce(ForceType, IgnoreSign, ref ComboName);
        }

        public IForce FindMaximumForce(ForceType ForceType, bool IgnoreSign, ref string CombinationName)
        {
            double MaxVal = double.NegativeInfinity;
            IForce cForce =null;
            double forceValue = 0.0;
            foreach (var combinationResult in CombinationResults)
            {
                foreach (var f in combinationResult.Forces)
                {
                    switch (ForceType)
                    {
                        case ForceType.F1:
                            forceValue = IgnoreSign == true ? Math.Abs(f.F1) : f.F1;
                            break;
                        case ForceType.F2:
                            forceValue = IgnoreSign == true ? Math.Abs(f.F2) : f.F2;
                            break;
                        case ForceType.F3:
                            forceValue = IgnoreSign == true ? Math.Abs(f.F3) : f.F3;
                            break;
                        case ForceType.M1:
                            forceValue = IgnoreSign == true ? Math.Abs(f.M1) : f.M1;
                            break;
                        case ForceType.M2:
                            forceValue = IgnoreSign == true ? Math.Abs(f.M2) : f.M2;
                            break;
                        case ForceType.M3:
                            forceValue = IgnoreSign == true ? Math.Abs(f.M3) : f.M3;
                            break;
                    }
                    if (forceValue > MaxVal)
                    {
                        MaxVal = forceValue;
                        CombinationName = combinationResult.CombinationName;
                        cForce = f;
                    }
                    
                }
            }
            return cForce;
        }

        public IForce FindMinimumForce(ForceType ForceType, bool IgnoreSign, ref string CombinationName)
        {
            double MinVal = double.PositiveInfinity;
            IForce cForce = null;
            double forceValue = 0.0;
            foreach (var combinationResult in CombinationResults)
            {
                foreach (var f in combinationResult.Forces)
                {
                    switch (ForceType)
                    {
                        case ForceType.F1:
                            forceValue = IgnoreSign == true ? Math.Abs(f.F1) : f.F1;
                            break;
                        case ForceType.F2:
                            forceValue = IgnoreSign == true ? Math.Abs(f.F2) : f.F2;
                            break;
                        case ForceType.F3:
                            forceValue = IgnoreSign == true ? Math.Abs(f.F3) : f.F3;
                            break;
                        case ForceType.M1:
                            forceValue = IgnoreSign == true ? Math.Abs(f.M1) : f.M1;
                            break;
                        case ForceType.M2:
                            forceValue = IgnoreSign == true ? Math.Abs(f.M2) : f.M2;
                            break;
                        case ForceType.M3:
                            forceValue = IgnoreSign == true ? Math.Abs(f.M3) : f.M3;
                            break;
                    }
                    if (forceValue < MinVal)
                    {
                        MinVal = forceValue;
                        CombinationName = combinationResult.CombinationName;
                        cForce = f;
                    }

                }
            }
            return cForce;
        }

      }
}
