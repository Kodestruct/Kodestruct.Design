using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using KodestructAci = Kodestruct.Concrete.ACI;
using KodestructAci14 = Kodestruct.Concrete.ACI318_14;
using KodestructSection = Kodestruct.Common.Section;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14.Materials;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Concrete.ACI318_14.Tests.Compression
{

    [TestFixture]
    class AciShearWallTests: ConcreteTestBase
    {
        [Test]
        public void ShearWallCalculatesMoment()
        {
            double f_c_prime = 6000;
            double f_c_prime_psi = f_c_prime * 1000;
            RebarMaterialFactory factory = new RebarMaterialFactory();
            string RebarSpecificationId = "A615Grade60";
            IRebarMaterial LongitudinalRebarMaterial = factory.GetMaterial(RebarSpecificationId);
            double h_total = 20 * 12;
            double t_w = 10;
            double N_curtains = 2;
            double s = 12;
            double c_edge = 3;

            BoundaryZone BoundaryZoneTop     = new BoundaryZone(4,2, "No5",6,3,3);
            BoundaryZone BoundaryZoneBottom = new BoundaryZone(4, 2, "No5", 6, 3, 3);
            string RebarSizeId = "No5";

            double P_u = 1000;
            FlexuralCompressionFiberPosition p = FlexuralCompressionFiberPosition.Top;
            ConcreteMaterial Concrete = new ConcreteMaterial(f_c_prime_psi, ConcreteTypeByWeight.Normalweight, null);
            ConfinementReinforcementType ConfinementReinforcementType = KodestructAci.ConfinementReinforcementType.Ties;
            CrossSectionIShape shape = GetIShape(Concrete, h_total, t_w, BoundaryZoneTop, BoundaryZoneBottom);

            List<KodestructAci.RebarPoint> LongitudinalBars = GetLongitudinalBars(shape.SliceableShape as ISectionI, h_total, t_w, RebarSizeId, N_curtains, s, c_edge,
            BoundaryZoneTop, BoundaryZoneBottom, LongitudinalRebarMaterial);

            KodestructAci.IConcreteFlexuralMember fs = new KodestructAci14.ConcreteSectionFlexure(shape, LongitudinalBars, null, ConfinementReinforcementType);

            IConcreteSectionWithLongitudinalRebar Section = fs as IConcreteSectionWithLongitudinalRebar;
            ConcreteSectionCompression column = new ConcreteSectionCompression(Section, ConfinementReinforcementType, null);
            //Convert axial force to pounds
            double P_u_pounds = P_u * 1000.0;
            double phiM_n = column.GetDesignMomentWithCompressionStrength(P_u_pounds, p).phiM_n / 1000.0; // convert to kip inch units
        }

        private List<KodestructAci.RebarPoint> GetWallBars(double h, double t_w, string RebarSizeId, double N_curtains, double s, double c_edge, IRebarMaterial LongitudinalRebarMaterial)
        {

            RebarDesignation des;
            bool IsValidString = Enum.TryParse(RebarSizeId, true, out des);
            if (IsValidString == false)
            {
                throw new Exception("Rebar size is not recognized. Check input.");
            }
            RebarSection sec = new RebarSection(des);
            double A_b = sec.Area;

            int NBarLines = (int)Math.Floor(h / s);
            double A_s = NBarLines * N_curtains * A_b;
            RebarLine Line = new RebarLine(A_s,
            new Point2D(0.0, -h / 2.0 + c_edge),
            new Point2D(0.0, h / 2.0 - c_edge),
            LongitudinalRebarMaterial, false, false, NBarLines);

            return Line.RebarPoints;
        }

        private List<KodestructAci.RebarPoint> GetBoundaryZoneBars(BoundaryZone BoundaryZone, IRebarMaterial LongitudinalRebarMaterial, Point2D BzCentroid, bool IsTop)
        {

            Point2D topPoint;
            Point2D botPoint;

            if (IsTop == true)
            {
                topPoint = new Point2D(0, BzCentroid.Y + (BoundaryZone.h / 2.0 - BoundaryZone.c_cntrEdge));
                botPoint = new Point2D(0, BzCentroid.Y - (BoundaryZone.h / 2.0 - BoundaryZone.c_cntrInterior));
            }
            else
            {
                topPoint = new Point2D(0, BzCentroid.Y + (BoundaryZone.h / 2.0 - BoundaryZone.c_cntrInterior));
                botPoint = new Point2D(0, BzCentroid.Y - (BoundaryZone.h / 2.0 - BoundaryZone.c_cntrEdge));
            }



            RebarLine Line = new RebarLine(BoundaryZone.A_s,
            botPoint, topPoint, LongitudinalRebarMaterial, false, false, (int)BoundaryZone.N_Bar_Rows - 1);

            return Line.RebarPoints;
        }

        private List<KodestructAci.RebarPoint> GetLongitudinalBars(KodestructSection.Interfaces.ISectionI shape, double h_total, double t_w,
   string RebarSizeId, double N_curtains, double s, double c_edge,
    BoundaryZone BoundaryZoneTop, BoundaryZone BoundaryZoneBottom, IRebarMaterial LongitudinalRebarMaterial)
        {

            List<KodestructAci.RebarPoint> BzTopBars = GetBoundaryZoneBars(BoundaryZoneTop, LongitudinalRebarMaterial, new Point2D(0.0, shape.d / 2.0 - BoundaryZoneTop.h / 2.0), true);
            List<KodestructAci.RebarPoint> BzBottomBars = GetBoundaryZoneBars(BoundaryZoneBottom, LongitudinalRebarMaterial, new Point2D(0.0, -(shape.d / 2.0) + BoundaryZoneTop.h / 2.0), false);

            List<KodestructAci.RebarPoint> retBars = null;
            if (N_curtains != 0)
            {
                List<KodestructAci.RebarPoint> WallBars = GetWallBars(h_total - (BoundaryZoneTop.h + BoundaryZoneBottom.h), t_w, RebarSizeId, N_curtains, s, c_edge, LongitudinalRebarMaterial);
                retBars = BzTopBars.Concat(BzBottomBars).Concat(WallBars).ToList();
            }
            else
            {
                retBars = BzTopBars.Concat(BzBottomBars).ToList();
            }

            return retBars;
        }
        private CrossSectionIShape GetIShape(IConcreteMaterial Material, double h_total, double t_w, BoundaryZone BoundaryZoneTop, BoundaryZone BoundaryZoneBottom)
        {

            double d = h_total;
            double b_fTop = BoundaryZoneTop.b == 0 ? t_w : BoundaryZoneTop.b;
            double b_fBot = BoundaryZoneBottom.b == 0 ? t_w : BoundaryZoneBottom.b;
            double t_fTop = BoundaryZoneTop.h;
            double t_fBot = BoundaryZoneBottom.h;

            return new CrossSectionIShape(Material, null, d, b_fTop, b_fBot, t_fTop, t_fBot, t_w);

        }

    }

    public class BoundaryZone
    {

        public double b { get; set; }

        public double h { get; set; }

        public double A_s { get; set; }

        public double N_curtains { get; set; }

        public string RebarSizeId { get; set; }

        public double s { get; set; }

        public double c_cntrEdge { get; set; }

        public double N_Bar_Rows { get; set; }

        public double c_cntrInterior { get; set; }

        public BoundaryZone(double N_curtains, double N_Bar_Rows, string RebarSizeId, double s, double c_cntrEdge,
                            double c_cntrInterior, double b = 0)
        {


            h = (N_Bar_Rows - 1) * s + c_cntrEdge + c_cntrInterior;
            this.b = b;
            this.N_curtains = N_curtains;
            this.N_Bar_Rows = N_Bar_Rows;
            this.RebarSizeId = RebarSizeId;
            this.s = s;
            this.c_cntrEdge = c_cntrEdge;
            this.c_cntrInterior = c_cntrInterior;


            RebarDesignation des;
            bool IsValidString = Enum.TryParse(RebarSizeId, true, out des);
            if (IsValidString == false)
            {
                throw new Exception("Rebar size is not recognized. Check input.");
            }
            RebarSection sec = new RebarSection(des);
            double A_b = sec.Area;
            A_s = N_Bar_Rows * N_curtains * A_b;
        }

    }
}
