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
using System.Globalization;
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

       protected double d_hWidth  {get; set;}
       protected double d_hLength { get; set; }
       protected bool BoltHoleSizeCalculated { get; set; }

        public double GetBoltHoleWidth(BoltHoleType HoleType, bool IsTensionOrShearCalculation = true)
        {
            double d_h = 0.0;
            if (BoltHoleSizeCalculated == false)
            {
                GetBoltHoleDimensions(HoleType, IsTensionOrShearCalculation);
            }
            return d_hWidth;
        }

        public double GetBoltHoleLength(BoltHoleType HoleType, bool IsTensionOrShearCalculation = true)
        {
            double d_h = 0.0;

            if (BoltHoleSizeCalculated == false)
            {
                GetBoltHoleDimensions(HoleType, IsTensionOrShearCalculation);
            }
            return d_hLength;

        }

        protected virtual void GetBoltHoleDimensions(BoltHoleType HoleType, bool IsTensionOrShearCalculation=true)
        {

            #region Read Table Data

            //<Diameter><Standard>< Oversize><Short-Slot Width><Short-Slot Length><Long-Slot Width><Long-Slot Length>
            var Tv11 = new { d_b = 0.0, STD = 0.0, OVS = 0.0, SSL_Width = 0.0, SSL_Length = 0.0, LSL_Width = 0.0, LSL_Length = 0.0,}; // sample
            var AllBoltsList = ListFactory.MakeList(Tv11);

            using (StringReader reader = new StringReader(Resources.AISC360_10TableJ3_3NominalHoleDimensions))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 7)
                    {
                        double V0 = double.Parse(Vals[0], CultureInfo.InvariantCulture);
                        double V1 = double.Parse(Vals[1], CultureInfo.InvariantCulture);
                        double V2 = double.Parse(Vals[2], CultureInfo.InvariantCulture);
                        double V3 = double.Parse(Vals[3], CultureInfo.InvariantCulture);
                        double V4 = double.Parse(Vals[4], CultureInfo.InvariantCulture);
                        double V5 = double.Parse(Vals[5], CultureInfo.InvariantCulture);
                        double V6 = double.Parse(Vals[6], CultureInfo.InvariantCulture);


                        AllBoltsList.Add(new { 
                            d_b =           V0,
                            STD =           V1,
                            OVS =           V2,
                            SSL_Width =     V3,
                            SSL_Length =    V4,
                            LSL_Width =     V5,
                            LSL_Length =    V6,
                        
                        });
                    }
                }

            }

            #endregion

            if (Diameter >= 1 + 1.0 / 8.0)
            {
                switch (HoleType)
                {
                    case BoltHoleType.STD:
                        d_hWidth = Diameter + 1 / 16.0;
                        d_hLength = Diameter + 1 / 16.0;
                        break;
                    case BoltHoleType.SSL_Perpendicular:
                        d_hWidth = Diameter + 1 / 16.0;
                        d_hLength = Diameter +3 / 8.0;
                        break;
                    case BoltHoleType.SSL_Parallel:
                        d_hWidth = Diameter + 1 / 16.0;
                        d_hLength = Diameter +3 / 8.0;
                        break;
                    case BoltHoleType.OVS:
                        d_hWidth = Diameter + 5 / 16.0;
                        d_hLength = Diameter +5 / 16.0;
                        break;
                    case BoltHoleType.LSL_Perpendicular:
                        d_hWidth = Diameter + 1 / 16.0;
                        d_hLength = Diameter*2.5;
                        break;
                    case BoltHoleType.LSL_Parallel:
                        d_hWidth = Diameter + 1 / 16.0;
                       d_hLength = Diameter*2.5;
                        break;
                }
            }
            else
            {


                var closest_d_b = AllBoltsList.Aggregate((x, y) => Math.Abs(x.d_b - Diameter) < Math.Abs(y.d_b - Diameter) ? x : y);
                switch (HoleType)
                {
                    case BoltHoleType.STD:
                        d_hWidth = closest_d_b.STD;
                        d_hLength = closest_d_b.STD;
                        break;
                    case BoltHoleType.SSL_Perpendicular:
                        d_hWidth = closest_d_b.SSL_Width;
                        d_hLength = closest_d_b.SSL_Length;
                        break;
                    case BoltHoleType.SSL_Parallel:
                        d_hWidth = closest_d_b.SSL_Width;
                        d_hLength = closest_d_b.SSL_Length;
                        break;
                    case BoltHoleType.OVS:
                        d_hWidth = closest_d_b.OVS;
                        d_hLength = closest_d_b.OVS;
                        break;
                    case BoltHoleType.LSL_Perpendicular:
                        d_hWidth = closest_d_b.LSL_Width;
                        d_hLength = closest_d_b.LSL_Length;
                        break;
                    case BoltHoleType.LSL_Parallel:
                        d_hWidth = closest_d_b.LSL_Width;
                        d_hLength = closest_d_b.LSL_Length;
                        break;
                }
            }
            // Per AISC add 
            if (IsTensionOrShearCalculation==true)
            {
                d_hWidth = d_hWidth+ 1.0/16.0;
                d_hLength = d_hLength + 1.0 / 16.0; 
            }

            BoltHoleSizeCalculated = true;

        }
    }
    
}
