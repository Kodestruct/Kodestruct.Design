using Kodestruct.Aluminum.AA.Entities.Material;
using Kodestruct.Common.Section.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.Entities.Section
{
    public class AluminumSection : IAluminumSection
    {

        private IAluminumMaterial material;

        public IAluminumMaterial Material
        {
            get { return material; }

        }

        public ISection Shape { get; set; }

        public AluminumSection(IAluminumMaterial Material, ISection Shape)
        {
            this.Shape = Shape;
            this.material = Material;
        }

    }
}
