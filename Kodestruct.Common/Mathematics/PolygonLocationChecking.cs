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
using Kodestruct.Common.Maps;

namespace Kodestruct.Common.Mathematics
{

    public class PolygonLocationChecking
    {

        //http://stackoverflow.com/questions/217578/point-in-polygon-aka-hit-test

        //  int    polySides  =  how many corners the polygon has
        //  float  polyX[]    =  horizontal coordinates of corners
        //  float  polyY[]    =  vertical coordinates of corners
        //  float  x, y       =  point to be tested
        //
        private static bool pointInPolygon(int polySides, double[] polyX, double[] polyY, double x, double y)
        {
            int i;
            int j = polySides - 1;
            bool oddNodes = false;

            for (i = 0; i < polySides; i++)
            {
                if ((polyY[i] < y && polyY[j] >= y || polyY[j] < y && polyY[i] >= y) && (polyX[i] <= x || polyX[j] <= x))
                {
                    oddNodes ^= (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x);
                }
                j = i;
            }

            return oddNodes;
        }

        //public static bool IsLocationInPolygon(List<Location> polygon, Location checkPoint)
        public static bool IsLocationInPolygon(LocationCollection polygon, Location checkPoint)
        {
            List<double> xList = new List<double>();
            List<double> yList = new List<double>();

            int sideCount = -1;
            foreach (var location in polygon)
            {
                sideCount++;
                xList.Add(location.Latitude);
                yList.Add(location.Longitude);
            }

            return pointInPolygon(sideCount, xList.ToArray(), yList.ToArray(), checkPoint.Latitude, checkPoint.Longitude);

        }

        //LocationCollection
        //public static bool IsLocationInComplexPolygon(List<Location> mainPolygon, List<List<Location>> holes, Location checkPoint)
        public static bool IsLocationInComplexPolygon(LocationCollection mainPolygon, List<LocationCollection> holes, Location checkPoint)
        {
            if (checkPoint != null)
            {
                // check if point is inside boundary box
                double minX = mainPolygon[0].Latitude;
                double maxX = mainPolygon[0].Latitude;
                double minY = mainPolygon[0].Longitude;
                double maxY = mainPolygon[0].Longitude;

                foreach (var q in mainPolygon)
                {
                    minX = Math.Min(q.Latitude, minX);
                    maxX = Math.Max(q.Latitude, maxX);
                    minY = Math.Min(q.Longitude, minY);
                    maxY = Math.Max(q.Longitude, maxY);
                }

                if (checkPoint.Latitude < minX || checkPoint.Latitude > maxX || checkPoint.Longitude < minY || checkPoint.Longitude > maxY)
                {
                    // point is not inside boundary box, do not continue
                    return false;
                }

                // check if point is inside main polygon
                var result = IsLocationInPolygon(mainPolygon, checkPoint);

                // point is not inside main polygon, do not continue
                if (result == false) return false;

                // check if point is not inside of any hole
                if (holes != null)
                {
                    foreach (var holePolygon in holes)
                    {
                        var holeResult = IsLocationInPolygon(holePolygon, checkPoint);

                        if (holeResult)
                        {
                            // point is inside hole, that means it doesn't belong to complex polygon, return false
                            return false;
                        }
                    }
                }

                // if all tests passed then point is inside Polygon.
                return true;

            }
            else
            {
                return false;
            }
        }
    }
}
