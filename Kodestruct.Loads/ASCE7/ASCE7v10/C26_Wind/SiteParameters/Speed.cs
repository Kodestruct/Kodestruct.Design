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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Loads.Properties;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindLocation : AnalyticalElement
    {
        public double GetWindSpeed(string ZoneId, BuildingRiskCategory RiskCategory, bool IsSpecialWindRegion)
        {

            double v = 0;
            string filename = null;
            string Figure = null;

            if (ZoneId == "0")
            {
                return -1; //error zone not found
            }

            else
            {

                #region Read Zone Data

                var SampleValue = new { ZoneId = "", LoadData = "" }; // sample
                var WindZoneList = ListFactory.MakeList(SampleValue);

                using (StringReader reader = new StringReader(Resources.ASCE7_10F26_5_1WindZones))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] Vals = line.Split(',');
                        if (Vals.Count() == 2)
                        {
                            string zone = (string)Vals[0];
                            string load = (string)Vals[1];


                            WindZoneList.Add(new
                            {
                                ZoneId = zone,
                                LoadData = load
                            });
                        }
                    }

                }

                #endregion

                switch (RiskCategory)
                {
                    case BuildingRiskCategory.I:
                        filename = "ASCE7_10WindEastCategoryI.txt";
                        Figure = "26.5-1C";
                        break;
                    case BuildingRiskCategory.II:
                        filename = "ASCE7_10WindEastCategoryII.txt";
                        Figure = "26.5-1A";
                        break;
                    case BuildingRiskCategory.III:
                        filename = "ASCE7_10WindEastCategoryIIIandIV.txt";
                        Figure = "26.5-1B";
                        break;
                    case BuildingRiskCategory.IV:
                        filename = "ASCE7_10WindEastCategoryIIIandIV.txt";
                        Figure = "26.5-1B";
                        break;
                }

                string loadStr = WindZoneList.First(z => z.ZoneId == ZoneId).LoadData.ToString();

                if (loadStr != "VAR")
                {
                    v = double.Parse(loadStr, CultureInfo.InvariantCulture);
                }

                else
                {

                    if (filename != null)
                    {
                        WindDataPoint wdp = FindClosestDataPoint(filename, Latitude, Longitude);
                        v = Math.Ceiling(wdp.WindSpeed);
                    }
                    else
                    {
                        v = -1;
                    }
                }
            }
            //Add CalcLogEntry

            #region v
            ICalcLogEntry vEntry = new CalcLogEntry();
            vEntry.ValueName = "v";
            vEntry.AddDependencyValue("Latitude", Math.Round(Latitude, 3));
            vEntry.AddDependencyValue("Longitude", Math.Round(Longitude, 3));
            //vEntry.AddDependencyValue("County", County);
            vEntry.AddDependencyValue("Figure", Figure);
            vEntry.AddDependencyValue("RiskCategory", RiskCategory.ToString());
            vEntry.Reference = "";
            if (County != null)
            {
                vEntry.AddDependencyValue("County", County);
                if (IsSpecialWindRegion==true)
                {
                    vEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindSpeedSWR.docx";
                }
                else
                {
                    vEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindSpeed.docx";
                }
                
            }
            else
            {
                if (IsSpecialWindRegion == true)
                {
                    vEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindSpeedNoCountySWR.docx";
                }
                else
                {
                    vEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindSpeedNoCounty.docx";
                }
                
            }
            vEntry.FormulaID = null; //reference to formula from code
            vEntry.VariableValue = Math.Round(v, 1).ToString();
            #endregion
            this.AddToLog(vEntry);
            return v;
        }

        private WindDataPoint FindClosestDataPoint(string Filename, double Latitude, double Longitude)
        {
            //string path = HostingEnvironment.MapPath("~/data");
            string path = "";
            
            string filepath = path + "/" + Filename;
            double MinDistance = double.PositiveInfinity;
            WindDataPoint targetPoint = new WindDataPoint() { Latitude = Latitude, Longitude = Longitude };
            WindDataPoint closestPoint = null;

            foreach (string line in File.ReadLines(filepath))
            {
                WindDataPoint dp = null;
                string[] Vals = line.Split(',');
                if (Vals.Count() == 3)
                {
                    double thisPtLongitude = Double.Parse(Vals[0], CultureInfo.InvariantCulture);
                    double thisPtLatitude = Double.Parse(Vals[1], CultureInfo.InvariantCulture);
                    double thisPtSpeed = Double.Parse(Vals[2], CultureInfo.InvariantCulture);
                    dp = new WindDataPoint() { Latitude = thisPtLatitude, Longitude = thisPtLongitude, WindSpeed = thisPtSpeed };
                }
                if (dp != null)
                {
                    double dist = FindDistanceBetween2DataPoints(targetPoint, dp);
                    if (dist < MinDistance)
                    {
                        closestPoint = dp;
                        MinDistance = dist;
                    }
                }
            }

            return closestPoint;

        }

        private double FindDistanceBetween2DataPoints(WindDataPoint PointA, WindDataPoint PointB)
        {
            double dist, X1, X2, Y1, Y2;
            X1 = PointA.Longitude;
            Y1 = PointA.Latitude;
            X2 = PointB.Longitude;
            Y2 = PointB.Latitude;

            dist = Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y2 - Y1, 2));
            return dist;
        }
        private class WindDataPoint
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double WindSpeed { get; set; }
        }
    }

}
