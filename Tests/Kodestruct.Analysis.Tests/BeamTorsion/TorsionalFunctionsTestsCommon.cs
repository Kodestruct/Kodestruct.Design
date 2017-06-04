 
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Analysis.Torsion;

namespace Kodestruct.Analysis.Tests.BeamTorsion
{
    [TestFixture]
    public partial class TorsionalFunctionsTests : ToleranceTestBase
    {
        private void SetAiscDG9Example5_1Parameters()
        {

            L=180;
            G = 11200;
            T_u = -90;
            J = 1.39;
            a = 62.1;
            C_w = 2070;
            W_no=23.6;
            S_w1=33.0;
            Q_f=13.0;
            Q_w=30.2;
            E = 29000;
            t=0;
            alpha = 0.5;
            tolerance = 0.07; //7% can differ from imprecision of graphs
        }

        private void SetAiscDG9Example5_5Parameters()
        {

            L = 144;
            G = 11200;
            T_u = 0;
            J = 1.23;
            a = 42.4;
            C_w = 852;
            W_no = 22.0;
            W_n2 = 10.4;
            S_w1 = 17.4;
            S_w2 = 13.5;
            S_w3 = 6.75;
            Q_f = 19.7;
            Q_w = 37.9;
            E = 29000;
            t = 6.66/12;
            alpha = 0.5;
            tolerance = 0.07; //7% can differ from graph approximation
        }

            double L;
            double alpha;
            double t;
            double G;
            double E;
            double T_u;
            double J;
            double a;
            double C_w;
            double W_no;
            double W_n2;
            double S_w1;
            double S_w2;
            double S_w3;
            double Q_f;
            double Q_w;
        double z;
        double tolerance;

    }
}
