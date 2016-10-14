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
 
namespace Kodestruct.Common.MapData
{
    public class MapDataInfo
    {
        public string Key { get; set; }
        public double MinLongitude { get; set; }
        public double MinLatitude { get; set; }
        public double MaxLongitude { get; set; }
        public double MaxLatitude { get; set; }
        public double RangeMinValue { get; set; }
        public double RangeMaxValue { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public string UnderlayUriFormat { get; set; }
        public string MainLayerUriFormat { get; set; }
    }
}
