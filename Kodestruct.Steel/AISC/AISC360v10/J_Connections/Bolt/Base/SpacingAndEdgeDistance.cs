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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Data;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;
using Kodestruct.Steel.Properties;


namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public abstract partial class Bolt : BoltBase
    {
        protected double  GetStandardHoleMinimumEdgeDistance()
        {
            double s_Edge = 0.0;
            double D=0.0;
            D = this.Diameter < 0.5 ? 0.5 : this.Diameter;
            if (this.Diameter > 1.25 )
	            {
		             s_Edge = 1.25* this.Diameter;
	            }
             else
	            {
                    #region Read Hole Data

                    var SampleValue = new { Diameter = 0.0, EdgeDistance = 0.0};
                    var EdgeDistanceTableVals = ListFactory.MakeList(SampleValue);

                    using (StringReader reader = new StringReader(Resources.AISC360_10TableJ3_4_MinimumEdgeDistance))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] Vals = line.Split(',');
                            if (Vals.Length == 2)
                            {
                                double Diameter = double.Parse(Vals[0]);
                                double EdgeDistance = double.Parse(Vals[1]);
                                
                                EdgeDistanceTableVals.Add
                                (new
                                {
                                    Diameter = Diameter,
                                    EdgeDistance = EdgeDistance
                                }

                                );
                            }
                        }

                    }

                    #endregion

                    var ed = EdgeDistanceTableVals.First(l => l.Diameter == Diameter).EdgeDistance;
                    s_Edge = ed;
	            }

            return s_Edge;
        }
    }
}
