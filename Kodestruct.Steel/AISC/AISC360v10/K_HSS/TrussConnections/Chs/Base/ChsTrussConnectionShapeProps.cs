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
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Common.Mathematics;
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public abstract partial class ChsTrussBranchConnection:  HssTrussConnection, IHssTrussBranchConnection
    {

        private double _theta;

        public double theta
        {
            get
            {
                _theta = thetaMain;
                return _theta;
            }
            set { _theta = value; }
        }

        private double _sin_theta;

        public double sin_theta
        {
            get {
                _sin_theta = Math.Sin(theta.ToRadians());
                return _sin_theta; }
            set { _sin_theta = value; }
        }
        
        /// <summary>
        /// Width ratio  B_b/B
        /// </summary>
        private double _beta;

        protected double beta
        {
            get {
                _beta = Get_beta();
                return _beta; }
            set {
                
                _beta = value; }
        }

        private double Get_beta()
        {

           return D_b / D;
        }

        private double _D_b;

        public double D_b
        {
            get 
            {
                double D_b = 0.0;

                if (IsMainBranch == true)
                {
                    D_b = MainBranch.Section.D;
                }
                else
                {
                    D_b = SecondBranch.Section.D;
                }

                return _D_b; 
            }
            set { _D_b = value; }
        }
        
        private double _gamma;

        /// <summary>
        /// Chord slenderness ratio
        /// </summary>
        protected double gamma
        {
            get {
                _gamma = Get_gamma();
                return _gamma; }
            set { _gamma = value; }
        }

        private double Get_gamma()
        {

            return D / (2.0 * t);
        }


        private double _t;

        protected double t
        {
            get
            {
                _t = Chord.Section.t_des;
                return _t;
            }
            set { _t = value; }
        }


        protected override double GetF_y()
        {
            return Chord.Material.YieldStress; 
        }

        protected override double GetF_yb()
        {
            return MainBranch.Material.YieldStress;
        }


        private double _D;

        public double D
        {
            get {
                _D = Chord.Section.D;
                return _D; }
            set { _D = value; }
        }


        private double _D_bComp;

        public double D_bComp
        {
            get {
                _D_bComp = MainBranch.Section.D;
                return _D_bComp; }
            set { _D_bComp = value; }
        }

        //private double _t_b;

        //public double t_b
        //{
        //    get
        //    {

        //        SteelChsSection br = getBranch();
        //        _t_b = br.Section.t_des;
        //        return _t_b;
        //    }
        //    set { _t_b = value; }
        //}


        private double _t_b;

        public double t_b
        {
            get {

                SteelChsSection br = getBranch();
                _t_b = br.Section.t_des;
                return _t_b; }
            set { _t_b = value; }
        }



        private double _E;

        public double E
        {
            get {
                _E = SteelConstants.ModulusOfElasticity;
                return _E; }
            set { _E = value; }
        }
        

        
        protected abstract SteelChsSection getBranch();

    }


}
