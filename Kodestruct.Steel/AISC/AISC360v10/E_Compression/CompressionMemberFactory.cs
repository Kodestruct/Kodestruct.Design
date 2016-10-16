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
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Compression;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public class CompressionMemberFactory
    {
        //SteelMaterial mat 

        public ISteelCompressionMember GetCompressionMember(ISection Shape, ISteelMaterial mat,  double L_ex, double L_ey, double L_ez,  bool IsRolledShape = true)
        {
            return GetCompressionMember(Shape, L_ex, L_ey, L_ez, mat.YieldStress, mat.ModulusOfElasticity, IsRolledShape);
        }
        public ISteelCompressionMember GetCompressionMember(ISection Shape, double L_ex, double L_ey, double L_ez, double F_y, double E, bool IsRolledShape = true)
        {
            string DEFAULT_EXCEPTION_STRING = "Selected shape is not supported. Select a different shape.";
            ISteelCompressionMember col = null;
            CalcLog log = new CalcLog();
            SteelMaterial Material = new SteelMaterial(F_y, E);

            if (Shape is ISectionI)
            {
                ISectionI IShape = Shape as ISectionI;
                SteelSectionI SectionI = new SteelSectionI(IShape, Material);
                IShapeFactory IShapeFactory = new IShapeFactory();
                return IShapeFactory.GetIshape(SectionI, IsRolledShape, L_ex, L_ey, L_ez, log);
 
            }


            else if (Shape is ISectionChannel)
            {
                ISectionChannel ChannelShape = Shape as ISectionChannel;
                SteelChannelSection ChannelSection = new SteelChannelSection(ChannelShape, Material);
                throw new Exception(DEFAULT_EXCEPTION_STRING);
 
            }


            else if (Shape is ISectionPipe)
            {
                ISectionPipe SectionPipe = Shape as ISectionPipe;
                SteelPipeSection PipeSection = new SteelPipeSection(SectionPipe, Material);
                ChsShapeFactory ChsFactory = new ChsShapeFactory();
                return ChsFactory.GetChsShape(PipeSection, L_ex, L_ey, L_ez, log);
 
            }

            else if (Shape is ISectionTube)
            {
                ISectionTube TubeShape = Shape as ISectionTube;
                SteelRhsSection RectHSS_Section = new SteelRhsSection(TubeShape, Material);
                RhsShapeFactory RhsFactory = new RhsShapeFactory();
                return RhsFactory.GetRhsShape(RectHSS_Section, L_ex, L_ey, L_ez, log);
 
            }


            else if (Shape is ISectionBox)
            {
                ISectionBox BoxShape = Shape as ISectionBox;
                SteelBoxSection BoxSection = new SteelBoxSection(BoxShape, Material);

                RhsShapeFactory RhsFactory = new RhsShapeFactory();
                return RhsFactory.GetRhsShape(BoxSection, L_ex, L_ey, L_ez, log);
 
            }

            else if (Shape is ISectionTee)
            {
                ISectionTee TeeShape = Shape as ISectionTee;
                SteelTeeSection TeeSection = new SteelTeeSection(TeeShape, Material);
                throw new Exception(DEFAULT_EXCEPTION_STRING);
            }
            else
            {
                throw new Exception(DEFAULT_EXCEPTION_STRING);
            }


        }

    }
    
}
