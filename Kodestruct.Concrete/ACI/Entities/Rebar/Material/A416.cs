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
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI
{
    //prestressed strand
    public partial class MaterialAstmA416 : IPrestressedRebarMaterial
    {
        public MaterialAstmA416(A416Grade Grade, StrandType StrandType)
        {
            this.grade = Grade;
            this.strandType = StrandType;
            yieldStress = Grade == A416Grade.Grade250 ? 225000.0 : 243000.0; //psi
            fpu = Grade == A416Grade.Grade250 ? 250000.0 : 270000.0; //psi
        }

        private A416Grade grade;

        public A416Grade ReinforcementGrade
        {
            get { return grade; }
            set { grade = value; }
        }

        private StrandType strandType;

        public StrandType StrandType
        {
            get { return strandType; }
            set { strandType = value; }
        }

        double fpu;



         public double GetPermissibleStressAtJacking()
         {
             throw new NotImplementedException();
         }

         public double GetPermissibleStressAtTransfer()
         {
             throw new NotImplementedException();
         }

         public double GetStressAtNominalFlexuralStrength()
         {
             throw new NotImplementedException();
         }

         public string Name { get; set; }
         public double StressAtJacking { get; set; }
         public double StressAtTransfer { get; set; }
         public double StressEffective { get; set; }

         double yieldStress;
         public double YieldStress
         {
             get { return yieldStress; }
         }

         public double GetUltimateStrain(double Diameter)
         {
             throw new NotImplementedException();
         }

         public double GetStress(double Strain)
         {
             throw new NotImplementedException();
         }

         public double GetDesignStress()
         {
             throw new NotImplementedException();
         }



        /// <summary>
        /// Per ACI section 21.2.2.2 For all prestressed reinforcement, epsilon_ty shall be taken as 0.002
        /// </summary>

         public double YieldStrain
         {
             get { return 0.002; }
         }
    }


}
