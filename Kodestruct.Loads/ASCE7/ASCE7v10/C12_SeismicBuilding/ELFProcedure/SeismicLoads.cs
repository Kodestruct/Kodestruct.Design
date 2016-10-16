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

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class SeismicLateralForceResistingStructure : AnalyticalElement
    {

        
        public List<double> CalculateSeismicLoads(double T, double Cs, List<double> StoryElevationsFromBase, List<double> StoryWeights )
        {
            List<double> loads = new List<double>();
            if (StoryElevationsFromBase.Count() != StoryWeights.Count())
            {
                throw new Exception("Lists for StoryElevationsFromBase and StoryWeights must have the same number of elements");
            }
            else
            {
                List<StorySeismicData> storyData = new List<StorySeismicData>();
                for (int i = 0; i < StoryElevationsFromBase.Count(); i++)
                {
                    storyData.Add(new StorySeismicData()
                        {
                            ElevationFromBase = StoryElevationsFromBase[i],
                            SeismicWeight = StoryWeights[i]
                        });
                }

                double k = GetBuildingPeriodExponent_k(T);
                loads = CalculateSeismicLevelLoads(k, Cs, storyData);
            }

            return loads;
        }
        public List<double>  CalculateSeismicLoads( double T, double Cs, List<StorySeismicData> storyData)
        {
            double k = GetBuildingPeriodExponent_k(T);
            List<double> loads = CalculateSeismicLevelLoads(k, Cs, storyData);
            return loads;
        }

        public List<double>  CalculateSeismicLevelLoads( double k, double Cs, List<StorySeismicData> storyData)
        {
            List<StorySeismicLoad> loads = new List<StorySeismicLoad>();

            //Sum(w_i*h_i^k)
            int N = storyData.Count();
            double Sum_w_h_k = 0.0;
            double W = 0.0;

            foreach (var story in storyData)
            {
                double wi = story.SeismicWeight;
                double hi = story.ElevationFromBase;

                Sum_w_h_k = Sum_w_h_k + wi * Math.Pow(hi, k);
                W = W + wi;
            }

            double Vb = this.GetBaseShearVb(Cs, W);
            List<double> StoryForces = new List<double>();

            foreach (var story in storyData)
            {
                double wx = story.SeismicWeight;
                double hx = story.ElevationFromBase;

                //(12.8-12)
                double Cvx = wx * Math.Pow(hx, k) / Sum_w_h_k;

                //(12.8-11)
                double Fx = Cvx * Vb;

                StorySeismicLoad load = new StorySeismicLoad()
                    {
                        Cvx = Cvx,
                        Fx = Fx,
                        StoryId = story.StoryId,
                        ElevationFromBase = story.ElevationFromBase,
                        Weight = story.SeismicWeight
                    };
                
                loads.Add(load);

                StoryForces.Add(Fx);
            }

            return StoryForces;
        }
    }
}
