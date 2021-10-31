using Kodestruct.Wood.NDS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public partial class GlulamMember : WoodMember
    {

        public double GetWetServiceFactor(ReferenceDesignValueType ValueType)
        {
            switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:
                    return 0.8;
                case ReferenceDesignValueType.TensionParallelToGrain:
                    return 0.8;
                case ReferenceDesignValueType.ShearParallelToGrain:
                    return 0.875;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain:
                    return 0.53;
                case ReferenceDesignValueType.CompresionParallelToGrain:
                    return 0.73;
                case ReferenceDesignValueType.ModulusOfElasticity:
                    return 0.833;
                case ReferenceDesignValueType.ModulusOfElasticityMin:
                    return 0.833;
                default:
                    return 0.8;
            }
 
        }
    }
}
