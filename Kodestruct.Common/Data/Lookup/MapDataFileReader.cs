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
using System.IO;
using System.Reflection;
using Kodestruct.Common.Mathematics;


namespace Kodestruct.Common.Data
{

    public partial class MapDataFileReader
    {

        public int DataStartIndex { get; set; }
        public int DataEndIndex { get; set; }
        public int NumberOfColumns { get; set; }
        public decimal CoordinateIncrement { get; set; }
        ReadPointValueDelegate readPointValue;

        public MapDataFileReader(int DataStartIndex, int DataEndIndex, int NumberOfColumns, decimal CoordinateIncrement)
        {
            this.DataStartIndex = DataStartIndex;
            this.DataEndIndex = DataEndIndex;
            this.NumberOfColumns = NumberOfColumns;
            this.CoordinateIncrement = CoordinateIncrement;
        }

        protected MapDataElement ReadData(decimal Latitude, decimal Longitude, ReadPointValueDelegate ReadPointValue, decimal DataCoordinateStep)
        {
            this.readPointValue = ReadPointValue;
            //find the lower-left cornerpoint of a quad 
            decimal LatLow = Latitude.FloorWithSignificance(DataCoordinateStep);
            decimal LongLow = Longitude.FloorWithSignificance(DataCoordinateStep);

            //SeismicGroundMotionDataPoint pointFound = this.FindPointInDataFileRecursive(LatLow, LongLow, DataStartIndex+1, DataEndIndex);
            IMultipleValueDataPoint2D foundPoint = this.FindPointInDataFileRecursive(LatLow, LongLow, DataStartIndex + 1, DataEndIndex);
            MapDataElement de = new MapDataElement(Latitude, Longitude);
            //add neigboring points for interpolation

            #region Neigbouring points for interpolation
            #region Special cases
            //Corner points
            if (foundPoint.DataArrayIndex == DataStartIndex + 1 || foundPoint.DataArrayIndex == NumberOfColumns + 1 || foundPoint.DataArrayIndex == DataEndIndex || foundPoint.DataArrayIndex == DataEndIndex - NumberOfColumns + 1)
            {
                de.LowerLeftPoint = foundPoint;
                de.LowerRightPoint = foundPoint.Clone();
                de.LowerRightPoint.Longitude = de.LowerRightPoint.Longitude + CoordinateIncrement;
                de.UpperLeftPoint = foundPoint.Clone();
                de.UpperLeftPoint.Latitude = de.UpperLeftPoint.Latitude + CoordinateIncrement;
                de.UpperRightPoint = foundPoint.Clone();
                de.UpperRightPoint.Latitude = de.UpperRightPoint.Latitude + CoordinateIncrement;
                de.UpperRightPoint.Longitude = de.UpperRightPoint.Longitude + CoordinateIncrement;
                return de;
            }
            //bottom row- do nothing 
            //top row 
            if (foundPoint.DataArrayIndex < (NumberOfColumns + 1))
            {
                de.LowerLeftPoint = foundPoint;
                IMultipleValueDataPoint2D pNextLongitude = readPointValue(foundPoint.DataArrayIndex + 1);
                de.LowerRightPoint = pNextLongitude;
                IMultipleValueDataPoint2D NextLatitudePoint = foundPoint.Clone();
                NextLatitudePoint.Latitude = foundPoint.Latitude + CoordinateIncrement;
                de.UpperLeftPoint = NextLatitudePoint;
                IMultipleValueDataPoint2D NextLatitudeAndLongitudePoint = pNextLongitude.Clone();
                NextLatitudeAndLongitudePoint.Latitude = foundPoint.Latitude + CoordinateIncrement;
                NextLatitudeAndLongitudePoint.Longitude = foundPoint.Longitude + CoordinateIncrement;
                de.UpperRightPoint = NextLatitudeAndLongitudePoint;
                return de;
            }
            //first column -do nothing

            //last column
            if ((foundPoint.DataArrayIndex - DataStartIndex) % (NumberOfColumns) == 0 && foundPoint.DataArrayIndex > 2)
            {
                de.LowerLeftPoint = foundPoint;
                IMultipleValueDataPoint2D pNextLongitude = foundPoint.Clone();
                pNextLongitude.Longitude = foundPoint.Longitude + CoordinateIncrement;
                de.LowerRightPoint = pNextLongitude;
                IMultipleValueDataPoint2D pNextLatitude = readPointValue(foundPoint.DataArrayIndex - NumberOfColumns);
                de.UpperLeftPoint = pNextLatitude;
                IMultipleValueDataPoint2D NextLatitudeAndLongitudePoint = pNextLatitude.Clone();
                NextLatitudeAndLongitudePoint.Latitude = pNextLatitude.Latitude;
                NextLatitudeAndLongitudePoint.Longitude = pNextLongitude.Longitude;
                de.UpperRightPoint = NextLatitudeAndLongitudePoint;
                return de;
            }

            #endregion
            #region Typical case
                    de.LowerLeftPoint = foundPoint;
                    de.LowerRightPoint = readPointValue(foundPoint.DataArrayIndex + 1); //NextLongitudePoint;
                    de.UpperLeftPoint = readPointValue(foundPoint.DataArrayIndex - NumberOfColumns);  //NextLatitudePoint;
                    de.UpperRightPoint = readPointValue(foundPoint.DataArrayIndex - NumberOfColumns + 1); //NextLatitudeAndLongitudePoint 
            
            #endregion
            #endregion

            //Interpolation is done by MapDataElement


                        //if (pointFound!=null)
                        //{

                        //    decimal SS = GetInterpolatedValue(pointFound, Latitude, Longitude, GroundMotionParameterType.SS);
                        //    decimal S1 = GetInterpolatedValue(pointFound, Latitude, Longitude, GroundMotionParameterType.S1);

                        //    return new SeismicGroundMotionDataPoint(Latitude, Longitude, SS, S1);
                        //}

                        //return new SeismicGroundMotionDataPoint();
            return de;
        }
    }
}
