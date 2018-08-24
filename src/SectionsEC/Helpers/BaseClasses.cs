using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SectionsEC.Extensions;
using SectionsEC.Dimensioning;
using CommonMethods;

namespace SectionsEC.Helpers
{
    public class PointD : IEquatable<PointD>
    {
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointD()
        {
        }

        public double X { get; set; }
        public double Y { get; set; }

        public bool Equals(PointD other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return X.IsApproximatelyEqualTo(other.X) && Y.IsApproximatelyEqualTo(other.Y);
        }

        public override int GetHashCode()
        {
            int hashY = X.GetHashCode();
            int hashValue = Y.GetHashCode();
            return hashY ^ hashValue;
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

    public class Bar : IEquatable<Bar>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double As { get; set; }

        public bool Equals(Bar other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return X.IsApproximatelyEqualTo(other.X) && Y.IsApproximatelyEqualTo(other.Y) && As.IsApproximatelyEqualTo(other.As);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var bar = obj as Bar;
            if (bar == null)
                return false;
            else
                return this.Equals(bar);
        }

        public override int GetHashCode()
        {
            int hashX = X.GetHashCode();
            int hashY = Y.GetHashCode();
            int hashAs = As.GetHashCode();
            return hashX ^ hashY ^ hashAs;
        }
    }

    public class LoadCase : IEquatable<LoadCase>
    {
        private static int ID;

        public string Name { get; set; }
        public double NormalForce { get; set; }
        public int Id { get; private set; }

        public LoadCase()
        {
            ID++;
            this.Id = ID;
            this.Name = string.Empty;
            this.NormalForce = 0d;
        }

        public bool Equals(LoadCase other)
        {
            if (other == null)
                return false;

            if (this.Name == other.Name
                && this.NormalForce.IsApproximatelyEqualTo(other.NormalForce)
                && this.Id == other.Id)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            int hashName = Name.GetHashCode();
            int hashNormalForce = NormalForce.GetHashCode();
            return hashName ^ hashNormalForce;
        }
    }

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

    public class Reinforcement
    {
        public double E { get; set; }
        public Bar Bar { get; set; }
        public double D { get; set; }
        public double My { get; set; }
        public bool IsCompressed { get; set; }
    }

    public class Section : IIntegrable
    {
        public IList<PointD> Coordinates { get; private set; }
        public double D { get; set; }
        public double MaxY { get; private set; }
        public double MinY { get; private set; }
        public double H { get; private set; }
        public double B { get; private set; }
        public double Cz { get; private set; }
        public double IntegrationPointY { get; set; }

        public Section(IList<PointD> coordinates)
        {
            Coordinates = checkIfCoordinatesAreClockwise(coordinates);
            calculateExtrementsAndDepth();
            Cz = SectionProperties.Cz(Coordinates, MaxY);
            IntegrationPointY = MinY;
        }

        private IList<PointD> checkIfCoordinatesAreClockwise(IList<PointD> coordinates)
        {
            double iw;
            var result = coordinates;
            for (int i = 0; i <= coordinates.Count - 3; i++)
            {
                iw = crossProduct(coordinates[i], coordinates[i + 1], coordinates[i + 2]);
                if (iw > 0)
                {
                    break;
                }
                else if (iw < 0)
                {
                    result = coordinates.Reverse().ToList();
                    break;
                }
                else
                {
                }
            }
            return result;
        }

        private double crossProduct(PointD p0, PointD p1, PointD p2)
        {
            var vector1 = new double[2];
            var vector2 = new double[2];
            vector1[0] = p1.X - p0.X;
            vector1[1] = p1.Y - p0.Y;
            vector2[0] = p2.X - p1.X;
            vector2[1] = p2.Y - p1.Y;
            double wynik;
            wynik = vector1[0] * vector2[1] - vector1[1] * vector2[0];
            return wynik;
        }

        private void calculateExtrementsAndDepth()
        {
            MinY = this.Coordinates.Min(p => p.Y);
            MaxY = this.Coordinates.Max(p => p.Y);
            H = MaxY - MinY;
        }
    }

    public class InteractionCurveResult
    {
        public double Mx { get; set; }
        public double My { get; set; }
    }
}