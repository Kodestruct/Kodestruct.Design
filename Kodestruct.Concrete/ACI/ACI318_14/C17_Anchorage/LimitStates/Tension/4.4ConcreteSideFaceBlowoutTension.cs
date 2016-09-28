#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

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
 
using Kodestruct.Concrete.ACI318_14.Anchorage.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI318_14.Anchorage.LimitStates
{
    public class ConcreteSideFaceBlowoutTension : AnchorageConcreteLimitState
    {
        public ConcreteSideFaceBlowoutTension
            (int n, double h_eff, AnchorInstallationType AnchorType
            )
            : base(n, h_eff, AnchorType)
        {

        }
        public override double GetNominalStrength()
        {
            double Nsb =   GetNsb ();
            double Nsbg = GetNsbg(Nsb);

            double phi = GetPhi();
            double phiNsbg = phi*Nsbg;
            return phiNsbg;

        }

        private double GetPhi()
        {
            //=if(B5=Z4,if(AnchorType=O5,0.75,if(B6=AA4,0.75,if(B6=AA5,0.65,0.55))),if(AnchorType=O5,0.7,if(B6=AA4,0.65,if(B6=AA5,0.55,0.45))))
            throw new NotImplementedException();
        }

        private double GetNsbg(double Nsb)
        {
            //=if(MultipleHeadedCloseToEdge=S4,noOfSuchAnchors(1+s_0/6/ca_MIN)+(B7-noOfSuchAnchors)*if(ca_MIN<0.4*h_eff,160*ca_MIN*l*SQRT(Abrg*fc)/1000,10000),B7*Nsb)
            throw new NotImplementedException();
        }

        private double GetNsb()
        {
            //=if(ca_MIN<0.4*h_eff,160*ca_MIN*l*SQRT(Abrg*fc)/1000*if(ca_2<3*ca_MIN,(1+ca_2/ca_MIN)/4,1),10000)
            throw new NotImplementedException();
        }
    }
}
