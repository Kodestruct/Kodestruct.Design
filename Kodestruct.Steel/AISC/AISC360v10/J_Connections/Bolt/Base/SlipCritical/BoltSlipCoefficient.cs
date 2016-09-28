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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using p = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltParagraphs;
using f = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltFormulas;
using v = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltValues;
using d = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltDescriptions;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public abstract partial class BoltSlipCritical : Bolt, IBoltSlipCritical
    {
        internal double GetSlipCoefficient()
        {
            
            ICalcLogEntry ent = Log.CreateNewEntry();
            ent.ValueName = v.mu;
            ent.Reference = "AISC Section J3-8";
            ent.FormulaID = f.mu;
            double mu=0.3;
            switch (FayingSurface)
            {
                case BoltFayingSurfaceClass.ClassA:
                     ent.DescriptionReference = d.mu.ClassA;
                    return 0.3;
                case BoltFayingSurfaceClass.ClassB:
                    ent.DescriptionReference = d.mu.ClassB;
                    return 0.5;
            }
            ent.VariableValue = mu.ToString();
            AddToLog(ent);
            return mu;
        }
    }
}
