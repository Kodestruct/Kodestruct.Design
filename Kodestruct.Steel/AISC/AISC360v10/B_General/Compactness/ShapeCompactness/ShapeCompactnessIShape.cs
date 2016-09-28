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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
 
 


namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{

    public partial class ShapeCompactness
    {
        public class IShapeMember: ShapeCompactnessBase
        {

            public IShapeMember(ISteelSection Section, bool IsRolledShape, 
                FlexuralCompressionFiberPosition compressionFiberPosition)
            {

                double b;
                double tf;


                if (Section.Shape is ISectionI)
	            {
                ISectionI sectI = Section.Shape as ISectionI;

                switch (compressionFiberPosition)
                {
                    case FlexuralCompressionFiberPosition.Top:
                        b = sectI.b_fTop;
                        tf = sectI.t_fTop;
                        break;
                    case FlexuralCompressionFiberPosition.Bottom:
                        b = sectI.b_fBot;
                        tf = sectI.t_fBot;
                        break;
                    default:
                        throw new CompressionFiberPositionException();
                }


                //flange compactness
                if (IsRolledShape == true)
                {
                    this.FlangeCompactness = new FlangeOfRolledIShape(Section.Material, b / 2.0, tf);
                }
                else
                {
                    this.FlangeCompactness = new FlangeOfBuiltUpI(Section.Material, sectI, compressionFiberPosition);
                }

                //web compactness

                bool isDoublySymmetric = ShapeISymmetry.IsDoublySymmetric(Section.Shape);


                if (isDoublySymmetric == true)
                {
                    WebCompactness = new WebOfDoublySymI(Section.Material, sectI);
                }
                else
                {
                    WebCompactness = new WebOfSinglySymI(Section.Material, sectI, compressionFiberPosition);
                } 
	            }
            }


        }


    }
}
