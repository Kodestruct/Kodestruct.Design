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
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindLocation : AnalyticalElement
    {

        public double GetKzt(
            double x, 
            double z,
            double H, 
            double Lh, 
            WindExposureCategory windExposureCategory,
            TopographicLocation TopographicLocation,
            TopographyType TopographyType
            )
        {
            double HToLh = H/Lh;
            double Mu = this.GetMu(TopographyType,TopographicLocation);
            double gamma = Getgamma(TopographyType);
            double K1OverHToLh = GetK1OverHToLh(windExposureCategory, TopographyType, HToLh);
            double K1 = GetK1(K1OverHToLh, HToLh);
            double K2 = GetK2(x,Mu,Lh);
            double K3 = GetK3(gamma, z, Lh);

            double Kzt = Math.Pow(1.0 + K1 * K2 * K3, 2.0);

            
            #region Kzt
            ICalcLogEntry KztEntry = new CalcLogEntry();
            KztEntry.ValueName = "Kzt";
            KztEntry.AddDependencyValue("WindExposureCategory", windExposureCategory.ToString() );
            KztEntry.AddDependencyValue("HillShape", GetHillShapeString(TopographyType));
            KztEntry.AddDependencyValue("LocationRelativeToCrest",GetLocationString(TopographicLocation) );
            KztEntry.AddDependencyValue("HToLh", Math.Round(HToLh, 3));
            KztEntry.AddDependencyValue("z", Math.Round(z, 3));
            KztEntry.AddDependencyValue("x", Math.Round(x, 3));
            KztEntry.AddDependencyValue("Lh", Math.Round(Lh, 3));
            KztEntry.AddDependencyValue("mu", Math.Round(Mu, 3));
            KztEntry.AddDependencyValue("gamma", Math.Round(gamma, 3));
            KztEntry.AddDependencyValue("K1OverHToLh", Math.Round(K1OverHToLh, 3));
            KztEntry.AddDependencyValue("K1", Math.Round(K1, 3));
            KztEntry.AddDependencyValue("K2", Math.Round(K2, 3));
            KztEntry.AddDependencyValue("K3", Math.Round(K3, 3));
            KztEntry.Reference = "";
            KztEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindTopographicFactor.docx";
            KztEntry.FormulaID = null; //reference to formula from code
            KztEntry.VariableValue = Math.Round(Kzt, 3).ToString();
            #endregion
            this.AddToLog(KztEntry);
            return Kzt;

        }

        private string GetLocationString(TopographicLocation loc)
        {
            string ShapeStr = "";
            switch (loc)
            {
                case TopographicLocation.UpwindOfCrest:
                    ShapeStr = "upwind of crest";
                    break;
                case TopographicLocation.DownwindOfCrest:
                    ShapeStr = "downwind of crest";
                    break;
            }
            return ShapeStr;
        }
        private string GetHillShapeString(TopographyType topoType)
        {
            string ShapeStr = "";
            switch (topoType)
            {
                case TopographyType.TwoDimensionalRidge:
                    ShapeStr ="2D Ridge";
                    break;
                case TopographyType.TwoDimensionalEscarpment:
                    ShapeStr = "2D Escarpment";
                    break;
                case TopographyType.ThreeDimensionalAxisymHill:
                    ShapeStr = "3D Axisymmetric hill";
                    break;
            }
            return ShapeStr;
        }
    }
}
