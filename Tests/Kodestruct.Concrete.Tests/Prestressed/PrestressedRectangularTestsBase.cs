//Sample license text.

//using NUnit.Framework;
//using Rhino.Mocks;
//using System.Collections.Generic;
//using Kodestruct.Analytics.ACI318_14.Tests.Flexure;
//using Kodestruct.Concrete.ACI;
//using Kodestruct.Concrete.ACI.Infrastructure.Entities.Rebar;
//using Kodestruct.Concrete.ACI318_14;


//namespace Kodestruct.Analytics.ACI318_14.Tests.Prestressed
//{
//    [TestFixture]
//    public partial class PrestressedRectangularTestBase : PrestressedBeamTestBase
//    {



//        private IPrestressedConcreteMaterial mat;

//        public IPrestressedConcreteMaterial Material
//        {
//            get
//            {
//                if (mat == null)
//                {
//                    //mat = new ConcretePrestressed(5000,3500, ConcreteTypeByWeight.Normalweight, log);
//                    mat = GetPrestressedConcreteMaterial(5000, 3500);
//                }
//                return mat;
//            }
//            set { mat = value; }
//        }

//        public PrestressedRectangularTestBase()
//        {


//        }

//        protected PrestressedConcreteSection GetRectangularPrestressedConcreteBeam(double Width, double Height, double fc,
//            double fci, params RebarInput[] rebarInput)
//        {

//            IPrestressedConcreteSection Section = GetRectangularPrestressedSection(Width, Height, fc, fci);

//            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
//            foreach (var bar in rebarInput)
//            {
//                Rebar thisBar = new Rebar(bar.Area, new MaterialAstmA615(Concrete.ACI.Entities.A615Grade.Grade60));
//                RebarPoint point = new RebarPoint(thisBar, new RebarCoordinate() { X = 0, Y = -Height / 2.0 + bar.Cover });
//                LongitudinalBars.Add(point);
//            }

//            PrestressedConcreteSection beam = new PrestressedConcreteSection
//                (Section, LongitudinalBars,
//              CrossSectionLocationType.EndOfSImplySupported, MemberClass.U, Log);
//            return beam;
//        }



//        protected IPrestressedConcreteSection GetRectangularPrestressedSection(double Width, double Height, double fc, double fci)
//        {
//            IPrestressedConcreteMaterial mat = GetPrestressedConcreteMaterial(fc, fci);
//            PrestressedConcreteSectionRectangular section = new PrestressedConcreteSectionRectangular
//                (mat, null, Width, Height);
//            return section;
//        }
//    }
//}
