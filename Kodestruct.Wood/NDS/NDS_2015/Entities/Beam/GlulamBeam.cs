using Kodestruct.Wood.NDS.Entities;
using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.NDS.NDS2015.GluLam;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS_2015.Entities.Beam
{
    public class GlulamBeam
    {
        public double b { get; set; }
        public double d { get; set; }
        public int NumberLaminations { get; set; }
        public GlulamSimpleFlexuralStressClass StressClass { get; set; }
        public GlulamWoodSpeciesSimple WoodSpecies { get; set; }
        public double l_e { get; set; }
        public double L { get; set; }
        public double Temperature { get; set; }
        public MoistureCondition MoistureCondition { get; set; }
        GlulamSoftwoodFlexuralMemberSimple  Member { get; set; }

        public bool IsPressureTreated { get; set; }

        public GlulamBeam(double b, double d, int NumberLaminations, GlulamSimpleFlexuralStressClass StressClass, GlulamWoodSpeciesSimple WoodSpecies,
            double l_e, double L, double Temperature,  MoistureCondition MoistureCondition, bool IsPressureTreated)
        {
            this.b = b;
            this.d = d;
            this.NumberLaminations = NumberLaminations;
            this.StressClass = StressClass;
            this.WoodSpecies = WoodSpecies;
            this.l_e = l_e;
            this.L = L;
            this.Temperature = Temperature;
            this.MoistureCondition = MoistureCondition;
            this.IsPressureTreated = IsPressureTreated;
            this.Member = new GlulamSoftwoodFlexuralMemberSimple(b, d, NumberLaminations, StressClass, WoodSpecies);
        }
        
        public double GetFlexuralStrengthMajorAxis(LoadCombinationType CombinationType)
        {
            double F_bx_plus = Member.Material.F_bx_p;
            var f = GetAdjustmentFactors(F_bx_plus, CombinationType, true);

            //The beam stability factor, CL, shall not apply simultaneously with the volume factor, CV, for structural glued laminated timber bending
            //members.Therefore, the lesser of these adjustment factors shall apply.

            double F_b_prime = F_bx_plus * f.C_M_Fb * f.C_t_Fb * Math.Min(f.C_L, f.C_v) * f.C_fu * f.C_c * f.C_I * f.K_F * f.phi * f.lambda;
            double S_x = this.Member.Section.S_xBot;

            return F_b_prime * S_x;
        }
        AdjustmentFactors GetAdjustmentFactors(double F_b, LoadCombinationType CombinationType, bool IsStrongAxis)
        {

            double C_M_Fb = MoistureCondition == MoistureCondition.Wet?  Member.GetWetServiceFactor(ReferenceDesignValueType.Bending) :1.0;
            double C_M_E = MoistureCondition == MoistureCondition.Wet ? Member.GetWetServiceFactor(ReferenceDesignValueType.ModulusOfElasticity):1.0;
            double C_M_Emin = MoistureCondition == MoistureCondition.Wet ? Member.GetWetServiceFactor(ReferenceDesignValueType.ModulusOfElasticityMin):1.0;

            double C_t_Fb = Member.GetTemperatureFactorCt(ReferenceDesignValueType.Bending, Temperature, MoistureCondition);
            double C_t_E = Member.GetTemperatureFactorCt(ReferenceDesignValueType.ModulusOfElasticity, Temperature, MoistureCondition);
            double C_t_Emin = Member.GetTemperatureFactorCt(ReferenceDesignValueType.ModulusOfElasticityMin, Temperature, MoistureCondition);

            double C_c = Member.GetCurvatureFactor_C_c();
            double C_I = Member.GetCurvatureFactor_C_I();

            double lambda = Member.GetTimeEffectFactor(CombinationType, false, IsPressureTreated);
            double phi = Member.GetStrengthReductionFactor_phi(ReferenceDesignValueType.Bending); //Table N.3.2
            double K_F = Member.GetFormatConversionFactor_K_F(ReferenceDesignValueType.Bending);

            bool IsSouthernPine = (WoodSpecies == GlulamWoodSpeciesSimple.DouglasFir || WoodSpecies == GlulamWoodSpeciesSimple.Other) ? false : true;
            double C_L = Member.GetStabilityFactorC_L(b, d, F_b, L, l_e, Member.Material.E_x_min, C_M_Fb, C_M_E, C_t_Fb, C_t_E, C_c, C_I, lambda, IsSouthernPine);
            double C_v = Member.GetVolumeFactor_C_v(d, L, IsSouthernPine);


            double C_fu = IsStrongAxis? 1.0: Member.GetFlatUseFactor();
            return new AdjustmentFactors()
            {
                C_M_Fb = C_M_Fb,
                C_M_E = C_M_E,
                C_M_Emin = C_M_Emin,
                C_t_Fb = C_t_Fb,
                C_t_E = C_t_E,
                C_t_Emin = C_t_Emin,
                C_c = C_c,
                C_I = C_I,
                C_L = C_L,
                C_v = C_v,
                C_fu = C_fu,
                lambda = lambda,
                phi = phi,
                K_F = K_F
            };

        }

        private double GetFlatUseFactor()
        {
            //placeholder
            return 1.0;
        }
    }

    class AdjustmentFactors
    {
        public double C_M_Fb { get; set; }
        public double C_M_E { get; set; }
        public double C_M_Emin { get; set; }
        public double C_t_Fb { get; set; }
        public double C_t_E { get; set; }
        public double C_t_Emin { get; set; }
        public double C_c { get; set; }
        public double C_I { get; set; }
        public double C_L  { get; set; }
        public double C_v  { get; set; }
        public double C_fu { get; set; }
        public double lambda { get; set; }
        public double phi { get; set; }
        public double K_F { get; set; }
    }
}
