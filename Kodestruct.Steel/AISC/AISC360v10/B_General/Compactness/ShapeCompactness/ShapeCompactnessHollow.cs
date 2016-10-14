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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.Interfaces;



namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{

    public partial class ShapeCompactness
    {

        public class HollowMember : ShapeCompactnessBase
       {
           //ICompactnessElement FlangeCompactness;
           //ICompactnessElement WebCompactness;

           public HollowMember(ISteelSection section,
                FlexuralCompressionFiberPosition compressionFiberPosition, MomentAxis MomentAxis)
            {
                ISection Section = section.Shape;
                if (Section is ISectionTube|| Section is ISectionPipe || Section is ISectionBox)
                {
                    if (Section is ISectionTube)
                    {
                        ISectionTube tube = Section as ISectionTube;
                        if (MomentAxis == MomentAxis.XAxis)
                        {
                            FlangeCompactness = new FlangeOfRhs(section.Material, tube, MomentAxis);
                            WebCompactness = new WebOfRhs(section.Material, tube, MomentAxis);
                        }
                        else
                        {
                            WebCompactness = new FlangeOfRhs(section.Material, tube, MomentAxis);
                            FlangeCompactness = new WebOfRhs(section.Material, tube, MomentAxis);
                        }
                        

                    }

                    if (Section is ISectionPipe)
                    {
                        ISectionPipe pipe = Section as ISectionPipe;
                        FlangeCompactness = new WallOfChs(section.Material, pipe);
                        WebCompactness = new WallOfChs(section.Material, pipe);
                    }

                    if (Section is ISectionBox)
                    {
                        ISectionBox box = Section as ISectionBox;
                        if (MomentAxis == MomentAxis.XAxis)
                        {
                            FlangeCompactness = new FlangeOfBox(section.Material, box);
                            WebCompactness = new WebOfBox(section.Material, box);
                        }
                        else
                        {
                            WebCompactness = new FlangeOfBox(section.Material, box);
                            FlangeCompactness = new WebOfBox(section.Material, box);
                        }
                    } 
                }
                else
                {
                    throw new SectionWrongTypeException("ISectionTube, ISectionPipe or ISectionBox");
                }

            }
       }


    }
}
