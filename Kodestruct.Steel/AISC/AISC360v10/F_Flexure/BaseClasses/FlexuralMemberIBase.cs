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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;



namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public abstract partial class FlexuralMemberIBase : FlexuralMember
    {
        public FlexuralMemberIBase(ISteelSection section, bool IsRolledMember,  ICalcLog CalcLog)
            : base(section, CalcLog)
        {
            
            this.isRolledMember = IsRolledMember;


            if (!(Section.Shape is IDoubleFlangeMember))
            {
                throw new SectionWrongTypeException("I-section or Channel");

            }
            else
            {
                if (Section.Shape is ISectionI)
                {
                    ISectionI s = Section.Shape as ISectionI;
                }
            }
        }

          ISectionI SectionI;
         

        private bool isRolledMember;

        public virtual bool IsRolledMember
        {
            get { return isRolledMember; }
            set { isRolledMember = value; }
        }
        

        protected virtual double GetHeight()
        {
            return SectionI.d;
        }

        protected virtual double GetBfTop()
        {
            return SectionI.b_fTop;
        }
        protected virtual double Get_tfTop()
        {
            return SectionI.t_fTop;
        }

        protected virtual double GetBfBottom()
        {
            return SectionI.b_fBot;
        }
        protected virtual double Get_tfBottom()
        {
            return SectionI.t_fBot;
        }

        //protected virtual double GetkBottom()
        //{
        //    return SectionI.k;
        //}
        //protected virtual double GetkTop()
        //{
        //    return SectionI.k;
        //}

        protected virtual double Gettw()
        {
            return SectionI.t_w;
        }


        protected virtual double GetFlangeCentroidDistanceho()
        {
            return SectionI.h_o;
        }




    }
}
