
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Analysis.Tests.BeamForces
{
    [TestFixture]
    public class BeamForces_C2A_1Tests
    {
        IAnalysisBeam GetBeam (double L, double a, double P)
        {

            BeamFactoryData dat = new BeamFactoryData(L,P,0, 0, a, 0,  0, 0, 0, 0, 0,29000,510); //W18X35
            string BeamCaseId = "C2A_2";
            BeamLoadFactoryLocator loc = new BeamLoadFactoryLocator();
            IBeamLoadFactory loadFactory = loc.GetLoadFactory(BeamCaseId,dat);
            LoadBeam load = loadFactory.GetLoad(BeamCaseId);
            BeamInstanceFactory beamFactory = new BeamInstanceFactory(dat);
            IAnalysisBeam bm = beamFactory.CreateBeamInstance(BeamCaseId, load, null);
            return bm;
        }
        [Test]
        public void C2A_1ReturnsNegativeMoment()
        {
            double L = 120;
            double a = 48;
            double P = -20;
            IAnalysisBeam bm = GetBeam(L,a,P);
            double X = L;

            double M_min = bm.GetMomentMinimum().Value;
            Assert.AreEqual(-48*20, M_min);
           //double M_x = bm.GetMoment(X);
           //double V_x = bm.GetShear(X);
           //
           //double M_min = bm.GetMomentMinimum();
           //double V_max = bm.GetShearMaximumValue();
        }

        //[Test]
        //public void C1B_1ReturnsMaxDeflection()
        //{
        //    double L = 240;
        //    double w = 0.125;
        //    IAnalysisBeam bm = GetBeam(L, w);
        //    double Delta = bm.GetMaximumDeflection();
        //    Assert.AreEqual(0.3651, Math.Round(Delta, 4));
        //}
    }
}
