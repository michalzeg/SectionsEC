﻿using System.Xml.Serialization;

namespace SectionsEC.Helpers
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
        public double GammaM { get; set; }

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
                return Acc * Fck / GammaM;
            }
        }
    }
}