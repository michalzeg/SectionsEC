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
        public PointD() { }
        public double X { get; set; }

        public double Y { get; set; }

        public bool Equals(PointD other)
        {
            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;


            //Check whether the products' properties are equal. 
            return X.IsApproximatelyEqualTo(other.X) && Y.IsApproximatelyEqualTo(other.Y);
        }
        public override int GetHashCode()
        {
            int hashY = X.GetHashCode();

            //Get hash code for the Code field. 
            int hashValue = Y.GetHashCode();

            //Calculate the hash code for the product. 
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
            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;



            //Check whether the products' properties are equal. 
            return X.IsApproximatelyEqualTo(other.X) && Y.IsApproximatelyEqualTo(other.Y) && As.IsApproximatelyEqualTo(other.As);
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
        public string Name { get; set; }
        public double NormalForce { get; set; }
        public LoadCase()
        {
            Name = string.Empty;
            NormalForce = 0d;
        }

        public bool Equals(LoadCase other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;


            return ((Name == other.Name) && NormalForce.IsApproximatelyEqualTo(other.NormalForce));
        }
        public override int GetHashCode()
        {

            int hashName = Name.GetHashCode();

            //Get hash code for the Code field. 
            int hashNormalForce = NormalForce.GetHashCode();

            //Calculate the hash code for the product. 
            return hashName ^ hashNormalForce;
        }
    }
    public class CalculationResults
    {
        //opisuje wszystkie wyniki
        public double Mrd { get; set; } //nosnosc
        public double X { get; set; } //zasieg strefy sciskanej
        public double Ec { get; set; } //odksztalcenie w betonie
        public double Es { get; set; } //odksztalcenie w stali
        public IList<PointD> CompressionZone { get; set; }
        public double D { get; set; } //wysokosc uzyteczna przekroju
        public double MrdConcrete { get; set; }//nosnosc ze wzgledu na beton
        public double ForceConcrete { get; set; }// sila w betonie
        public IEnumerable<Reinforcement> Bars { get; set; } //wyniki dla zbrojenia
        public double H { get; set; }
        public double Cz { get; set; }
    }
    public class Reinforcement
    {
        public double E { get; set; } //odkształcenie w zbrojeniu
        public Bar Bar { get; set; }
        public double D { get; set; } //odległość zbrojenia od krawędzi najbardziej ściskanej (wysokość użytkowa dla danego pręta)
        public double My { get; set; } //moment od zbrojeni
        public bool IsCompressed { get; set; }//okresla czy zbrojenie jest sciskane czy rozciagane
    }
    public class Section : IIntegrable
    {
        public IList<PointD> Coordinates { get; private set; } //wspolrzedne przekroju
        public double D { get; set; }      //wysokosc uzyteczna przekroju
        public double MaxY { get; private set; } //najwieksza wspolrzedna y
        public double MinY { get; private set; } // najmniejsza wsplrzedna y
        public double H { get; private set; } //wysokosc przekroju
        public double B { get; private set; }//szerokosc przekroju;
        public double Cz { get; private set; }// odleglosc srodka ciezkosci od najbardziej sciskanego wlokna
        public double IntegrationPointY { get; set; }

        public Section(IList<PointD> coordinates)
        {
            Coordinates = checkIfCoordinatesAreClockwise(coordinates);

            calculateExtrementsAndDepth();
            Cz = SectionProperties.Cz(Coordinates, MaxY);
            IntegrationPointY = MinY;
        }

        private IList<PointD> checkIfCoordinatesAreClockwise(IList<PointD> coordinates) //procedura sprawdza czy wspolrzedne przekroju sa wprowadzone zgodnie ze wskazowkami zegara
        {
            //procedura bierze dwa pierwsze punkty i liczy iloczyn wektorowy. Jezeli wynik jest dodatni(wspolrzedna "z" to układ jest prawoskretny
            // co oznacza ze wspolrzedne wprowadzone sa przeciwnie do ruchu wskazowek zegara
            //jesli iloczyn wektorowy jest rowny 0 tzn ze wektory sa rownolegle ->nalezy wziasc kolejny punkt
            double iw; //iloczyn wektorowy
            IList<PointD> result = coordinates;

            for (int i = 0; i <= coordinates.Count - 3; i++)
            {
                iw = crossProduct(coordinates[i], coordinates[i + 1], coordinates[i + 2]);
                if (iw > 0)
                {
                    //uklad prawoskretny
                    break;
                }
                else if (iw < 0)
                {
                    //uklad lewoskretny, nalezy odwrocic wspolrzedne
                    result = coordinates.Reverse().ToList();
                    break;
                }
                else
                {
                    //dwa rownolegle wektory, nie rob nic, wez kolejne punkty
                }
            }

            return result;
        }

        private double crossProduct(PointD p0, PointD p1, PointD p2)//funckja oblicza iloczyn wektorowy
        {
            double[] vector1 = new double[2]; //wspolrzedne wektora 1 (0 - X, 1 - y) 
            double[] vector2 = new double[2];//wspolrzedne wektora 2

            vector1[0] = p1.X - p0.X;
            vector1[1] = p1.Y - p0.Y;
            vector2[0] = p2.X - p1.X;
            vector2[1] = p2.Y - p1.Y;

            double wynik; //ax*by-ay*bz
            wynik = vector1[0] * vector2[1] - vector1[1] * vector2[0];
            return wynik;
        }
        private void calculateExtrementsAndDepth() //procedura wyznacza maksymalne wartosci wspolrzdnej y oraz wysokosc przekroju
        {
            /*double[] taby = new double[Coordinates.Count];//pomocnicza tablica
            double[] tabx = new double[Coordinates.Count];
            for (int i = 0; i < Coordinates.Count; i++)
            {
                taby[i] = Coordinates[i].Y;
                tabx[i] = Coordinates[i].X;
            }*/
            MinY = this.Coordinates.Min(p => p.Y);
            MaxY = this.Coordinates.Max(p => p.Y);
            H = MaxY - MinY; //wysokosc
        }
    }
    public class InteractionCurveResult
    {
        public double Mx { get; set; }
        public double My { get; set; }
    }
}
