using Kodestruct.Aluminum.AA.Entities.Material;
using Kodestruct.Common.Section.Interfaces;
using System;
namespace Kodestruct.Aluminum.AA.Entities.Section
{
    public interface IAluminumSection
    {
        IAluminumMaterial Material { get; }
        ISection Shape { get; }
    }
}
