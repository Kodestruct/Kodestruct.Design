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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class StandardHookInTension: Development
    {
        

        public StandardHookInTension(IConcreteMaterial Concrete, Rebar Rebar, 
             ICalcLog log,double ExcessFlexureReinforcementRatio)
            : base (Concrete, Rebar, ExcessFlexureReinforcementRatio,log)
        {
            db = Rebar.Diameter;
        }


        //public StandardHookInTension(IConcreteMaterial Concrete, Rebar Rebar,
        //        ICalcLog log, HookType HookType, double SideCover, double BarExtensionCover,
        //        double ExcessFlexureReinforcementRatio = 1.0)
        //        :this(Concrete,Rebar,log,ExcessFlexureReinforcementRatio)
        //{
        //    this.hookType = HookType;
        //    this.sideCover = SideCover;
        //}

        //public StandardHookInTension(IConcreteMaterial Concrete, Rebar Rebar,
        //ICalcLog log, HookType HookType, double SideCover, double BarExtensionCover,
        //    double EnclosingRebarSpacing,
        //    bool EnclosingRebarIsPerpendicular, double ExcessFlexureReinforcementRatio = 1.0)
        //    : this(Concrete, Rebar, log, HookType,SideCover, BarExtensionCover,
        //        ExcessFlexureReinforcementRatio)
        //{
        //    this.enclosingRebarSpacing = EnclosingRebarSpacing;
        //    this.enclosingRebarIsPerpendicular = EnclosingRebarIsPerpendicular;
        //}

        //private HookType hookType;

        //public HookType HookType
        //{
        //    get { return hookType; }
        //    set { hookType = value; }
        //}

        //private double sideCover;

        //public double SideCover
        //{
        //    get { return sideCover; }
        //    set { sideCover = value; }
        //}

        //private double barExtensionCover;

        //public double BarExtensionCover
        //{
        //    get { return barExtensionCover; }
        //    set { barExtensionCover = value; }
        //}
        

        //private double enclosingRebarSpacing;

        //public double EnclosingRebarSpacing
        //{
        //    get { return enclosingRebarSpacing; }
        //    set { enclosingRebarSpacing = value; }
        //}

        //private bool enclosingRebarIsPerpendicular;

        //public bool EnclosingRebarIsPerpendicular
        //{
        //    get { return enclosingRebarIsPerpendicular; }
        //    set { enclosingRebarIsPerpendicular = value; }
        //}
        

        double db;
    }
}
