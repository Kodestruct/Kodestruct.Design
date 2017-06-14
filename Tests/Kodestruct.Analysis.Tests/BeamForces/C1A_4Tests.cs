
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Analysis.Tests.BeamForces
{
    //[TestFixture]
    //public class BeamForces_C1A_4Tests
    //{
    //    IAnalysisBeam GetBeam(double L, double a, double P)
    //    {

    //        BeamFactoryData dat = new BeamFactoryData(L, P, 0, 0, a, 0, 0, 0, 0, 0, 0, 29000, 510); //W18X35
    //        string BeamCaseId = "C1A_4";
    //        BeamLoadFactoryLocator loc = new BeamLoadFactoryLocator();
    //        IBeamLoadFactory loadFactory = loc.GetLoadFactory(BeamCaseId, dat);
    //        LoadBeam load = loadFactory.GetLoad(BeamCaseId);
    //        BeamInstanceFactory beamFactory = new BeamInstanceFactory(dat);
    //        IAnalysisBeam bm = beamFactory.CreateBeamInstance(BeamCaseId, load, null);
    //        return bm;
    //    }
    //    [Test]
    //    public void C1A_4ReturnsPositiveMoment()
    //    {
    //        double L = 120;
    //        double a = L/2.0;
    //        double P = 20;
    //        IAnalysisBeam bm = GetBeam(L, a, P);
    //        double X = L;

    //        double M_max = bm.GetMomentMinimum().Value;
    //        Assert.AreEqual(-3.0*P*L/16.0, M_min);
    //    }
    }
}