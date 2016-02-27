using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SectionsEC.Helpers
{
    public class PointD
    {
        private double x;
        private double y;
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public PointD() { }
        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }
        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
    }
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
        public double EukToEud { get; set; }
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
                return Euk * EukToEud;
            }
        }
    }
    public class Bar
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double As { get; set; }
    }

}
