using System;
using Kodestruct.Steel.AISC.Steel.Entities;
namespace Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public interface IHssCapPlateConnection
    {
        SteelLimitStateValue GetHssYieldingOrCrippling();
        double t_plCap { get; set; }
    }
}
