using System.Xml.Serialization;

namespace SectionsEC.Calculations.Materials
{
    public class Concrete
    {
        [XmlElement]
        public string Grade { get; set; }

        [XmlElement]
        public double Fck { get; set; }

        [XmlElement]
        public double Acc { get; set; }

        [XmlElement]
        public double GammaC { get; set; }

        [XmlElement]
        public double N { get; set; }

        [XmlElement]
        public double Ec2 { get; set; }

        [XmlElement]
        public double Ecu2 { get; set; }

        public double Fcd
        {
            get
            {
                return Acc * Fck / GammaC;
            }
        }
    }
}