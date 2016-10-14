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
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE7.Entities;


namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindStructure: AnalyticalElement
    {

        public WindStructure(ICalcLog CalcLog): base(CalcLog)
        {
            terrainCoefficientsNeedCalculation = true;
        }

        public WindStructure(double Width, double Length, double Height, double WindSpeed, 
        WindStructureDynamicResponseType StructureDynamicClass,
            double Damping, ICalcLog CalcLog)
            : this(Width, Length, Height, WindSpeed, WindExposureCategory.C, WindEnclosureType.Enclosed, StructureDynamicClass, Damping, CalcLog)
        {
            terrainCoefficientsNeedCalculation = true;
        }

        public WindStructure(double Width, double Length, double Height, double WindSpeed,
        double Damping, ICalcLog CalcLog)
            : this(Width, Length, Height, WindSpeed, WindExposureCategory.C, WindEnclosureType.Enclosed, WindStructureDynamicResponseType.Flexible, Damping, CalcLog)
        {
            terrainCoefficientsNeedCalculation = true;
        }

        public WindStructure(double Width, double Length, double Height, double WindSpeed, WindExposureCategory windExposureType,
            WindEnclosureType EnclosureClass, WindStructureDynamicResponseType StructureDynamicClass,
            double Damping, ICalcLog CalcLog): base(CalcLog)
        {
            this.width = Width;
            this.length = Length;
            this.height = Height;
            this.windSpeed = WindSpeed;
            this._windExposure = windExposureType;
            this.enclosureClassification = EnclosureClass;
            this.dynamicClassification = StructureDynamicClass;
            this.damping = Damping;
            
            
            terrainCoefficientsNeedCalculation = true;
        }

        private double width;

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        private double length;

        public double Length
        {
            get { return length; }
            set { length = value; }
        }
        

        private double height;

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        private double period;

        public double Period
        {
            get { return period; }
            set { period = value; }
        }


        private WindExposureCategory _windExposure;

        public WindExposureCategory WindExposure
        {
            get { return _windExposure; }
            set { _windExposure = value; }
        }

        private double windSpeed;

        public double WindSpeed
        {
            get { return windSpeed; }
            set { windSpeed = value; }
        }


        private WindEnclosureType enclosureClassification;

        public WindEnclosureType EnclosureClassification
        {
            get { return enclosureClassification; }
            set { enclosureClassification = value; }
        }

        private WindStructureDynamicResponseType dynamicClassification;

        public WindStructureDynamicResponseType DynamicClassification
        {
            get { return dynamicClassification; }
            set { dynamicClassification = value; }
        }

        private double damping;

        public double Damping
        {
            get { return damping; }
            set { damping = value; }
        }
        
    }
}
