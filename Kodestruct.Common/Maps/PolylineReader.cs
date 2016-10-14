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
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;
using Kodestruct.Common.Maps;
using Kodestruct.Common.Data;
using Kodestruct.Common.Mathematics;


namespace Kodestruct.Common.MapData
{
    public class PolylineReader
    {
        
        public static List<MapPolyline> ReadPolylineData(string stringData, string LayerName)
        {
            List<MapPolyline> Polylines = new List<MapPolyline>();
            string aLine = null;
            int polyIterator = 0;
            var polydata = new { AttributeData = "", XData = "", YData = "" }; // sample
            var polyList = ListFactory.MakeList(polydata);
            string attrDat = null; string xDat = null; string yDat = null;


            StringReader strReader = new StringReader(stringData);
            while (true)
            {
                aLine = strReader.ReadLine();
                if (aLine != null)
                {
                    polyIterator++;
                    polyIterator = polyIterator > 3 ? 1 : polyIterator;
                    switch (polyIterator)
                    {
                        case 1:
                            attrDat = aLine;
                            break;
                        case 2:
                            xDat = aLine;
                            break;
                        case 3:
                            yDat = aLine;
                            polyList.Add(new { AttributeData = attrDat, XData = xDat, YData = yDat });
                            attrDat = null; xDat = null; yDat = null;
                            break;
                    }
                }
                else
                {
                    //process all the data
                    if (polyList.Count > 1)
                    {
                        foreach (var polyDatElem in polyList)
                        {
                            MapPolyline polyline = new MapPolyline();
                            polyline.Locations = new LocationCollection();

                            string[] attVals = polyDatElem.AttributeData.Split(','); // not used for now, use later for lineweight an annotation
                            string[] xVals = polyDatElem.XData.Split(',');
                            string[] yVals = polyDatElem.YData.Split(',');
                            if (xVals.Length == yVals.Length)
                            {
                                for (int i = 0; i < xVals.Length; i++)
                                {
                                    double x;
                                    double y;
                                    if (Double.TryParse(xVals[i], out x) && Double.TryParse(yVals[i], out y)) // if done, then is a number
                                    {
                                        Location thisLoc = new Location(x, y);
                                        polyline.Locations.Add(thisLoc);
                                    }
                                }
                               Polylines.Add(polyline);
                            }
                            else
                            {
                                throw new Exception("Number of X coordinates and Y coordinates in data does not match");
                            }
                        }
                    }
                    break;
                }
            }
            return Polylines;
        }

        public static List<MapAnalyticalPolygon> ReadAnalyticalPolygonData(XDocument xmlString)
        {
            List<MapAnalyticalPolygon> polygonData = new List<MapAnalyticalPolygon>();


            var collection = from nod in xmlString.Descendants("MultiPolygon") select nod;
            foreach (var multiPolygon in collection)
            {
                MapAnalyticalPolygon ap = new MapAnalyticalPolygon();
                List<LocationCollection> innerLoops = new List<LocationCollection>();
                LocationCollection outerLoop = new LocationCollection();

                var Loops = from poly in multiPolygon.Descendants("Polygon") select poly;
                var LoopList = multiPolygon.Descendants("Polygon")
                    .Select(p => GetLocationData(p.Value))
                    .ToList();
                
                if (LoopList.Count==1)
                {
                    ap.OuterLoop = LoopList[0];
                }
                else
                {
                    for (int i = 0; i < LoopList.Count; i++)
                    {
                        var poly = LoopList[i];
                        bool IsOuter = DetermineIfPolygonIsOuter(poly, LoopList,i);
                        if (IsOuter == true)
                        {
                            ap.OuterLoop = poly;
                        }
                        else
                        {
                            ap.InnerLoops.Add(poly);
                        }
                    }
                }

                ap.ObjectId = multiPolygon.Attribute("ObjectId").Value;
                polygonData.Add(ap);
            }

            return polygonData;
        }

        private static bool DetermineIfPolygonIsOuter(LocationCollection poly, List<LocationCollection> LoopList, int currentIndex)
        {
            //Assumption is that all points are inside.
            //therefore it is necessary to only test one point
            bool IsInside = false;
            bool FoundPolyOutsideThisOne = false;
            Location point = poly[0];

            for (int i = 0; i < LoopList.Count; i++)
            {
                if (i!=currentIndex) // make sure to exclude current loop from checking
                {
                    LocationCollection loop = LoopList[i];
                    IsInside = PolygonLocationChecking.IsLocationInComplexPolygon(loop, null, point);
                    if (IsInside == true)
                    {
                        FoundPolyOutsideThisOne = true;
                        break;
                    }
                }
            }
            if (FoundPolyOutsideThisOne==true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static List<MapMultiPolygon> ReadPolygonData(XDocument xmlString)
        {
            List<MapMultiPolygon> polygonData = new List<MapMultiPolygon>();
            

            var collection = from nod in xmlString.Descendants("MultiPolygon") select nod;
                foreach (var multiPolygon in collection)
	            {
                    List<LocationCollection> points = new List<LocationCollection>();
		            var Loops = from poly in multiPolygon.Descendants("Polygon") select poly;
                    foreach (var polygon in Loops)
                    {
                        LocationCollection locations = GetLocationData(polygon.Value);
                        points.Add(locations);
                    }
                    MapMultiPolygon multiP = new MapMultiPolygon();
                    var IdAtt = multiPolygon.Attribute("ObjectId");
                    multiP.ObjectId = IdAtt.Value;
                    multiP.Vertices = points;
                    polygonData.Add(multiP);
	            }
            
            return polygonData;
        }


        private static LocationCollection GetLocationData(string coordinateStringData)
        {
            LocationCollection locations = new LocationCollection();
            string[] coordVals = coordinateStringData.Split(',');
            int numberOfCoordEntries = coordVals.Length;
            double Latitude, Longitude;

            if ( numberOfCoordEntries % 2 == 0 ) 
            {
                for (int i = 0; i < coordVals.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        Latitude = double.Parse(coordVals[i], CultureInfo.InvariantCulture);
                        Longitude = double.Parse(coordVals[i + 1], CultureInfo.InvariantCulture);
                        Location l = new Location(Latitude, Longitude);
                        locations.Add(l);
                    }
                }
            }
            return locations;
        }
        
    }


}
