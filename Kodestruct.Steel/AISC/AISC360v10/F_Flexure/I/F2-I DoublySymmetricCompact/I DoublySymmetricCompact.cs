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
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 

using Kodestruct.Steel.AISC.SteelEntities;
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.Steel.Entities;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamIDoublySymmetricCompact : BeamIDoublySymmetricBase, ISteelBeamFlexure
    {
        public BeamIDoublySymmetricCompact(ISteelSection section, bool IsRolledMember, ICalcLog CalcLog)
            : base(section, IsRolledMember, CalcLog)
        {
            SectionValuesWereCalculated = false;
            //GetSectionValues();
        }



        #region Limit States

        public override SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            if (SectionValuesWereCalculated == false)
            {
                GetSectionValues();
            }

           double M_n= GetMajorNominalPlasticMoment();
           double phiM_n = 0.9 * M_n;
            return new SteelLimitStateValue(phiM_n, true);
        }

        public override SteelLimitStateValue GetFlexuralLateralTorsionalBucklingStrength(double C_b, double L_b, FlexuralCompressionFiberPosition CompressionLocation,
            FlexuralAndTorsionalBracingType BracingType)
        {
            if (SectionValuesWereCalculated == false)
            {
                GetSectionValues();
            }

            SteelLimitStateValue ls;
            if (BracingType == FlexuralAndTorsionalBracingType.FullLateralBracing)
            {
                ls = new SteelLimitStateValue(-1, false);
            }
            else
            {
                double phiM_n = GetFlexuralTorsionalBucklingMomentCapacity(L_b, C_b);
                ls = new SteelLimitStateValue(phiM_n, true);
            }
            return ls;
        }


        public override SteelLimitStateValue GetLimitingLengthForInelasticLTB_Lr(FlexuralCompressionFiberPosition CompressionLocation)
        {
            if (SectionValuesWereCalculated == false)
            {
                GetSectionValues();
            }
            //double rts = Getrts(Iy, Cw, Sx);
            double Lr = GetLr(rts, E, F_y, S_x, J, c, ho);  // (F2-6)
            SteelLimitStateValue ls = new SteelLimitStateValue();
            ls.IsApplicable = true;
            ls.Value = Lr;

            return ls;
        }

        public override SteelLimitStateValue GetLimitingLengthForFullYielding_Lp(FlexuralCompressionFiberPosition CompressionLocation)
        {
            if (SectionValuesWereCalculated == false)
            {
                GetSectionValues();
            }
            double Lp = GetLp(r_y, E, F_y); //(F2-5)
            SteelLimitStateValue ls = new SteelLimitStateValue();
            ls.IsApplicable = true;
            ls.Value = Lp;


            return ls;

        }


        #endregion


        bool SectionValuesWereCalculated;


        internal void GetSectionValues()
        {
            //if (SectionValuesWereCalculated == false)
            //{
                _E = Section.Material.ModulusOfElasticity;
                _F_y = Section.Material.YieldStress;
                _I_y = Section.Shape.I_y;
                _S_xBot = Section.Shape.S_xBot;
                _S_xTop = Section.Shape.S_xTop;
                _S_x = Math.Min(_S_xBot, _S_xTop);
                _Z_x = Section.Shape.Z_x;
                _r_y = Section.Shape.r_y;
                _C_w = Section.Shape.C_w;
                _J = Section.Shape.J;
                _c = Get_c();
                _ho = Get_ho();
                _rts = this.GetEffectiveRadiusOfGyration();
                SectionValuesWereCalculated = true;
            //}
        }

        private double GetEffectiveRadiusOfGyration()
        {
            double r_ts = Math.Sqrt(((Math.Sqrt(_I_y * _C_w)) / (_S_x))); //(F2-7)
            return r_ts;
        }

         double _E  ;    
         double _F_y;    
         double _I_y ;   
         double _S_xBot; 
         double _S_xTop; 
         double _S_x  ;  
         double _Z_x  ;  
         double _r_y  ;  
         double _C_w ;   
         double _J ;     
         double _c;      
         double _ho;     
         double _rts;    

        double E      {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _E  ;     }}
        double F_y    {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _F_y;     }}
        double I_y    {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _I_y ;    }}
        double S_xBot {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _S_xBot;  }}
        double S_xTop {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _S_xTop;  }}
        double S_x    {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _S_x  ;   }}
        double Z_x    {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _Z_x  ;   }}
        double r_y    {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _r_y  ;   }}
        double C_w    {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _C_w ;    }}
        double J      {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _J ;      }}
        double c      {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _c;       }}
        double ho     {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _ho;      }}
        double rts   {get { if(SectionValuesWereCalculated==false) {GetSectionValues();} return  _rts;    }}



    }
}
