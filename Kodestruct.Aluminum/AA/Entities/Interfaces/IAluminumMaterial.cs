using System;
namespace Kodestruct.Aluminum.AA.Entities.Material
{
    public interface IAluminumMaterial
    {
        double E { get; set; }
        double F_cy { get; set; }
        double F_su { get; set; }
        double F_sy { get; set; }
        double F_tu { get; set; }
        double F_tuw { get; set; }
        double F_ty { get; set; }
        double F_tyw { get; set; }
        bool IsWelded { get; set; }
        double k_t { get; set; }
        string Temper { get; set; }
    }
}
