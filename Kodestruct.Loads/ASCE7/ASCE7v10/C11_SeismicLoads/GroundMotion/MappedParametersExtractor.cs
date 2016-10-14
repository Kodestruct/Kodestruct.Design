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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE7.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.MapData;


namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class SeismicLocation : AnalyticalElement
    {
        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set { 
                longitude = value;
            IsDirty = true;
            }
        }

        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set 
            { 
                latitude = value;
                IsDirty = true;
            }
        }

        private List<MapDataInfo>  loadedMapsInfo;

        public List<MapDataInfo>  LoadedMapsInfo
        {
            get { return loadedMapsInfo; }
            set { 
                loadedMapsInfo = value;
                IsDirty = true;
            }
        }

        double SS;
        double S1;
        double TL;

        bool IsDirty;

        public SeismicLocation(double Latitude, double Longitude, List<MapDataInfo> LoadedMapsInfo, ICalcLog Log)
            :base(Log)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.LoadedMapsInfo = LoadedMapsInfo;
            IsDirty = true;
        }
        public double GetSS()
        {
            if (IsDirty==true)
            {
                CalculateSeismicValues();
            }

            //Add to log
            #region SS
            ICalcLogEntry SSEntry = new CalcLogEntry();
            SSEntry.ValueName = "SS";
            SSEntry.AddDependencyValue("Latitude", Math.Round(Latitude, 3).ToString());
            SSEntry.AddDependencyValue("Longitude", Math.Round(Longitude, 3).ToString());
            SSEntry.Reference = "MCER mapped short-period acceleration";
            SSEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicMappedSS.docx";
            SSEntry.FormulaID = null; //reference to formula from code
            SSEntry.VariableValue = SS.ToString();
            #endregion
            this.AddToLog(SSEntry);

            return SS;
        }
        public double GetS1()
        {
            if (IsDirty == true)
            {
                CalculateSeismicValues();
            }

            #region S1
            ICalcLogEntry S1Entry = new CalcLogEntry();
            S1Entry.ValueName = "Sone";
            S1Entry.AddDependencyValue("Latitude", Math.Round(Latitude, 3).ToString());
            S1Entry.AddDependencyValue("Longitude", Math.Round(Longitude, 3).ToString());
            S1Entry.Reference = "MCER mapped 1-second period acceleration";
            S1Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicMappedS1.docx";
            S1Entry.FormulaID = null; //reference to formula from code
            S1Entry.VariableValue = S1.ToString();
            #endregion
            this.AddToLog(S1Entry);

            return S1;
        }
        public double GetTL()
        {
            if (IsDirty == true)
            {
                CalculateSeismicValues();
            }


            #region TL
            ICalcLogEntry TLEntry = new CalcLogEntry();
            TLEntry.ValueName = "TL";
            TLEntry.AddDependencyValue("Latitude", Math.Round(Latitude, 3).ToString());
            TLEntry.AddDependencyValue("Longitude", Math.Round(Longitude, 3).ToString());
            TLEntry.Reference = "Long period transition period";
            TLEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicMappedTL.docx";
            TLEntry.FormulaID = null; //reference to formula from code
            TLEntry.VariableValue = TL.ToString();
            #endregion
            this.AddToLog(TLEntry);

            return TL;
        }


        private void CalculateSeismicValues()
        {
                decimal LatitideD = (decimal)Latitude;
                decimal LongitudeD = (decimal)Longitude;

                SeismicGroundMotionMapDataReader reader = new SeismicGroundMotionMapDataReader();
                MapDataElement dp = reader.ReadData(LatitideD, LongitudeD);
                decimal SSVal = dp.GetValue(GroundMotionParameterType.SS,true,1.05m, 6.0m);
                decimal S1Val = dp.GetValue(GroundMotionParameterType.S1,true,1.05m, 6.0m);
                decimal TLVal = dp.GetValue(GroundMotionParameterType.TL,true,1.05m, 6.0m);

                SS =(double) Math.Round(SSVal / 100.0m,3, MidpointRounding.AwayFromZero);
                S1 = (double) Math.Round(S1Val / 100.0m,3, MidpointRounding.AwayFromZero);
                TL = (double) Math.Round(TLVal,3, MidpointRounding.AwayFromZero);

                IsDirty = false;
            }
        }

        //private void CalculateSeismicValues()
        //{

        //    string SSImageResourceName = "ASCE7_10_SS.png";
        //    string S1ImageResourceName = "ASCE7_10_S1.png";
        //    string TLImageResourceName = "ASCE7_10_TL.png";

        //    Application a = new Application(); // this is required to 'force' full pack://application: package handling initialization
        //    WriteableBitmap SSReferenceImage = ReadImageResource(SSImageResourceName);
        //    WriteableBitmap S1ReferenceImage = ReadImageResource(S1ImageResourceName);
        //    WriteableBitmap TLReferenceImage = ReadImageResource(TLImageResourceName);

        //    double SSVal = 0, S1Val = 0, TLVal = 0;
        //    if (SSReferenceImage != null && S1ReferenceImage != null && TLReferenceImage != null)
        //    {
        //        try
        //        {
        //            MapDataInfo SSInfo = LoadedMapsInfo.Where(p => p.Key == "SS").SingleOrDefault();
        //            MapDataInfo S1Info = LoadedMapsInfo.Where(p => p.Key == "S1").SingleOrDefault();
        //            MapDataInfo TLInfo = LoadedMapsInfo.Where(p => p.Key == "TL").SingleOrDefault();
                    
        //            //Get Color from image
        //            SSVal = MapImageQuery.GetValueFromImage(SSReferenceImage, this.longitude, this.latitude, SSInfo.RangeMinValue, SSInfo.RangeMaxValue,
        //               SSInfo.MinLatitude, SSInfo.MinLongitude, SSInfo.MaxLatitude, SSInfo.MaxLongitude, SSInfo.PixelWidth, SSInfo.PixelHeight);
        //            S1Val = MapImageQuery.GetValueFromImage(S1ReferenceImage, this.longitude, this.latitude, S1Info.RangeMinValue, S1Info.RangeMaxValue,
        //               S1Info.MinLatitude, S1Info.MinLongitude, S1Info.MaxLatitude, S1Info.MaxLongitude, S1Info.PixelWidth, S1Info.PixelHeight);
        //            TLVal = MapImageQuery.GetValueFromImage(TLReferenceImage, this.longitude, this.latitude,TLInfo.RangeMinValue, TLInfo.RangeMaxValue,
        //               TLInfo.MinLatitude, TLInfo.MinLongitude, TLInfo.MaxLatitude, TLInfo.MaxLongitude, TLInfo.PixelWidth, TLInfo.PixelHeight);
        //        }
        //        catch (Exception)
        //        {
        //            //display error message
        //        }

        //        SS = SSVal / 100.0;
        //        S1 = S1Val / 100.0;
        //        TL = TLVal;

        //        IsDirty = false;
        //    }
        //}

        //private WriteableBitmap ReadImageResource(string ResourceName)
        //{
        //    Uri uri = new Uri("pack://application:,,,/Kodestruct.Loads.ASCE.ASCE7_10;component/SeismicLoads/Assets/" + ResourceName);
        //    BitmapImage bitmapImage = new BitmapImage(uri);
        //    WriteableBitmap thisImage = new WriteableBitmap(bitmapImage);
        //    return thisImage;
        //}
    //}
}
