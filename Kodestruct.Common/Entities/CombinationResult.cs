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
using Kodestruct.Common.Interfaces;

namespace Kodestruct.Common.Entities
{
    public class CombinationResult: ICombinationResult
    {
        private string combinationName;

        public string CombinationName
        {
            get { return combinationName; }
        }

        private List<IForce> forces;

        public List<IForce> Forces
        {
            get { return forces; }
        }
        
    public void AddForce(IForce Force)
    {
        if (forces == null)
        {
            forces = new List<IForce>();
        }
        forces.Add(Force);
    }

        public CombinationResult(string CombinationName)
        {
            this.combinationName = CombinationName;
        }
        public CombinationResult(string CombinationName, List<IForce> Forces)
        {
            this.combinationName = CombinationName;
            this.forces = Forces;
        }

    }
}
