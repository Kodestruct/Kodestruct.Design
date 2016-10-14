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
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Entities;
using Kodestruct.Wood.NDS.NDS2015;
using Kodestruct.Wood.Properties;


namespace Kodestruct.Wood.NDS.NDS2015
{
    public abstract partial class WoodMember : AnalyticalElement
    {
        public double GetEffectiveLength(double l_u, double d, LateralBracingCondition BracingConditition,
            SupportCondition Support, LoadType LoadType, int NumberOfLoads = 0)
        {
            double l_e = double.PositiveInfinity;

            //adjust values
            SupportCondition bc = Support;
            LateralBracingCondition br = BracingConditition;
            int N = NumberOfLoads;

            if (bc == SupportCondition.Cantilever)
            {
                br = LateralBracingCondition.Unbraced;
            }
            if (bc == SupportCondition.SingleSpan)
            {
                if (LoadType ==  NDS2015.LoadType.UniformlyDistributed)
                {
                    br = LateralBracingCondition.Unbraced;
                }
                else
                {
                    if (br == LateralBracingCondition.Unbraced && N>2)
                    {
                        N = 2;
                    }
                    else
                    {
                        if (N>7)
                        {
                            N = 7;
                        }
                    }
                }
            }
            //Find value in table

            #region Read Effective Length Data

            var SampleValue = new 
            { BC = "", LoadType = "", Br = "", N=1,
                l_d_Less7_luFactor= 0.0,
                l_d_Less7_dFactor= 0.0, 
                l_d_Less14_luFactor = 0.0, 
                l_d_Less14_dFactor = 0.0,
                l_d_More14_luFactor = 0.0, 
                l_d_More14_dFactor = 0.0 
            }; // sample
            var EffectiveLengthTableVals = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.NDS2015_Table3_3_3EffectiveLength))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Length == 10)
                    {

                        string _BC =        Vals[0];
                        string _LoadType =  Vals[1];
                        string _Br =        Vals[2];
                        int _N =         int.Parse(Vals[3]);
                        double _l_d_Less7_luFactor  =double.Parse(Vals[4]);
                        double _l_d_Less7_dFactor = double.Parse(Vals[5]);
                        double _l_d_Less14_luFactor = double.Parse(Vals[6]);
                        double _l_d_Less14_dFactor = double.Parse(Vals[7]);
                        double _l_d_More14_luFactor = double.Parse(Vals[8]);
                        double _l_d_More14_dFactor  =double.Parse(Vals[9]);

                        EffectiveLengthTableVals.Add
                        (new
                            {
                                BC =        _BC ,        
                                LoadType =  _LoadType ,  
                                Br =        _Br ,
                                N=_N,
                                l_d_Less7_luFactor = _l_d_Less7_luFactor , 
                                l_d_Less7_dFactor  = _l_d_Less7_dFactor  , 
                                l_d_Less14_luFactor= _l_d_Less14_luFactor, 
                                l_d_Less14_dFactor = _l_d_Less14_dFactor , 
                                l_d_More14_luFactor= _l_d_More14_luFactor, 
                                l_d_More14_dFactor = _l_d_More14_dFactor  

                           }

                        );
                    }
                }

            }

            #endregion

            double lu_factor = 0.0;
            double dFactor = 0.0;

            var EffectiveLengthEntryData = EffectiveLengthTableVals.First(l => 
                l.BC==bc.ToString()
                && l.Br == br.ToString()
                && l.LoadType == LoadType.ToString()
                && l.N == NumberOfLoads);
                    if (EffectiveLengthEntryData != null)
                    {

                        if (l_u/4<7)
                        {
                           
                        }
                        else if (l_u<=14.3 )
                        {
                            
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        return double.PositiveInfinity;
                    }

                    l_e = lu_factor * l_u + dFactor * d;

                    return l_e;
        }
    }


}
