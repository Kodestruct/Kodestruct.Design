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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public class BoltValues
    {
        public const string R = "R";
        public const string phiRn = "phiRn";
        public const string Rn_Omega = "Rn_Omega";
        public const string Fnt = "Fnt";
        public const string Fnv = "Fnv";
        public const string phiFnv = "phiFnv";
        public const string Fnv_Omega = "Fnv_Omega";
        public const string F_pp_nt = "F_pp_nt";
        public const string frv = "frv";
        public const string Ab = "Ab";
        public const string Vb = "Vb";
        public const string db = "db";
        public const string mu = "mu";
        public const string Du = "Du";
        public const string hf = "hf";
        public const string Tb = "Tb";
        public const string ns = "ns";
        public const string ksc = "ksc";
        public const string Tu= "Tu";
        public const string Ta = "Ta";
    }

    public class BoltDescriptions
    {
        public class phiRn
        {
            public const string ShearStrength = "Bolt-phiRn-Shear";
            public const string TensileStrength = "Bolt-phiRn-Tensile";
            public const string SlipResistance = "Bolt-phiRn-SlipResistance";
            public const string AvailableTensileStrengthForCombinedLoad = "Bolt-phiRn-AvailableTensileStrengthForCombinedLoad";
        }
        public class Rn_Omega
        {
            public const string ShearStrength = "Bolt-Rn_Omega-Shear";
            public const string TensileStrength = "Bolt-Rn_Omega-Tensile";
            public const string SlipResistance = "Bolt-Rn_Omega-SlipResistance";
            public const string AvailableTensileStrengthForCombinedLoad = "Bolt-Rn_Omega-AvailableTensileStrengthForCombinedLoad";
        }

        public const string Fnt = "Fnt";
        public const string Fnv = "Fnv";
        public const string phiFnv = "phiFnv";
        public const string Fnv_Omega = "Fnv_Omega";
        public const string F_pp_nt = "F_pp_nt";
        public const string frv = "frv";
        public const string Ab = "Ab";
        public const string Vb = "Vb";
        public const string db = "db";
        public const string Du = "Du";
        public const string hf = "hf";
        public const string Tb = "Tb";
        public const string ns = "ns";
        public const string F_pp_ntMax= "F_pp_ntMax";
        public const string ksc = "ksc";
 
        public class mu
        {
          public const string ClassA = "mu_ClassA";
          public const string ClassB = "mu_ClassB";  
        }
    }

    public class BoltFormulas
    {
        public class J3_1
        {
            public const string LRFD = "J3-1_LRFD";
            public const string ASD = "J3-1_ASD";
        }

        public class J3_2
        {
            public const string LRFD = "J3-2_LRFD";
            public const string ASD = "J3-2_ASD";
        }

        public class J3_3
        {
            public const string a = "J3-3a";
            public const string b = "J3-3b";
        }

        public class J3_4
        {
            public const string LRFD = "J3-4_LRFD";
            public const string ASD = "J3-4_ASD";
        }

        public class J3_5
        {
            public const string LRFD = "J3-5_LRFD";
            public const string ASD = "J3-5_ASD";
        }

        public const string F_pp_ntMax = "F_pp_ntMax";
        public const string mu = "mu";
        public const string Du = "Du";
        public const string hf = "hf";
        public const string Tb = "Tb";
        public const string ns = "ns";
        public const string frv = "frv";

    }
    public class BoltParagraphs
    {

    }
}
