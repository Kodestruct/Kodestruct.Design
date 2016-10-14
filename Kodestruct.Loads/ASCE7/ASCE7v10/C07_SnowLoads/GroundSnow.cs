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
using System.Threading.Tasks;
using System.Xml.Linq;
using Kodestruct.Common.Entities;
using Kodestruct.Common.MapData;
using Kodestruct.Common.Maps;
using Kodestruct.Common.Mathematics;
using Kodestruct.Loads.ASCE7.Entities;
//using Kodestruct.Loads.ASCE7.ASCE7_10.C07_SnowLoads.MapData;

namespace Kodestruct.Loads.ASCE7.ASCE7_10.C07_SnowLoads
{
    public class GroundSnow: MapAnalyticalElement
    {
        SnowMapId MapId;

        
        public double GetGroundSnow(double latitude, double longitude, double z_s, string SnowMapId="US")
        {
            //this.MapId = Enum.Parse(typeof(Kodestruct.Loads.ASCE7.Entities.SnowMapId), SnowMapId);
            XDocument xmlShapeData  = 
                XDocument.Load("pack://application:,,,/ASCE7/ASCE7_10/C07_SnowLoads/MapData/GroundSnowData.xml");

            double pg=0.0;
            switch (SnowMapId)
            {
                default:
                    break;
            }
            //double pg;
            //                        case SnowMapId.US:
            //                        pg = GetGroundSnowForZoneASCE(SnowZoneId, Z, Latitude, Longitude, County);
            //                        break;
            //                    case SnowMapId.NY:
            //                        pg = GetGroundSnowForZoneNY(SnowZoneId, Z, Latitude, Longitude, County);
            
            throw  new NotImplementedException();

        }



        protected override XDocument ReadXmlPolygonFile(string LayerId)
        {
            XDocument regionDoc;
            switch (LayerId)
            {
                case "US":


                    regionDoc = XDocument.Load("pack://application:,,,/ASCE7/ASCE7_10/C07_SnowLoads/MapData/SnowRegionData.xml");
                    break;
                case "NY":
                    regionDoc = XDocument.Load("pack://application:,,,/ASCE7/ASCE7_10/C07_SnowLoads/MapData/StateRegionDataNY.xml");
                    break;
                default:
                    regionDoc = XDocument.Load("pack://application:,,,/ASCE7/ASCE7_10/C07_SnowLoads/MapData/SnowRegionData.xml");
                    break;
            }

            return regionDoc;
        } 
    }
}
