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
using Kodestruct.Steel.AISC.Steel.Entities.Sections;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public abstract partial class RhsTrussBranchConnection:  HssTrussConnection, IHssTrussBranchConnection
    {

   
        private double _theta;

        public double theta
        {
            get {
                _theta = thetaMain;
                return _theta; }
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
            return B_b / B;
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
            return B / (2.0 * t);
        }

        /// <summary>
        /// Load length parameter
        /// </summary>
        private double _eta;

        protected double eta
        {
            get {
                _beta = Get_eta();
                return _eta; }
            set { _eta = value; }
        }

        private double Get_eta()
        {
            return l_b / B;
        }


        private double _B;

        protected double B
        {
            get {
                _B = Chord.Section.B;
                return _B; }
            set { _B = value; }
        }

        private double _H;

        protected double H
        {
            get
            {
                _H = Chord.Section.H;
                return _H;
            }
            set { _H = value; }
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




        private double _B_b;

        protected double B_b
        {
            get
            {
                SteelRhsSection br = getBranch();
                _B_b = br.Section.B;
                return _B_b;
            }
            set { _B = value; }
        }

        private double _H_b;

        public double H_b
        {
            get {
                SteelRhsSection br = getBranch();
                _H_b = br.Section.H;
                return _H_b; }
            set { _H_b = value; }
        }


        private double _t_b;

        public double t_b
        {
            get {

                SteelRhsSection br = getBranch();
                _t_b = br.Section.t_des;
                return _t_b; }
            set { _t_b = value; }
        }


        private double _beta_eop;

        public double beta_eop
        {
            get {
                _beta_eop = Get_beta_eop();
                return _beta_eop; }
            set { _beta_eop = value; }
        }
        
        public double Get_beta_eop()
        {
            double beta_eop = ((5 * beta) / (gamma));
            beta_eop = beta_eop > beta ? beta : beta_eop;
            return beta_eop;
        }

        private double _E;

        public double E
        {
            get {
                _E = SteelConstants.ModulusOfElasticity;
                return _E; }
            set { _E = value; }
        }
        

        private double _l_b;

        public double l_b
        {
            get {
                _l_b = H_b / sin_theta;
                return _l_b; }
            set { _l_b = value; }
        }


        /// <summary>
        /// outside corner radius of HSS
        /// is 1.5t 
        /// </summary>
        private double _k;

        public double k
        {
            get {
                return 1.5 * t;
                return _k; }
            set { _k = value; }
        }
        
        protected abstract SteelRhsSection getBranch();

    }


}
