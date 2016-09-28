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
using System.Linq;
using System.Text; 
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.Exceptions;



namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamChannel : BeamIDoublySymmetricCompact
    {
        public BeamChannel(ISteelSection section, bool IsRolledMember, ICalcLog CalcLog)
            : base( section, IsRolledMember, CalcLog)
        {
            SectionChannel = null;
            ISectionChannel s = Section.Shape as ISectionChannel;
            this.isRolledMember = IsRolledMember;

            if (s == null)
            {
                throw new SectionWrongTypeException(typeof(ISectionChannel));
            }
            else
            {
                SectionChannel = s;
            }
        }


        ISectionChannel SectionChannel;


        private bool isRolledMember;

        public override bool IsRolledMember
        {
            get { return isRolledMember; }
            set { isRolledMember = value; }
        }


        protected override double GetHeight()
        {
            return SectionChannel.d;
        }

        protected override double GetBfTop()
        {
            return SectionChannel.b_f;
        }
        protected override double Get_tfTop()
        {
            return SectionChannel.t_f;
        }

        protected override double GetBfBottom()
        {
            return SectionChannel.b_f;
        }
        protected override double Get_tfBottom()
        {
            return SectionChannel.t_f;
        }

        //protected override double GetkBottom()
        //{
        //    return SectionChannel.k;
        //}
        //protected override double GetkTop()
        //{
        //    return SectionChannel.k;
        //}

        protected override double Gettw()
        {
            return SectionChannel.t_w;
        }


        public override double Get_ho()
        
        {
            return SectionChannel.h_o;
        }



       public override double Get_c()
       {
           
           double Iy = SectionChannel.I_y;
           double ho = SectionChannel.h_o;
           double Cw = SectionChannel.C_w;
           double c=ho/2.0*Math.Sqrt(Iy/Cw);


           return c;
       }


    }
}
