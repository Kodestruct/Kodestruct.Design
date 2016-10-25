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
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Steel.AISC.AISC360v10.B_General;
using Kodestruct.Common;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Common.Section.SectionTypes;
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{

    public class FlexuralMemberFactory
    {
  

        public ISteelBeamFlexure GetBeam(ISection Shape, ISteelMaterial Material, ICalcLog Log, MomentAxis MomentAxis,
                 FlexuralCompressionFiberPosition compressionFiberPosition, bool IsRolledMember=true)
        {
            ISteelBeamFlexure beam = null;

            if (MomentAxis == Common.Entities.MomentAxis.XAxis)
	        {
                    if (Shape is ISectionI)
                    {
                        ISectionI IShape = Shape as ISectionI;
                        SteelSectionI SectionI = new SteelSectionI(IShape, Material);
                        if (IShape.b_fBot == IShape.b_fTop && IShape.t_fBot == IShape.t_fTop) // doubly symmetric
                        {
                            DoublySymmetricIBeam dsBeam = new DoublySymmetricIBeam(SectionI, Log, compressionFiberPosition, IsRolledMember);
                            beam = dsBeam.GetBeamCase();
                        }
                        else
                        {
                            SinglySymmetricIBeam ssBeam = new SinglySymmetricIBeam(SectionI, IsRolledMember, compressionFiberPosition, Log );
                            beam = ssBeam.GetBeamCase();
                        }
                    }
                    else if (Shape is ISolidShape)
                    {
                        ISolidShape solidShape = Shape as ISolidShape;
                        SteelSolidSection SectionSolid = new SteelSolidSection(solidShape, Material);
                        beam = new BeamSolid(SectionSolid, Log, MomentAxis);
                    }

                    else if (Shape is ISectionChannel)
                    {
                        ISectionChannel ChannelShape = Shape as ISectionChannel;
                        SteelChannelSection ChannelSection = new SteelChannelSection(ChannelShape, Material);
                        beam = new BeamChannel(ChannelSection, IsRolledMember, Log);


                        IShapeCompactness compactness = new ShapeCompactness.ChannelMember(ChannelSection, IsRolledMember, compressionFiberPosition);

                        CompactnessClassFlexure flangeCompactness = compactness.GetFlangeCompactnessFlexure();
                        CompactnessClassFlexure webCompactness = compactness.GetWebCompactnessFlexure();

                        if (flangeCompactness != CompactnessClassFlexure.Compact || webCompactness != CompactnessClassFlexure.Compact)
                        {
                            throw new Exception("Channels with non-compact and slender flanges or webs are not supported. Revise input.");
                        }
                    }


                    else if (Shape is ISectionPipe)
                    {
                        ISectionPipe SectionPipe = Shape as ISectionPipe;
                        SteelPipeSection PipeSection = new SteelPipeSection(SectionPipe, Material);
                        beam = new BeamCircularHss(PipeSection, Log);
                    }

                    else if (Shape is ISectionTube)
                    {
                        ISectionTube TubeShape = Shape as ISectionTube;
                        SteelRhsSection RectHSS_Section = new SteelRhsSection(TubeShape, Material);
                        beam = new BeamRectangularHss(RectHSS_Section,compressionFiberPosition, MomentAxis, Log);
                    }


                    else if (Shape is ISectionBox)
                    {
                        ISectionBox BoxShape = Shape as ISectionBox;
                        SteelBoxSection BoxSection = new SteelBoxSection(BoxShape, Material);
                        beam = new BeamRectangularHss(BoxSection, compressionFiberPosition, MomentAxis, Log);
                    }

                    else if (Shape is ISectionTee)
                    {
                        ISectionTee TeeShape = Shape as ISectionTee;
                        SteelTeeSection TeeSection = new SteelTeeSection(TeeShape, Material);
                        beam = new BeamTee(TeeSection, Log);
                    }
                    else if (Shape is ISectionAngle)
                    {
                        ISectionAngle Angle = Shape as ISectionAngle;
                        SteelAngleSection AngleSection = new SteelAngleSection(Angle,Material);
                        beam = new BeamAngle(AngleSection, Log, Angle.AngleRotation, MomentAxis, Angle.AngleOrientation);
                    }
                    else if (Shape is ISolidShape)
                    {
                        ISolidShape SolidShape = Shape as ISolidShape;
                        SteelSolidSection SolidSection = new SteelSolidSection(SolidShape, Material);
                        beam = new BeamSolid(SolidSection, Log,MomentAxis);
                        
                    }
                    else
                    {
                        throw new Exception("Specified section type is not supported for this node.");
                    }
	        }
            else  // weak axis
	        {
                if (Shape is ISectionI)
                {
                    ISectionI IShape = Shape as ISectionI;
                    SteelSectionI SectionI = new SteelSectionI(IShape, Material);

                    beam = new BeamIWeakAxis(SectionI, IsRolledMember, Log);
                }
                else if (Shape is ISectionChannel)
                {
                    ISectionChannel ChannelShape = Shape as ISectionChannel;
                    SteelChannelSection SectionChannel = new SteelChannelSection(ChannelShape, Material);
                    beam = new BeamChannelWeakAxis(SectionChannel, IsRolledMember, Log);
                }

                else if (Shape is ISectionPipe)
                {
                    ISectionPipe SectionPipe = Shape as ISectionPipe;
                    SteelPipeSection PipeSection = new SteelPipeSection(SectionPipe, Material);
                    beam = new BeamCircularHss(PipeSection, Log);
                }

                else if (Shape is ISectionTube)
                {
                    ISectionTube TubeShape = Shape as ISectionTube;
                    ISectionTube TubeShapeClone = (ISectionTube)TubeShape.GetWeakAxisClone();
                    SteelRhsSection RectHSS_Section = new SteelRhsSection(TubeShapeClone, Material);
                    beam = new BeamRectangularHss(RectHSS_Section, compressionFiberPosition, MomentAxis, Log);
                }
                else if (Shape is ISectionAngle)
                {
                    ISectionAngle SectionAngle = Shape as ISectionAngle;
                    SteelAngleSection AngleSection = new SteelAngleSection(SectionAngle.GetWeakAxisClone() as ISectionAngle, Material);
                    beam = new BeamCircularHss(AngleSection, Log);
                }
                else
                {
                    throw new NotImplementedException();
                }
	        }

            return beam;
        }

        private ISteelBeamFlexure CreateRhsBeam(CompactnessClassFlexure FlangeCompactness, FlexuralCompressionFiberPosition compressionFiberPosition,
            CompactnessClassFlexure WebCompactness,
            ISectionTube RhsSec, ISteelMaterial Material, MomentAxis MomentAxis, ICalcLog Log)
        {
            SteelRhsSection steelSection = new SteelRhsSection(RhsSec, Material);
            ISteelBeamFlexure beam = null;
            beam = new BeamRectangularHss(steelSection,compressionFiberPosition, MomentAxis, Log);
            return beam;
        }

        private ISteelBeamFlexure CreateIBeam(CompactnessClassFlexure FlangeCompactness,
            CompactnessClassFlexure WebCompactness, ISectionI Section, ISteelMaterial Material, 
            ICalcLog Log, bool IsRolled)
        {
            SteelSectionI steelSection = new SteelSectionI(Section, Material);
            ISteelBeamFlexure beam = null;
            if (FlangeCompactness== CompactnessClassFlexure.Compact && WebCompactness == CompactnessClassFlexure.Compact)
            {
                //F2
                beam = new BeamIDoublySymmetricCompact(steelSection, IsRolled, Log);
            }
            else if (WebCompactness == CompactnessClassFlexure.Compact && FlangeCompactness!= CompactnessClassFlexure.Compact)
            {
                //F3
                throw new NotImplementedException();
            }

            return beam;
        }

    }
}
