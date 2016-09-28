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



namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public abstract partial class FlexuralMemberIBase : FlexuralMember
    {
        public double Gethc(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            //hc - Twice the distance from the center of gravity to the following:
            //the inside face of the compression flange less the fillet or corner
            //radius, for rolled shapes; the nearest line of fasteners at the
            //compression flange or the inside faces of the compression flange
            //when welds are used, for built-up sections, in. (mm) B4.1 (b)

            double hc = 0.0;

            double k = Getk();

            double dsection = GetHeight();
            double yCentroid = Section.Shape.y_Bar;
            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    double tfTop = Get_tfTop();
                    hc = 2.0 * (dsection - yCentroid - tfTop - k);
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    double tfBot = Get_tfBottom();
                    hc = 2.0 * (yCentroid - tfBot - k);
                    break;
                default:
                    throw new Exception("Compression Flange not defined for I-shape and Channel weak axis bending");
            }

            return hc;
        }

        private double Getk()
        {
            return (SectionI.d - SectionI.h_web) / 2;
        }
    }
}
