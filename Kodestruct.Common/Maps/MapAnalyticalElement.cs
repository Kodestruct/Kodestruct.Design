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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.MapData;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Common.Maps
{
    public abstract class MapAnalyticalElement : AnalyticalElement
    {
        public MapAnalyticalElement()
        {

        }

        public MapAnalyticalElement(ICalcLog CalcLog): base(CalcLog)
        {

        }

        Dictionary<string, List<MapAnalyticalPolygon>> polygonDictionary;

        public string GetActiveLocationId(Location point, string Layer)
        {
            string Id = null;
            if (Layer != null)
            {
                if (polygonDictionary == null)
                {
                    this.InitializeShapes(Layer);
                }
                if (polygonDictionary != null)
                {
                    List<MapAnalyticalPolygon> activeLayerData = polygonDictionary[Layer];
                    if (activeLayerData != null)
                    {
                        foreach (MapAnalyticalPolygon ap in activeLayerData)
                        {
                            bool IsInside = PolygonLocationChecking.IsLocationInComplexPolygon(ap.OuterLoop, ap.InnerLoops, point);
                            if (IsInside)
                            {
                                Id = ap.ObjectId;
                            }
                        }
                    }
                }
            }
            return Id;
        }

        public void InitializeShapes(string MapId)
        {
            CreateShapeLayer(MapId);
        }

        private void CreateShapeLayer(string MapId)
        {
            XDocument XmlShapeData = ReadXmlPolygonFile(MapId);
            Dictionary<string, XDocument> resourceData = new Dictionary<string, XDocument>();
            resourceData.Add(MapId, XmlShapeData);
            InitializeShapeResources(resourceData);

        }

        protected abstract XDocument ReadXmlPolygonFile(string MapId);

        public void InitializeShapeResources(Dictionary<string, XDocument> ShapeData)
        {

            foreach (var polygonData in ShapeData)
            {
                if (polygonDictionary.ContainsKey(polygonData.Key) == false)
                {
                    List<MapAnalyticalPolygon> polygons = PolylineReader.ReadAnalyticalPolygonData(polygonData.Value);
                    polygonDictionary.Add(polygonData.Key, polygons);
                }
            }

        }

    }
}
