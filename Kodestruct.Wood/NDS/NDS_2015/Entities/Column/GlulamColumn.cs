using Kodestruct.Wood.NDS.Entities;
using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.NDS.NDS2015.GluLam;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS_2015.Entities.Column
{
    public class GlulamColumn
   {
        public double b { get; set; }
        public double d { get; set; }
        public int NumberLaminations { get; set; }
        public double l_e { get; set; }
        public double Temperature { get; set; }
        public MoistureCondition MoistureCondition { get; set; }
        GlulamSoftwoodAxialMember Member { get; set; }
        public bool IsPressureTreated { get; set; }

        

        public GlulamColumn (double b, double d, int NumberLaminations,
            GlulamSoftWoodAxialCombinationSymbol CombinationSymbol,
             double l_e, double Temperature, MoistureCondition MoistureCondition, bool IsPressureTreated)
        {
            this.b = b;
            this.d = d;
            this.NumberLaminations = NumberLaminations;
            this.l_e = l_e;
            this.Temperature = Temperature;
            this.MoistureCondition = MoistureCondition;
            this.IsPressureTreated = IsPressureTreated;
            this.Member = new GlulamSoftwoodAxialMember(b, d, NumberLaminations, CombinationSymbol);
        }

        public double GetCompressionStrength(LoadCombinationType LoadCombinationType)
        {
            double F_c = NumberLaminations<4 ? Member.Material.Fc_23 : Member.Material.F_c4;
            double E_min = Member.Material.E_min;
            var f = GetAdjustmentFactors(F_c, E_min, LoadCombinationType);

            //The beam stability factor, CL, shall not apply simultaneously with the volume factor, CV, for structural glued laminated timber bending
            //members.Therefore, the lesser of these adjustment factors shall apply.

            double F_c_prime = F_c * f.C_M_Fc * f.C_t_Fc * f.C_P * f.K_F * f.phi * f.lambda;
            double A = this.Member.Section.A;

            return F_c_prime * A;
        }
        AdjustmentFactors GetAdjustmentFactors(double F_c, double E_min, LoadCombinationType LoadCombinationType)
        {

            double C_M_Fc = MoistureCondition == MoistureCondition.Wet ? Member.GetWetServiceFactor(ReferenceDesignValueType.CompresionParallelToGrain) : 1.0;
            double C_M_E = MoistureCondition == MoistureCondition.Wet ? Member.GetWetServiceFactor(ReferenceDesignValueType.ModulusOfElasticity) : 1.0;
            double C_M_Emin = MoistureCondition == MoistureCondition.Wet ? Member.GetWetServiceFactor(ReferenceDesignValueType.ModulusOfElasticityMin) : 1.0;

            double C_t_Fc = Member.GetTemperatureFactorCt(ReferenceDesignValueType.CompresionParallelToGrain, Temperature, MoistureCondition);
            double C_t_E = Member.GetTemperatureFactorCt(ReferenceDesignValueType.ModulusOfElasticity, Temperature, MoistureCondition);
            double C_t_Emin = Member.GetTemperatureFactorCt(ReferenceDesignValueType.ModulusOfElasticityMin, Temperature, MoistureCondition);


            double lambda = Member.GetTimeEffectFactor(LoadCombinationType, false, IsPressureTreated);
            double phi = Member.GetStrengthReductionFactor_phi(ReferenceDesignValueType.Bending); //Table N.3.2
            double K_F = Member.GetFormatConversionFactor_K_F(ReferenceDesignValueType.Bending);

            double F_c_Star = Member.Get_FcStar(F_c, C_M_Fc, C_t_Fc, lambda);
            double E_min_prime = Member.GetAdjustedMinimumModulusOfElasticityForStability(E_min, C_M_E, C_t_E);
            double C_P_strong = Member.GetColumnStabilityFactor_C_P(F_c_Star, E_min_prime, l_e, d);
            double C_P_weak = Member.GetColumnStabilityFactor_C_P(F_c_Star, E_min_prime, l_e, b);

            return new AdjustmentFactors()
            {
                C_M_Fc = C_M_Fc,
                C_M_E = C_M_E,
                C_M_Emin = C_M_Emin,
                C_t_Fc = C_t_Fc,
                C_t_E = C_t_E,
                C_t_Emin = C_t_Emin,
                C_P = Math.Min(C_P_strong, C_P_weak),
                lambda = lambda,
                phi = phi,
                K_F = K_F
            };

        }

    }

    class AdjustmentFactors
    {
        public double C_M_Fc { get; set; }
        public double C_M_E { get; set; }
        public double C_M_Emin { get; set; }
        public double C_t_Fc { get; set; }
        public double C_t_E { get; set; }
        public double C_t_Emin { get; set; }
        public double C_P { get; set; }
        public double lambda { get; set; }
        public double phi { get; set; }
        public double K_F { get; set; }
    }
}
