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
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindStructure : AnalyticalElement
    {

        public double GetGustFacor
            (
            WindStructureDynamicResponseType DynamicClassification,
            double B    ,
            double h    ,
            double L    ,
            double beta ,
            double n1   ,
            double V    ,
            WindExposureCategory WindExposure
            )
        {
            this.Width = B;
            this.Height = h;
            this.Length = L;
            this.Damping = beta;
            this.n1 = n1;
            this.WindSpeed = V;


            double G = 0.85;
            CalculateTerrainCoefficients(WindExposure);
            if (DynamicClassification == WindStructureDynamicResponseType.Flexible)
            {

                double z_ob = GetEquivalentHeightZob();
                double Iz = GetTurbulenceIntensityIz();
                double V_z_ob = GetMeanHourlyWindVz();
                double Lz = GetIntegralLengthScaleOfTurbulenceLz();
                double R = GetResonantResponseFactorR();
                double gQ = 3.4;
                double gv = 3.4;
                double gR = GetPeakFactorForResonantResponse_gr();
                double Q = GetBackgroundResponseFactorQ();
                G = GetGustFacrorFlexible(gQ,  Q,  gR,  R,  gv,  Iz);
            }
            else
            {
                double z_ob = GetEquivalentHeightZob();
                double Iz = GetTurbulenceIntensityIz();
                double Lz = GetIntegralLengthScaleOfTurbulenceLz();
                double Q = GetBackgroundResponseFactorQ();
                double gQ = 3.4;
                double gv = 3.4;

                #region g
                ICalcLogEntry gEntry = new CalcLogEntry();
                gEntry.ValueName = "g";
                gEntry.Reference = "";
                gEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorRigidgv.docx";
                gEntry.FormulaID = null; //reference to formula from code
                gEntry.VariableValue = Math.Round(3.4, 3).ToString();
                #endregion
                this.AddToLog(gEntry);
                G =GetGustFactorRigid(gQ, gv, Q, Iz);
                
            }
            return G;
        }

        private double GetGustFactorRigid(double gQ, double gv, double Q, double Iz)
        {
            double tv1 = 1.7 * gQ * Iz * Q;
            double tv2 = 1.7 * gv * Iz;
            double G= 0.925 * (1.0 + tv1) / (1.0 + tv2);

            #region G
            ICalcLogEntry GEntry = new CalcLogEntry();
            GEntry.ValueName = "G";
            GEntry.AddDependencyValue("gQ", Math.Round(gQ, 3));
            GEntry.AddDependencyValue("gv", Math.Round(gv, 3));
            GEntry.AddDependencyValue("Iz", Math.Round(Iz, 3));
            GEntry.AddDependencyValue("Q", Math.Round(Q, 3));
            GEntry.Reference = "";
            GEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/GustFactorRigid.docx";
            GEntry.FormulaID = null; //reference to formula from code
            GEntry.VariableValue = Math.Round(G, 3).ToString();
            #endregion
            this.AddToLog(GEntry);
            return G;
        }

        double GetGustFacrorFlexible(double gQ, double Q, double gR, double R, double gv, double Iz)
        {
            double G;
            double a=(gQ*gQ) * (Q*Q)+(gR*gR) * (R*R);
            double b = 1.0 + 1.7 * gv * Iz;
            G =0.925 * ((1.0 + 1.7 * Iz * Math.Sqrt(a)) / b);

            
            #region G
            ICalcLogEntry GEntry = new CalcLogEntry();
            GEntry.ValueName = "G";
            GEntry.AddDependencyValue("gQ", Math.Round(gQ, 3));
            GEntry.AddDependencyValue("gr", Math.Round(gR, 3));
            GEntry.AddDependencyValue("gv", Math.Round(gv, 3));
            GEntry.AddDependencyValue("Iz", Math.Round(Iz, 3));
            GEntry.AddDependencyValue("R", Math.Round(R, 3));
            GEntry.AddDependencyValue("Q", Math.Round(Q, 3));
            GEntry.Reference = "";
            GEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/GustFactorFlexible.docx";
            GEntry.FormulaID = null; //reference to formula from code
            GEntry.VariableValue = Math.Round(G, 3).ToString();
            #endregion
            this.AddToLog(GEntry);

            return G;
        }

        bool z_obClean;
        private double _z_ob;

        public double z_ob
        {
            get {
                if (z_obClean==false)
                {
                    GetEquivalentHeightZob();
                }
                return _z_ob; }
            set { _z_ob = value; }
        }
        
        

         double GetEquivalentHeightZob()
        {
            double h = this.height;
            if (terrainCoefficientsNeedCalculation == true )
            {
                CalculateTerrainCoefficients(WindExposure);
            }
            double z_ob, z_ob1, z_ob2, z_ob3;
            if (h<zmin)
            {
                z_ob3 = zmin;
                
                #region zob3
                ICalcLogEntry zob3Entry = new CalcLogEntry();
                zob3Entry.ValueName = "z_ob";
                zob3Entry.AddDependencyValue("h", Math.Round(h, 3));
                zob3Entry.Reference = "";
                zob3Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleEquivalentHeightZob3.docx";
                zob3Entry.FormulaID = null; //reference to formula from code
                zob3Entry.VariableValue = Math.Round(z_ob3, 3).ToString();
                #endregion
                this.AddToLog(zob3Entry);
                z_ob = z_ob3;
            }
            else
            {
                z_ob1 = h * 0.6;
                #region zob1
                ICalcLogEntry zob1Entry = new CalcLogEntry();
                zob1Entry.ValueName = "z_ob";
                zob1Entry.AddDependencyValue("h", Math.Round(h, 3));
                zob1Entry.Reference = "";
                zob1Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleEquivalentHeightZob1.docx";
                zob1Entry.FormulaID = null; //reference to formula from code
                zob1Entry.VariableValue = Math.Round(z_ob1, 3).ToString();
                #endregion
                this.AddToLog(zob1Entry);

                z_ob2 = zmin;
                #region zob2
                ICalcLogEntry zob2Entry = new CalcLogEntry();
                zob2Entry.ValueName = "z_ob";
                zob2Entry.AddDependencyValue("h", Math.Round(h, 3));
                zob2Entry.Reference = "";
                zob2Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleEquivalentHeightZob2.docx";
                zob2Entry.FormulaID = null; //reference to formula from code
                zob2Entry.VariableValue = Math.Round(z_ob2, 3).ToString();
                #endregion
                this.AddToLog(zob2Entry);

                z_ob = h * 0.6 > zmin ? h * 0.6 : zmin;

                #region zob
                ICalcLogEntry zobEntry = new CalcLogEntry();
                zobEntry.ValueName = "z_ob";
                zobEntry.AddDependencyValue("h", Math.Round(h, 3));
                zobEntry.Reference = "";
                zobEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleEquivalentHeightZob.docx";
                zobEntry.FormulaID = null; //reference to formula from code
                zobEntry.VariableValue = Math.Round(z_ob, 3).ToString();
                #endregion
                this.AddToLog(zobEntry);
            }


            
             z_obClean = true;
            this.z_ob = z_ob;
            return z_ob;
        }


         bool VzClean;
         private double _Vz_ob;

         public double Vz_ob
         {
             get {
                 if (VzClean ==false)
                 {
                     GetMeanHourlyWindVz();
                 }
                 return _Vz_ob; }
             set { _Vz_ob = value; }
         }

        

        double GetMeanHourlyWindVz()
        {
            if (terrainCoefficientsNeedCalculation == true)
            {
                CalculateTerrainCoefficients(WindExposure);
            }
            double V = windSpeed;
            double Vz_ob = b_ob * Math.Pow(z_ob / 33, alpha_ob) * (88.0 / 60.0) * V;

            
            #region Vz_ob
            ICalcLogEntry Vz_obEntry = new CalcLogEntry();
            Vz_obEntry.ValueName = "Vz";
            Vz_obEntry.AddDependencyValue("b_ob", Math.Round(b_ob, 3));
            Vz_obEntry.AddDependencyValue("z_ob", Math.Round(z_ob, 3));
            Vz_obEntry.AddDependencyValue("alphob", Math.Round(alpha_ob, 3));
            Vz_obEntry.AddDependencyValue("V", Math.Round(V, 3));
            Vz_obEntry.Reference = "";
            Vz_obEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleMeanHourlyWindVz.docx";
            Vz_obEntry.FormulaID = null; //reference to formula from code
            Vz_obEntry.VariableValue = Math.Round(Vz_ob, 3).ToString();
            #endregion
            this.AddToLog(Vz_obEntry);


            this.Vz_ob = Vz_ob;
            VzClean = true;
            return Vz_ob;
        }



        bool IzClean;
        private double _Iz;

        public double Iz
        {
            get {
                if (IzClean==false)
                {
                    GetTurbulenceIntensityIz();
                }
                return _Iz; }
            set { _Iz = value; }
        }
        
        
         double GetTurbulenceIntensityIz()
        {
            if (terrainCoefficientsNeedCalculation == true)
            {
                CalculateTerrainCoefficients(WindExposure);
            }
            double Iz= c * Math.Pow(33.0 / z_ob, 1.0 / 6.0);

            
            #region Iz
            ICalcLogEntry IzEntry = new CalcLogEntry();
            IzEntry.ValueName = "Iz";
            IzEntry.AddDependencyValue("c", Math.Round(c, 3));
            IzEntry.AddDependencyValue("z_ob", Math.Round(z_ob, 3));
            IzEntry.Reference = "";
            IzEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleTurbulenceIntensityIz.docx";
            IzEntry.FormulaID = null; //reference to formula from code
            IzEntry.VariableValue = Math.Round(Iz, 3).ToString();
            #endregion
            this.AddToLog(IzEntry);
            this.Iz = Iz;
            IzClean = true;
            return Iz;
        }


         bool LzClean;
        private double _Lz;

         public double Lz
         {
             get {
                 if (LzClean ==false)
                 {
                     GetIntegralLengthScaleOfTurbulenceLz();
                 }
                 return _Lz; }
             set { _Lz = value; }
         }
         
       
         double GetIntegralLengthScaleOfTurbulenceLz()
        {
            if (terrainCoefficientsNeedCalculation == true)
            {
                CalculateTerrainCoefficients(WindExposure);
            }

            double Lz = l * (Math.Pow(z_ob / 33.0, epsilon_overbar));
            #region Lz
            ICalcLogEntry LzEntry = new CalcLogEntry();
            LzEntry.ValueName = "Lz";
            LzEntry.AddDependencyValue("z_ob", Math.Round(z_ob, 3));
            LzEntry.AddDependencyValue("l", Math.Round(l, 3));
            LzEntry.AddDependencyValue("epsilon_overbar", Math.Round(epsilon_overbar, 3));
            LzEntry.Reference = "";
            LzEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleIntegralLengthScaleOfTurbulenceLz.docx";
            LzEntry.FormulaID = null; //reference to formula from code
            LzEntry.VariableValue = Math.Round(Lz, 3).ToString();
            #endregion
            this.AddToLog(LzEntry);
            this.Lz = Lz;
            LzClean = true;
            return Lz;
        }

        public double GetResonantResponseFactorR()
        {

            double Vz = Vz_ob;
            double beta = Damping;

            double N1 = n1 * Lz / Vz;
            
            #region N1
            ICalcLogEntry N1Entry = new CalcLogEntry();
            N1Entry.ValueName = "N1";
            N1Entry.AddDependencyValue("n1", Math.Round(n1, 3));
            N1Entry.AddDependencyValue("Lz", Math.Round(Lz, 3));
            N1Entry.AddDependencyValue("Vz", Math.Round(Vz, 3));
            N1Entry.Reference = "";
            N1Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleN1.docx";
            N1Entry.FormulaID = null; //reference to formula from code
            N1Entry.VariableValue = Math.Round(N1, 3).ToString();
            #endregion
            this.AddToLog(N1Entry);
            double Rn = 7.47 * N1 / (Math.Pow(1 + 10.3 * N1, 5.0 / 3.0));
            
            #region Rn
            ICalcLogEntry RnEntry = new CalcLogEntry();
            RnEntry.ValueName = "Rn";
            RnEntry.AddDependencyValue("N1", Math.Round(N1, 3));
            RnEntry.Reference = "";
            RnEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexiblegRn.docx";
            RnEntry.FormulaID = null; //reference to formula from code
            RnEntry.VariableValue = Math.Round(Rn, 3).ToString();
            #endregion
            this.AddToLog(RnEntry);

            double eta_L = Get_eta_L();
            double eta_B = Get_eta_B();
            double eta_h = Get_eta_h();

            double Rh = GetRl(eta_h, "h");
            double RB = GetRl(eta_B, "B");
            double RL = GetRl(eta_L, "L");
            double R= Math.Sqrt(1.0 / beta * Rn * Rh * RB * (0.53 + 0.47 * RL));

            
            #region R
            ICalcLogEntry REntry = new CalcLogEntry();
            REntry.ValueName = "R";
            REntry.AddDependencyValue("beta", Math.Round(beta, 3));
            REntry.AddDependencyValue("Rn", Math.Round(Rn, 3));
            REntry.AddDependencyValue("Rh", Math.Round(Rh, 3));
            REntry.AddDependencyValue("RB", Math.Round(RB, 3));
            REntry.AddDependencyValue("RL", Math.Round(RL, 3));
            REntry.Reference = "";
            REntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleR.docx";
            REntry.FormulaID = null; //reference to formula from code
            REntry.VariableValue = Math.Round(R, 3).ToString();
            #endregion
            this.AddToLog(REntry);

            return R;
        }

        double Get_eta_L()
        {
            double Vz = Vz_ob;
            double L = this.Length;
            double eta_L= 15.4 * n1 * L / Vz;

            
            #region eta_L
            ICalcLogEntry eta_LEntry = new CalcLogEntry();
            eta_LEntry.ValueName = "eta";
            eta_LEntry.AddDependencyValue("n1", Math.Round(n1, 3));
            eta_LEntry.AddDependencyValue("L", Math.Round(L, 3));
            eta_LEntry.AddDependencyValue("Vz", Math.Round(Vz, 3));
            eta_LEntry.Reference = "";
            eta_LEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexiblegEta_L.docx";
            eta_LEntry.FormulaID = null; //reference to formula from code
            eta_LEntry.VariableValue = Math.Round(eta_L, 3).ToString();
            #endregion
            this.AddToLog(eta_LEntry);

            return eta_L;
        }

        double Get_eta_B()
        {
            double Vz = Vz_ob;
            double B = this.width;
            double eta_B= 4.6 * n1 * B / Vz;  


            #region eta_B
            ICalcLogEntry eta_BEntry = new CalcLogEntry();
            eta_BEntry.ValueName = "eta";
            eta_BEntry.AddDependencyValue("n1", Math.Round(n1, 3));
            eta_BEntry.AddDependencyValue("B", Math.Round(B, 3));
            eta_BEntry.AddDependencyValue("Vz", Math.Round(Vz, 3));
            eta_BEntry.Reference = "";
            eta_BEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexiblegEta_B.docx";
            eta_BEntry.FormulaID = null; //reference to formula from code
            eta_BEntry.VariableValue = Math.Round(eta_B, 3).ToString();
            #endregion
            this.AddToLog(eta_BEntry);

            return eta_B;
        }


        double Get_eta_h()
        {
            double Vz = Vz_ob;
            double h = this.height;
            double eta_h = 4.6 * n1 * h / Vz;


            #region eta_h
            ICalcLogEntry eta_hEntry = new CalcLogEntry();
            eta_hEntry.ValueName = "eta";
            eta_hEntry.AddDependencyValue("n1", Math.Round(n1, 3));
            eta_hEntry.AddDependencyValue("h", Math.Round(h, 3));
            eta_hEntry.AddDependencyValue("Vz", Math.Round(Vz, 3));
            eta_hEntry.Reference = "";
            eta_hEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexiblegEta_h.docx";
            eta_hEntry.FormulaID = null; //reference to formula from code
            eta_hEntry.VariableValue = Math.Round(eta_h, 3).ToString();
            #endregion
            this.AddToLog(eta_hEntry);

            return eta_h;
        }


        private  double GetRl(double eta, string Subscript)
        {
            double Rl = 1.0;
            if (eta == 0)
            {
                
                #region Rl
                ICalcLogEntry RlEntry = new CalcLogEntry();
                RlEntry.ValueName = "Rl";
                RlEntry.Reference = "";
                RlEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleRl1.docx";
                RlEntry.FormulaID = null; //reference to formula from code
                RlEntry.VariableValue = Subscript;
                #endregion
                this.AddToLog(RlEntry);

            }
            else
            {
                double RBhL = 1 / eta - 1 / (2.0 * Math.Pow(eta, 2)) * (1 - Math.Exp(-2 * eta));
                
                #region RBhL
                ICalcLogEntry RBhLEntry = new CalcLogEntry();
                RBhLEntry.ValueName = "RBhL";
                RBhLEntry.AddDependencyValue("eta", Math.Round(eta, 3));
                RBhLEntry.AddDependencyValue("Rl", Subscript);
                RBhLEntry.Reference = "";
                RBhLEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleRl.docx";
                RBhLEntry.FormulaID = null; //reference to formula from code
                RBhLEntry.VariableValue = Math.Round(RBhL, 3).ToString();
                #endregion
                this.AddToLog(RBhLEntry);

                Rl = RBhL;
            }
            return Rl;
        }



        private double _n1;

        public double n1
        {
            get { return _n1; }
            set { _n1 = value; }
        }
        
        

        public double GetPeakFactorForResonantResponse_gr()
        {
            double log = 2 * Math.Log(3600 * n1);
            double gr = Math.Sqrt(log) + 0.577 / (Math.Sqrt(log));
            
            #region gr
            ICalcLogEntry grEntry = new CalcLogEntry();
            grEntry.ValueName = "gr";
            grEntry.AddDependencyValue("n1", Math.Round(n1, 3));
            grEntry.Reference = "";
            grEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexiblegv.docx";
            grEntry.FormulaID = null; //reference to formula from code
            grEntry.VariableValue = Math.Round(gr, 3).ToString();
            #endregion
            this.AddToLog(grEntry);
            
            return gr;
        }

        public  double GetBackgroundResponseFactorQ()
        {
            double B = this.width;
            double h = this.height;
            double Q = Math.Sqrt(1.0 / (1.0 + 0.63 * Math.Pow((B + h) / Lz, 0.63)));

            
            #region Q
            ICalcLogEntry QEntry = new CalcLogEntry();
            QEntry.ValueName = "Q";
            QEntry.AddDependencyValue("B", Math.Round(B, 3));
            QEntry.AddDependencyValue("h", Math.Round(h, 3));
            QEntry.AddDependencyValue("Lz", Math.Round(Lz, 3));
            QEntry.Reference = "";
            QEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/GustFactor/WindGustFactorFlexibleBackgroundResponseFactorQ.docx";
            QEntry.FormulaID = null; //reference to formula from code
            QEntry.VariableValue = Math.Round(Q, 3).ToString();
            #endregion
            this.AddToLog(QEntry);

            return Q;
        }


        
    }
}
