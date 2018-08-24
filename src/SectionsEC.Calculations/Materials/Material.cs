using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SectionsEC.Calculations.Materials
{
    public class Material
    {
        public List<Concrete> Concrete { get; set; }
        public List<Steel> Steel { get; set; }
    }
}