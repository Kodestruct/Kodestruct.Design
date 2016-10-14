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
 
using Kodestruct.Common.MapData;
namespace Kodestruct.Loads.ASCE7.Entities
{
    public class SSData : IMapData
    {
        MapDataInfo info;
        public SSData()
        {
            info = new MapDataInfo()
            {
                Key="SS",
                MainLayerUriFormat = "http://Kodestruct2.azurewebsites.net/MapsSources/ASCE7_10/Seismic/SS/Original/{0}.png",
                UnderlayUriFormat = "http://Kodestruct2.azurewebsites.net/MapsSources/ASCE7_10/Seismic/SS/Color/{0}.png",
                MaxLongitude = -65.0,
                MinLongitude =-125.0,
                MaxLatitude=50.0,
                MinLatitude=24.6,
                PixelHeight=509,
                PixelWidth=1201,
                RangeMaxValue = 370.08,
                RangeMinValue = 0.55
            };
              
            }
        


        public MapDataInfo  GetMapInfo()
        {
            return info;
        }


    }
}

