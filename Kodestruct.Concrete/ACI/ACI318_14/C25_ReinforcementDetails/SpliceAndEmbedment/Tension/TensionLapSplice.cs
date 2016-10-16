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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class TensionLapSplice : Splice, ISplice

    {

        //Basic calculation
        public TensionLapSplice
            (
            IConcreteMaterial Concrete,
            Rebar Bar1,
            Rebar Bar2,
            bool MeetsRebarSpacingAndEdgeDistance,
            bool HasMinimumTransverseReinforcement,
            bool IsTopRebar,
            TensionLapSpliceClass SpliceClass,
            ICalcLog log
            )
            : base(log)
        {
            this.Concrete = Concrete;
            this.Bar1 = Bar1;
            this.Bar2 = Bar2;
            this.MeetsRebarSpacingAndEdgeDistance=MeetsRebarSpacingAndEdgeDistance;
            this.HasMinimumTransverseReinforcement=HasMinimumTransverseReinforcement;

            this.SpliceClass = SpliceClass;
            this.IsTopRebar = IsTopRebar;
            this.A_tr = A_tr;
            this.s_tr = s_tr;
            this.n = n;


            CalculateValuesBasic();
        }

        private void CalculateValuesBasic()
        {
            this.Rebar1Diameter = Bar1.Diameter;
            this.Rebar2Diameter = Bar2.Diameter;
            DevelopmentTension td1 = new DevelopmentTension(Concrete, Bar1,MeetsRebarSpacingAndEdgeDistance,IsTopRebar,1.0,true, Log);
            DevelopmentTension td2 = new DevelopmentTension(Concrete, Bar2, MeetsRebarSpacingAndEdgeDistance, IsTopRebar, 1.0, true, Log);

            this.Rebar1DevelopmentLength = td1.GetTensionDevelopmentLength(HasMinimumTransverseReinforcement);
            this.Rebar2DevelopmentLength = td2.GetTensionDevelopmentLength(HasMinimumTransverseReinforcement);
        }

        //Detailed calculation
        public TensionLapSplice
            (
            IConcreteMaterial Concrete,
            Rebar Bar1,
            Rebar Bar2,
            double ClearSpacing,
            double ClearCover,
            bool IsTopRebar,
            double A_tr, 
            double s_tr, 
            double n,
            TensionLapSpliceClass SpliceClass,
            ICalcLog log
            )
            : base(log)
        {
            this.Concrete    =Concrete    ;
            this.Bar1        =Bar1        ;      
            this.Bar2        =Bar2        ;      
            this.ClearSpacing=ClearSpacing;     
            this.ClearCover  =ClearCover  ;
            this.IsTopRebar = IsTopRebar;
            this.A_tr =A_tr  ;
            this.s_tr =s_tr  ;
            this.n = n;
            this.SpliceClass = SpliceClass;

            CalculateValuesDetailed();
        }

        private void CalculateValuesDetailed()
        {
            this.Rebar1Diameter = Bar1.Diameter;
            this.Rebar2Diameter = Bar2.Diameter;
            DevelopmentTension td1 = new DevelopmentTension(Concrete, Bar1, ClearSpacing, ClearCover, IsTopRebar, 1.0, true, Log);
            DevelopmentTension td2 = new DevelopmentTension(Concrete, Bar2, ClearSpacing, ClearCover, IsTopRebar, 1.0, true, Log);

            this.Rebar1DevelopmentLength = td1.GetTensionDevelopmentLength(A_tr,s_tr,n);
            this.Rebar2DevelopmentLength = td2.GetTensionDevelopmentLength(A_tr, s_tr, n);
        }

            double A_tr  {get; set;}
            double s_tr  {get; set;}
            double n    { get; set; }

            bool MeetsRebarSpacingAndEdgeDistance  {get; set;}
            bool HasMinimumTransverseReinforcement {get; set;}
            IConcreteMaterial Concrete  {get; set;}
            Rebar Bar1                  {get; set;}
            Rebar Bar2                  {get; set;}
            double ClearSpacing         {get; set;}
            double ClearCover           {get; set;}
            bool IsTopRebar             { get; set; }

        public TensionLapSplice(
            double rebar1Diameter, double rebar1DevelopmentLength,
            double rebar2Diameter, double rebar2DevelopmentLength,
            TensionLapSpliceClass SpliceClass,
            ICalcLog log):base(log)
        {
            this.spliceClass = SpliceClass;
            this.Rebar1Diameter = rebar1Diameter;
            this.Rebar2Diameter = rebar2Diameter;
            this.Rebar1DevelopmentLength = Rebar1DevelopmentLength;
            this.Rebar2DevelopmentLength = Rebar2DevelopmentLength;
        }

        private TensionLapSpliceClass spliceClass;

        public TensionLapSpliceClass SpliceClass
        {
            get { return spliceClass; }
            set { spliceClass = value; }
        }

        double length;
        public override double Length
        {
            get {
                length = GetLs();
                return length; }
        }

        public double Rebar1Diameter { get; set; }
        public double Rebar2Diameter { get; set; }

        public double Rebar1DevelopmentLength { get; set; }
        public double Rebar2DevelopmentLength { get; set; }
    }
}
