using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI318_14.Materials;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.General;

namespace Kodestruct.Concrete.ACI.ACI318_14
{
    public class CompressionSectionFactory
    {

        public ConcreteSectionCompression GetCompressionMemberFromFlexuralSection(ConcreteSectionFlexure flexuralSection,
        CompressionMemberType CompressionMemberType)
        {
            
            CalcLog log = new CalcLog();

            ConcreteSectionCompression compSection = new ConcreteSectionCompression(flexuralSection, CompressionMemberType, log);
            return compSection;
        }

        public ConcreteSectionCompression GetCompressionMemberFromFlexuralSection(ConcreteSectionFlexure flexuralSection,
        ConfinementReinforcementType ConfinementReinforcement, bool IsPrestressed = false)
        {

            CalcLog log = new CalcLog();
            CompressionMemberType CompressionMemberType = GetCompressionMemberType(ConfinementReinforcement, IsPrestressed);

            ConcreteSectionCompression compSection = new ConcreteSectionCompression(flexuralSection, CompressionMemberType, log);
            return compSection;
        }

        private CompressionMemberType GetCompressionMemberType(ConfinementReinforcementType ConfinementReinforcement, bool IsPrestressed)
        {
            CompressionMemberType CompressionMemberType = Concrete.ACI318_14.CompressionMemberType.NonPrestressedWithTies;
            switch (ConfinementReinforcement)
            {
                case ConfinementReinforcementType.Spiral:
                    CompressionMemberType = IsPrestressed == false ? CompressionMemberType.NonPrestressedWithSpirals : Concrete.ACI318_14.CompressionMemberType.PrestressedWithSpirals;
                    break;
                case ConfinementReinforcementType.Ties:
                    CompressionMemberType = IsPrestressed == false ? CompressionMemberType.NonPrestressedWithTies : Concrete.ACI318_14.CompressionMemberType.PrestressedWithTies;
                    break;
                default:
                    CompressionMemberType = IsPrestressed == false ? CompressionMemberType.NonPrestressedWithTies : Concrete.ACI318_14.CompressionMemberType.PrestressedWithTies;
                    break;
            }

            return CompressionMemberType;
        }

        public ConcreteSectionCompression GetGeneralCompressionMember(List<Point2D> PolygonPoints, List<RebarPoint> LongitudinalBars,
        IConcreteMaterial ConcreteMaterial, IRebarMaterial RebarMaterial,
        ConfinementReinforcementType ConfinementReinforcement, double b_w=0.0, double d=0.0,  bool IsPrestressed = false)
        {
            CalcLog log = new CalcLog();
            var GenericShape = new PolygonShape(PolygonPoints);
            if (b_w ==0.0)
            {
                b_w = GenericShape.XMax - GenericShape.XMin;
            }

            if (d == 0.0)
            {
                d = GenericShape.YMax - GenericShape.YMin;
            }
            CrossSectionGeneralShape Section = new CrossSectionGeneralShape(ConcreteMaterial, null, GenericShape, b_w, d);
            ConcreteSectionFlexure flexuralSection = new ConcreteSectionFlexure(Section, LongitudinalBars, log, ConfinementReinforcement);

            return GetCompressionMemberFromFlexuralSection(flexuralSection, ConfinementReinforcement, IsPrestressed);

        }

        /// <summary>
        /// Creates compression member from top/bottom and side reinforcement
        /// </summary>
        /// <param name="Width">Width</param>
        /// <param name="Height">Heigth</param>
        /// <param name="ConcreteMaterial">Concrete material (as IConcreteMaterial)</param>
        /// <param name="RebarMaterial"> Rebar material (as IRebarMaterial) </param>
        /// <param name="A_s_TopBottom">Area of left or right reinforcement (each)</param>
        /// <param name="A_s_LeftRight">Area of top or bottom reinforcement (each)</param>
        /// <param name="c_centerTopBottom">Cover to rebar centroid for top and bottom reinforcement</param>
        /// <param name="c_centerLeftRight">Cover to rebar centroid for left and right side reinforcement</param>
        /// <param name="ConfinementReinforcement">Distiguishes between ties and spirals</param>
        /// <param name="IsPrestressed">Distinguishes between prestressed versus non-prestressed members</param>
        /// <returns></returns>
        public ConcreteSectionCompression GetRectangularCompressionMember(double Width, double Height,
             IConcreteMaterial ConcreteMaterial,  IRebarMaterial RebarMaterial,
             double A_s_TopBottom, double A_s_LeftRight,
             double c_centerTopBottom, double c_centerLeftRight,
            ConfinementReinforcementType ConfinementReinforcement, bool IsPrestressed = false)
        {
            CalcLog log = new CalcLog();
            RebarLine topLine = new RebarLine(A_s_TopBottom, 
                new Point2D(-Width / 2.0 + c_centerLeftRight, Height/2.0-c_centerTopBottom),
                new Point2D(Width / 2.0 - c_centerLeftRight, Height / 2.0 - c_centerTopBottom),
                RebarMaterial, false);

            RebarLine botLine = new RebarLine(A_s_TopBottom,
            new Point2D(-Width / 2.0 + c_centerLeftRight, -Height / 2.0 + c_centerTopBottom),
            new Point2D(Width / 2.0 - c_centerLeftRight, -Height / 2.0 + c_centerTopBottom),
            RebarMaterial, false);

            RebarLine leftLine = new RebarLine(A_s_LeftRight,
            new Point2D(-Width / 2.0 + c_centerLeftRight, -Height / 2.0 + c_centerTopBottom),
            new Point2D(-Width / 2.0 + c_centerLeftRight, Height / 2.0 - c_centerTopBottom),
            RebarMaterial, true);

            RebarLine rightLine = new RebarLine(A_s_LeftRight,
            new Point2D(Width / 2.0 - c_centerLeftRight, -Height / 2.0 + c_centerTopBottom),
            new Point2D(Width / 2.0 - c_centerLeftRight, Height / 2.0 - c_centerTopBottom),
            RebarMaterial, true);

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            LongitudinalBars.AddRange(topLine.RebarPoints);
            LongitudinalBars.AddRange(botLine.RebarPoints);
            LongitudinalBars.AddRange(leftLine.RebarPoints);
            LongitudinalBars.AddRange(rightLine.RebarPoints);

            return this.GetRectangularCompressionMember(Width, Height, ConcreteMaterial, LongitudinalBars, ConfinementReinforcement, log, IsPrestressed);
        }


        public ConcreteSectionCompression GetRectangularCompressionMember(double Width, double Height, IConcreteMaterial ConcreteMaterial,
            List<RebarPoint> LongitudinalBars,  ConfinementReinforcementType ConfinementReinforcement, 
            ICalcLog log , bool IsPrestressed = false)
        {


            CrossSectionRectangularShape Section = new CrossSectionRectangularShape(ConcreteMaterial, null, Width, Height);
            CompressionMemberType CompressionMemberType = GetCompressionMemberType(ConfinementReinforcement, IsPrestressed);
            ConcreteSectionCompression column = new ConcreteSectionCompression(Section, LongitudinalBars, CompressionMemberType, log);
            return column;
        }

        public   ConcreteSectionCompression GetRectangularCompressionMember(double Width, double Height, double fc, List<RebarPoint> LongitudinalBars, 
        ConfinementReinforcementType ConfinementReinforcement, bool IsPrestressed = false)
        {

            CalcLog log = new CalcLog();
            ConcreteMaterial mat = new ConcreteMaterial(fc, ConcreteTypeByWeight.Normalweight, log);
            CrossSectionRectangularShape Section = new CrossSectionRectangularShape(mat, null, Width, Height);
            CompressionMemberType CompressionMemberType = GetCompressionMemberType(ConfinementReinforcement, IsPrestressed);
            ConcreteSectionCompression column = new ConcreteSectionCompression(Section, LongitudinalBars, CompressionMemberType, log);
            return column;
        }
      }

    
}
