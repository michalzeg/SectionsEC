using System.Collections.Generic;
using System.Linq;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.Interfaces;
using SectionsEC.Calculations.SectionProperties;

namespace SectionsEC.Calculations.Sections
{
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
            Cz = SectionPropertiesCalculator.CenterElevation(Coordinates, MaxY);
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
}