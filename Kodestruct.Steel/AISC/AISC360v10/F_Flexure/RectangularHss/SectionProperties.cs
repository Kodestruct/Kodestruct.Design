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
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.Exceptions;



namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamRectangularHss : FlexuralMemberRhsBase, ISteelBeamFlexure
    {
        private double GetEffectiveSectionModulusX(MomentAxis MomentAxis)
        {
            double Se=0.0;
            double be = GetEffectiveFlangeWidth_beff(MomentAxis);
            double b = GetCompressionFlangeWidth_b(MomentAxis);
            double AOriginal = Section.Shape.A;


            double t_f = GetFlangeThickness(MomentAxis);
            double bRemoved = (b - be);
            double ADeducted = bRemoved * t_f;

            double h = GetSectionHeight(MomentAxis);

            //Find I reduced
            double I_Reduced = GetReducedMomentOfInertiaX(MomentAxis, ADeducted, bRemoved, t_f);


		     double yCentroidModifiedFromBottom = h / 2.0 - ADeducted / AOriginal;
             double yCentroidModifiedFromTop = h / 2.0 + ADeducted / AOriginal;

             Se = I_Reduced / yCentroidModifiedFromTop;
            

            return Se;
        }

        private double GetSectionHeight(MomentAxis MomentAxis)
        {
            double h = 0.0;
            if (ShapeTube != null)
            {

                if (MomentAxis == Common.Entities.MomentAxis.XAxis)
                {
                    h = ShapeTube.H;
                }
                else if (MomentAxis == Common.Entities.MomentAxis.YAxis)
                {
                    h = ShapeTube.B;
                }
                else
                {
                    throw new Exception("Principal flexure calculation not supported. Select X-axis and Y-axis");
                }
            }
            else if (ShapeBox != null)
            {

                if (MomentAxis == Common.Entities.MomentAxis.XAxis)
                {
                    h = ShapeBox.H;
                }
                else if (MomentAxis == Common.Entities.MomentAxis.YAxis)
                {
                    h = ShapeBox.B;
                }
                else
                {
                    throw new Exception("Principal flexure calculation not supported. Select X-axis and Y-axis");
                }
            }
            else
            {
                throw new ShapeTypeNotSupportedException(" effective moment of interia calculation for hollow section");
            }
            return h;
        }

        private double GetSectionWidth(MomentAxis MomentAxis)
        {
            double b = 0.0;
            if (ShapeTube != null)
            {

                if (MomentAxis == Common.Entities.MomentAxis.XAxis)
                {
                    b = ShapeTube.B;
                }
                else if (MomentAxis == Common.Entities.MomentAxis.YAxis)
                {
                    b = ShapeTube.H;
                }
                else
                {
                    throw new Exception("Principal flexure calculation not supported. Select X-axis and Y-axis");
                }
            }
            else if (ShapeBox != null)
            {

                if (MomentAxis == Common.Entities.MomentAxis.XAxis)
                {
                    b = ShapeBox.B;
                }
                else if (MomentAxis == Common.Entities.MomentAxis.YAxis)
                {
                    b = ShapeBox.H;
                }
                else
                {
                    throw new Exception("Principal flexure calculation not supported. Select X-axis and Y-axis");
                }
            }
            else
            {
                throw new ShapeTypeNotSupportedException(" effective moment of interia calculation for hollow section");
            }
            return b;
        }

        private double GetMomentOfInertia(MomentAxis MomentAxis)
        {
            double I;

            if (MomentAxis == Common.Entities.MomentAxis.XAxis)
            {
                I = Section.Shape.I_x;
            }
            else if (MomentAxis == Common.Entities.MomentAxis.YAxis)
            {
                I = Section.Shape.I_y;
            }
            else
            {
                throw new Exception("Principal flexure calculation not supported. Select X-axis and Y-axis");
            }
            return I;
        }

        private double GetReducedMomentOfInertiaX(MomentAxis MomentAxis, double ADeducted, double bRemoved, double tdes)
        {

            double IOriginal = GetMomentOfInertia(MomentAxis); //Section.Shape.I_x;
            double h = GetSectionHeight(MomentAxis);
            double yDeducted = (h - tdes) / 2.0;
            double Ideducted = bRemoved * Math.Pow(tdes, 3) / 12.0;
            //Use parallel axis theorem:
            double IFinal = IOriginal - (ADeducted * Math.Pow(yDeducted, 2) + Ideducted);
            return IFinal;
        }

        private double GetFlangeThickness(MomentAxis MomentAxis )
        {
            double t_flange = 0.0;
            if (ShapeTube!=null)
            {
                t_flange = ShapeTube.t_des;
            }
            else if (ShapeBox!=null)
            {
                
                if (MomentAxis == Common.Entities.MomentAxis.XAxis)
                {
                    t_flange = ShapeBox.t_f;
                }
                else if (MomentAxis == Common.Entities.MomentAxis.YAxis)
                {
                    t_flange = ShapeBox.t_w;
                }
                else
                {
                    throw new Exception("Principal flexure calculation not supported. Select X-axis and Y-axis");
                }
            }
            else
            {
                throw new ShapeTypeNotSupportedException(" effective moment of interia calculation for hollow section"); 
            }
            return t_flange;
        }

        protected virtual double GetCompressionFlangeWidth_b(MomentAxis MomentAxis)
        {
            // since section is symmetrical the location of compression fiber
            // does not matter

            double b_c = 0.0;
            double B = 0.0;
            B = GetSectionWidth(MomentAxis);

            if (ShapeTube != null)
            {



                            if (ShapeTube.CornerRadiusOutside == -1.0)
                            {
                                b_c = ShapeTube.B - 3.0 * ShapeTube.t_des;
                            }
                            else
                            {
                                b_c = ShapeTube.B - 2.0 * ShapeTube.CornerRadiusOutside;
                            }
            }
            else if (ShapeBox != null)
            {
                b_c = ShapeBox.B;
            }
            else
            {
                throw new ShapeTypeNotSupportedException(" effective moment of interia calculation for hollow section");
            }
            return b_c;


            
            return b_c;

        }

        protected double GetEffectiveFlangeWidth_beff(MomentAxis MomentAxis)
        {
            double b = GetCompressionFlangeWidth_b(MomentAxis);
            double tf = GetFlangeThickness(MomentAxis);
            double be = 1.92*tf*SqrtE_Fy()*(1.0-0.38/(b/tf)*SqrtE_Fy()-0.738); //(F7-4)
            be = be > b? b :be;
            be= be<0? 0 : be;

            return be;
        }

        
    }
}
