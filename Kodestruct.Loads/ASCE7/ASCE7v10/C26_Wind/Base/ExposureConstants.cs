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
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using Kodestruct.Common.Data;
using Kodestruct.Loads.Properties;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindStructure
    {
        public double GetTerrainExposureConstant(TerrainExposureConstant TerrainExposureConstant, WindExposureCategory WindExposure)
        {
            if (terrainCoefficientsNeedCalculation == true)
	        {
		        CalculateTerrainCoefficients(WindExposure);
                terrainCoefficientsNeedCalculation=false;
	        }
            switch (TerrainExposureConstant)
	        {
		        case TerrainExposureConstant.alpha:          return alpha;
                case TerrainExposureConstant.zg:             return zg;
                case TerrainExposureConstant.alpha_ob:       return alpha_ob;
                case TerrainExposureConstant.b_ob:           return b_ob;
                case TerrainExposureConstant.c:              return c;
                case TerrainExposureConstant.l:              return l;
                case TerrainExposureConstant.epsilon_ob:     return epsilon_overbar;
                case TerrainExposureConstant.zmin:           return zmin;
                default: throw new Exception("Unrecognized terrain _windExposure constant.");
	        }
        }

        bool terrainCoefficientsNeedCalculation = true;

         double alpha,  zg,  alpha_ob,  b_ob,  c,  l,  epsilon_overbar, zmin;

        private void CalculateTerrainCoefficients( WindExposureCategory WindExposure)
        {
            this.WindExposure = WindExposure;
            terrainCoefficientsNeedCalculation = false;

            var Tv11 = new { Exposure = WindExposureCategory.B, alpha = 0.0, zg = 0.0, alpha_ob = 0.0, b_ob = 0.0, c = 0.0, l = 0.0, epsilon_overbar = 0.0, zmin = 0.0 }; // sample
            var TerrainCoefficientList = ListFactory.MakeList(Tv11);

            using (StringReader reader = new StringReader(Resources.ASCE7_10T26_9_1))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 9)
                    {
                        WindExposureCategory _exp = (WindExposureCategory)Enum.Parse(typeof(WindExposureCategory), Vals[0]);
                        double _alpha = double.Parse(Vals[1], CultureInfo.InvariantCulture);
                        double _zg = double.Parse(Vals[2], CultureInfo.InvariantCulture);
                        double _alpha_ob = double.Parse(Vals[3], CultureInfo.InvariantCulture);
                        double _b_ob = double.Parse(Vals[4], CultureInfo.InvariantCulture);
                        double _c = double.Parse(Vals[5], CultureInfo.InvariantCulture);
                        double _l = double.Parse(Vals[6], CultureInfo.InvariantCulture);
                        double _epsilon_ob = double.Parse(Vals[7], CultureInfo.InvariantCulture);
                        double _zmin = double.Parse(Vals[8], CultureInfo.InvariantCulture);

                        TerrainCoefficientList.Add(new { Exposure = _exp, alpha = _alpha, zg = _zg, alpha_ob = _alpha_ob, b_ob = _b_ob, c = _c, l = _l, epsilon_overbar = _epsilon_ob, zmin = _zmin });
                    }
                }

            }

            var ScValues = from sc in TerrainCoefficientList where (sc.Exposure == WindExposure) select sc;
            var ScResult = ScValues.ToList()[0];   

            if (ScResult != null)
            {
                alpha = ScResult.alpha;
                zg = ScResult.zg;
                alpha_ob = ScResult.alpha_ob;
                b_ob = ScResult.b_ob;
                c = ScResult.c;
                l = ScResult.l;
                epsilon_overbar = ScResult.epsilon_overbar;
                zmin = ScResult.zmin;


                
                #region ExposureConst
                ICalcLogEntry ExposureConstEntry = new CalcLogEntry();
                ExposureConstEntry.ValueName = "ExposureConst";
                    ExposureConstEntry.AddDependencyValue("WindExposureCategory", WindExposure.ToString());
                    ExposureConstEntry.AddDependencyValue("alpha"           ,Math.Round(alpha           ,3));
                    ExposureConstEntry.AddDependencyValue("zg"              ,Math.Round(zg              ,3));
                    ExposureConstEntry.AddDependencyValue("alphob"         ,Math.Round(alpha_ob        ,3));
                    ExposureConstEntry.AddDependencyValue("b_ob"            ,Math.Round(b_ob            ,3));
                    ExposureConstEntry.AddDependencyValue("c"               ,Math.Round(c               ,3));
                    ExposureConstEntry.AddDependencyValue("l"               ,Math.Round(l               ,3));
                    ExposureConstEntry.AddDependencyValue("epsilon_overbar" ,Math.Round(epsilon_overbar ,3));
                    ExposureConstEntry.AddDependencyValue("zmin"            ,Math.Round(zmin            ,3));

                ExposureConstEntry.Reference = "";
                ExposureConstEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/TerrainExposureConstants.docx";
                ExposureConstEntry.FormulaID = null; //reference to formula from code
                ExposureConstEntry.VariableValue = WindExposure.ToString();
                #endregion
                this.AddToLog(ExposureConstEntry);
                terrainCoefficientsNeedCalculation = false;
            }
            else
            {
                throw new Exception("_windExposure not found. Failed to retrieve _windExposure coefficients");
            }

        }

    }
}
