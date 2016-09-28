using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;

namespace Kodestruct.Concrete.ACI.ACI318_14.C25_ReinforcementDetails.SpliceAndEmbedment.Compression
{
    public class CompressionLapSplice : AnalyticalElement
    {
        public CompressionLapSplice(IConcreteMaterial Concrete, Rebar Rebar, 
        ICalcLog log): base(log)
        {
            this.Concrete = Concrete;
            this.Rebar = Rebar;
        }

        IConcreteMaterial Concrete {get; set;}
        Rebar Rebar { get; set; }

        private double length;

        public double Length
        {
            get {
                length = GetCompressionSpliceLength();
                return length; }
        }

        private double GetCompressionSpliceLength()
        {
            double l_sc;
            double f_y = Rebar.Material.YieldStress;
            double d_b = Rebar.Diameter;
            double fc = Concrete.SpecifiedCompressiveStrength;


            // per ACI 318-14 section 25.5.5.1 
            if (f_y<=60000)
            {
                l_sc = 0.0005 * f_y * d_b;
            }
            else
            {
                l_sc = (0.0009*f_y-24.0)*d_b;
            }

                 if (l_sc<1.0)
	            {
		            l_sc = 12.0;
	            }

                    if (fc<3000)
	            {
                    l_sc = l_sc * 1.3334;
	            }

            return l_sc;
        }
        
    }
}
