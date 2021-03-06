﻿using System.Xml.Serialization;

namespace SectionsEC.Calculations.Materials
{
    public class Steel
    {
        [XmlElement]
        public string Grade { get; set; }

        [XmlElement]
        public double Fyk { get; set; }

        [XmlElement]
        public double GammaS { get; set; }

        [XmlElement]
        public double K { get; set; }

        [XmlElement]
        public double Es { get; set; }

        [XmlElement]
        public double Euk { get; set; }

        [XmlElement]
        public double EudToEuk { get; set; }

        public double Fyd
        {
            get
            {
                return Fyk / GammaS;
            }
        }

        public double Eud
        {
            get
            {
                return Euk * EudToEuk;
            }
        }
    }
}