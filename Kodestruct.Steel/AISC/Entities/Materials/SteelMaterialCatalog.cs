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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.Properties;


namespace Kodestruct.Steel.AISC.SteelEntities.Materials
{
    public class SteelMaterialCatalog: AnalyticalElement, ISteelMaterial
    {
        
        public SteelMaterialCatalog(string SteelMaterialId, ICalcLog Log): base(Log)
        {
            this.SteelMaterialId = SteelMaterialId;
            PropertiesWereCalculated = false;

        }
        public SteelMaterialCatalog(string SteelMaterialId, double FastenerDiameter, ICalcLog Log) :
            this(SteelMaterialId, Log)
        {
            this.FastenerDiameter = FastenerDiameter;
        }


        private string steelMaterialId;

        public string SteelMaterialId
        {
            get { return steelMaterialId; }
            set 
            {
                steelMaterialId = value;
                PropertiesWereCalculated = false;
            }
        }

        bool PropertiesWereCalculated;
        private void CalculateProperties()
        {
            if (PropertiesWereCalculated == false)
            {


                #region Read Table Data

                var Tv11 = new { Id = "", Fy = 0.0, Fu = 0.0, MinDiam = 0.0, MaxDiam = 0.0 }; // sample
                var AllMaterialsList = ListFactory.MakeList(Tv11);

                using (StringReader reader = new StringReader(Resources.AISC360_10_MaterialProperties))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] Vals = line.Split(',');
                        if (Vals.Count() == 3)
                        {
                            string MatKey = Vals[0];
                            double V1 = double.Parse(Vals[1], CultureInfo.InvariantCulture);
                            double V2 = double.Parse(Vals[2], CultureInfo.InvariantCulture);

                            AllMaterialsList.Add(new { Id = MatKey, Fy = V1, Fu = V2, MinDiam = 0.0, MaxDiam = 0.0 });
                        }
                        if (Vals.Count() == 5)
                        {
                            string MatKey = Vals[0];
                            double V1 = double.Parse(Vals[1], CultureInfo.InvariantCulture);
                            double V2 = double.Parse(Vals[2], CultureInfo.InvariantCulture);
                            double V3 = double.Parse(Vals[3], CultureInfo.InvariantCulture);
                            double V4 = double.Parse(Vals[4], CultureInfo.InvariantCulture);

                            AllMaterialsList.Add(new { Id = MatKey, Fy = V1, Fu = V2, MinDiam = V3, MaxDiam = V4 });
                        }
                    }

                }

                #endregion

                var Materials = AllMaterialsList.Where(v => v.Id == SteelMaterialId).ToList();
                if (Materials.Count > 1 && FastenerDiameter != 0)
                {
                    var MatPropList = Materials.Where(m =>
                    {
                        if (m.MaxDiam!=0)
                        {
                            if (FastenerDiameter > m.MinDiam && FastenerDiameter <= m.MaxDiam)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            } 
                        }
                        else
                        {
                            return true;
                        }
                    }).ToList();

                    if (MatPropList.Any())
                    {
                        var MatProp = MatPropList.FirstOrDefault();
                        this.YieldStress = MatProp.Fy;
                        this.UltimateStress = MatProp.Fu;
                    }
                }
                else
                {
                    var MatProp = Materials.FirstOrDefault();
                    this.YieldStress = MatProp.Fy;
                    this.UltimateStress = MatProp.Fu;
                }

                PropertiesWereCalculated = true;

                double Fy = YieldStress;
                double Fu = UltimateStress;
                double E = ModulusOfElasticity;
                double G = ShearModulus;
                
                #region Fy
                ICalcLogEntry FyEntry = new CalcLogEntry();
                FyEntry.ValueName = "Fy";
                FyEntry.AddDependencyValue("Fu", Math.Round(Fu, 3));
                FyEntry.AddDependencyValue("SteelMaterialId", SteelMaterialId);
                FyEntry.AddDependencyValue("E", Math.Round(E, 0));
                FyEntry.AddDependencyValue("G", Math.Round(G, 0));
                FyEntry.Reference = "";
                FyEntry.DescriptionReference = "/Templates/Steel/General/MaterialProperties.docx";
                FyEntry.FormulaID = null; //reference to formula from code
                FyEntry.VariableValue = Math.Round(Fy, 3).ToString();
                #endregion
                this.AddToLog(FyEntry);

               
            }

        }

        private double fastenerDiameter;

        public double FastenerDiameter
        {
            get { return fastenerDiameter; }
            set { 
                fastenerDiameter = value;
                PropertiesWereCalculated = false;
            }
        }

        private double yieldStress;

        public double YieldStress
        {
            get {
                CalculateProperties();
                return yieldStress; }
            set { yieldStress = value; }
        }

        private double ultimateStress;

        public double  UltimateStress
        {
            get {
                CalculateProperties();
                return ultimateStress; }
            set { ultimateStress = value; }
        }
        

        public double ModulusOfElasticity
        {
            get { return SteelConstants.ModulusOfElasticity; }
        }
        
        public double ShearModulus
        {
            get { return SteelConstants.ShearModulus; }
        }
        
    }
}
