using SectionsEC.Common.Geometry;
using SectionsEC.Common.LoadCases;
using SectionsEC.Common.Sections;
using System.Collections.Generic;

namespace SectionsEC.Common.Results
{
    public class CalculationResults
    {
        public IEnumerable<Reinforcement> Bars { get; set; }
        public IList<PointD> CompressionZone { get; set; }
        public double Cz { get; set; }
        public double D { get; set; }
        public double Ec { get; set; }
        public double Es { get; set; }
        public double ForceConcrete { get; set; }
        public double H { get; set; }
        public LoadCase LoadCase { get; set; }
        public double Mrd { get; set; }
        public double MrdConcrete { get; set; }
        public double X { get; set; }
    }
}