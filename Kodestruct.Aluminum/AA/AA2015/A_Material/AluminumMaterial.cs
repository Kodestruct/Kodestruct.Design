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
using Kodestruct.Aluminum.AA.Entities.Material;
using Kodestruct.Aluminum.Properties;
using Kodestruct.Common.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.AA2015
{
    public class AluminumMaterial : AluminumMaterialBase
    {
        public AluminumMaterial(string Alloy, string Temper, string ThicknessRange, string ProductType, bool IsWelded =false,
            bool MeetsAdditionalWeldingCriteria=true)
        {
           this.Alloy=Alloy;
           this.Temper=Temper;
           this.ThicknessRange=ThicknessRange;
           this.ProductType=ProductType;
           this.MeetsAdditionalWeldingCriteria = MeetsAdditionalWeldingCriteria;
           IsCustomMaterial = false;
           this.IsWelded = IsWelded;

        }

            string Alloy;
            string ThicknessRange;
            string ProductType;
            bool MeetsAdditionalWeldingCriteria;
            bool IsCustomMaterial;


            public AluminumMaterial(double F_tu,double F_ty,double F_tuw,double F_tyw,double k_t, string Temper, bool IsWelded =false)
            {
                    this.F_tu   =  F_tu   ;
                    this.F_ty   =  F_ty   ;
                    this.F_tuw  =  F_tuw  ;
                    this.F_tyw  =  F_tyw  ;
                    this.k_t = k_t;
                    this.Temper = Temper;
                    IsCustomMaterial = true;
                    this.IsWelded = IsWelded;
            }
        protected override void ReadMaterialProperties()
        {


            if (IsCustomMaterial == false)
            {
                #region Read Material Data

                var SampleValue = new { Alloy = "", Temper = "", ThicknessRange = "", Product = "", F_tu = 0.0, F_ty = 0.0, F_tuw = 0.0, F_tyw = 0.0, k_t = 0.0, F_tyw_Alt = 0.0 }; // sample
                var MaterialTableVals = ListFactory.MakeList(SampleValue);

                using (StringReader reader = new StringReader(Resources.AA2015_TableA3_3WroughtAluminumProducts))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] Vals = line.Split(',');
                        if (Vals.Length == 10)
                        {

                            string _Alloy = (string)Vals[0];
                            string _Temper = (string)Vals[1];
                            string _ThicknessRange = (string)Vals[2];
                            string _Product = (string)Vals[3];
                            double _F_tu = Double.Parse(Vals[4]);
                            double _F_ty = Double.Parse(Vals[5]);
                            double _F_tuw = Double.Parse(Vals[6]);
                            double _F_tyw = Double.Parse(Vals[7]);
                            double _k_t = Double.Parse(Vals[8]);
                            double _F_tyw_Alt = Double.Parse(Vals[9]);


                            MaterialTableVals.Add
                            (new
                            {
                                Alloy = _Alloy,
                                Temper = _Temper,
                                ThicknessRange = _ThicknessRange,
                                Product = _Product,
                                F_tu = _F_tu,
                                F_ty = _F_ty,
                                F_tuw = _F_tuw,
                                F_tyw = _F_tyw,
                                k_t = _k_t,
                                F_tyw_Alt = _F_tyw_Alt
                            }

                            );
                        }
                    }

                }

                #endregion

                var MaterialEntryData = MaterialTableVals.First(m =>
                    m.Alloy == this.Alloy &&
                    m.Temper == this.Temper &&
                    m.ThicknessRange == this.ThicknessRange &&
                    m.Product == this.ProductType);

                if (MaterialEntryData != null)
                {
                    this.F_tu = MaterialEntryData.F_tu;
                    this.F_ty = MaterialEntryData.F_ty;
                    this.F_tuw = MaterialEntryData.F_tuw;
                    this.F_tyw = MeetsAdditionalWeldingCriteria == false ? MaterialEntryData.F_tyw : MaterialEntryData.F_tyw_Alt;
                    this.k_t = MaterialEntryData.k_t;
                } 
            }
        }
        
    }
}
