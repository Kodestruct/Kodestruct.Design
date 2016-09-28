using System;
using Kodestruct.Steel.AISC.Steel.Entities;

namespace Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public interface IHssTransversePlateConnection
    {
        SteelLimitStateValue GetLocalCripplingAndYieldingStrengthOfHss();
        SteelLimitStateValue GetLocalPunchingStrengthOfPlate();
        SteelLimitStateValue GetLocalYieldingStrengthOfPlate();
    }
}
