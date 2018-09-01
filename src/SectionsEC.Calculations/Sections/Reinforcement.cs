namespace SectionsEC.Calculations.Sections
{
    public class Reinforcement
    {
        public double Epsilon { get; set; }
        public double Sigma { get; set; }
        public Bar Bar { get; set; }
        public double D { get; set; }
        public double Moment { get; set; }
        public double Force { get; set; }
        public bool IsCompressed { get; set; }
    }
}