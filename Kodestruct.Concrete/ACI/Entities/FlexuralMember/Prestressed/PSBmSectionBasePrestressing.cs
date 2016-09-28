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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kodestruct.Concrete.ACI
{
    public abstract partial class PrestressedBeamSectionBase : ConcreteFlexuralSectionBase, IConcreteFlexuralMember
    {

        public double GetPrestressForceAtJacking()
        {
            //jacking force affects only the reinforcement checks
            //by the time concrete sees those forces they are "at transfer" forces
            throw new NotImplementedException();
        }

        public double GetPrestressForceAtTransfer()
        {
            throw new NotImplementedException();
        }

        private double GetPrestressingForceEccentricityAtTransfer()
        {
            throw new NotImplementedException();
        }

        public double GetPrestressForceEffective()
        {
            throw new NotImplementedException();
        }
    }
}
