
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Analysis.Tests.BeamForces
{
    [TestFixture]
    public class BeamForces_C1B_1Tests
    {
        IAnalysisBeam GetBeam (double L, double w)
        {

            BeamFactoryData dat = new BeamFactoryData(L, 0, 0, w, 0, 0, 0, 0, 0, 0, 0,29000,510); //W18X35
            string BeamCaseId = "C1B_1";
            BeamLoadFactoryLocator loc = new BeamLoadFactoryLocator();
            IBeamLoadFactory loadFactory = loc.GetLoadFactory(BeamCaseId,dat);
            LoadBeam load = loadFactory.GetLoad(BeamCaseId);
            BeamInstanceFactory beamFactory = new BeamInstanceFactory(dat);
            IAnalysisBeam bm = beamFactory.CreateBeamInstance(BeamCaseId, load, null);
            return bm;
        }
        [Test]
        public void C1B_1ReturnsPositiveMoment()
        {
            double L = 20;
            double w = 1.5;
            IAnalysisBeam bm = GetBeam(L,w);

            double M_max = bm.GetMomentMaximum().Value;
            Assert.AreEqual(75, M_max);

        }

        [Test]
        public void C1B_1ReturnsPositiveMomentAtX()
        {
            double L = 20;
            double w = 1.5;
            IAnalysisBeam bm = GetBeam(L, w);
            double X = L / 2;
            
            double M_x = bm.GetMoment(X);

            Assert.AreEqual(75, M_x);
            //double V_x = bm.GetShear(X);
            //
            //double M_min = bm.GetMomentMinimum();
            //double V_max = bm.GetShearMaximumValue();
        }

        [Test]
        public void C1B_1ReturnsMaxDeflection()
        {
            double L = 240;
            double w = 0.125;
            IAnalysisBeam bm = GetBeam(L, w);
            double Delta = bm.GetMaximumDeflection();
            Assert.AreEqual(0.3651, Math.Round(Delta, 4));
        }
    }
}
