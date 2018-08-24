using System.Collections.Generic;

namespace SectionsEC.Helpers
{
    public class CalculationResults
    {
        public IEnumerable<Reinforcement> Bars { get; set; } //reinforcement
        public IList<PointD> CompressionZone { get; set; } //coordinates of compression zone
        public double Cz { get; set; }//position of centre of gravity
        public double D { get; set; } //efective depth of section
        public double Ec { get; set; }  //max strain in concrete
        public double Es { get; set; }  //max strain in steel
        public double ForceConcrete { get; set; } //force in compression zone
        public double H { get; set; }//height of section
        public LoadCase LoadCase { get; set; } //load case
        public double Mrd { get; set; } //section capacity
        public double MrdConcrete { get; set; } //moment due to compression zone
        public double X { get; set; }   //depth of compression zone
    }
}