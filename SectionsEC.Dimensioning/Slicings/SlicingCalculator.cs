using SectionsEC.Calculations.Extensions;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.SectionProperties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SectionsEC.Dimensioning.Slicing
{
    internal class SlicingCalculator
    {
        public SectionSlice GetSlice(IList<PointD> section, double upperY, double lowerY)
        {
            var lowerCoordinates = this.lowerSection(section, lowerY);
            var upperCoordinates = this.UpperSection(lowerCoordinates, upperY);
            var sectionSlice = this.calculateProperties(upperCoordinates);
            return sectionSlice;
        }

        private SectionSlice calculateProperties(IList<PointD> coordinates)
        {
            var slice = new SectionSlice();
            slice.Area = SectionPropertiesCalculator.Area(coordinates);
            slice.CentreOfGravityY = SectionPropertiesCalculator.CenterElevation(coordinates);
            return slice;
        }

        private IList<PointD> lowerSection(IList<PointD> section, double a)
        {
            var compressedSection = new System.Collections.Generic.List<PointD>();

            for (int i = 0; i <= section.Count - 2; i++)
            {
                var pointA = section[i];
                var pointB = section[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y >= a) && (pointB.Y >= a))
                    {
                        compressedSection.Add(pointA);
                        compressedSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = this.IntersectionPoint(pointA, pointB, a);
                    if (this.IsPointInsideSection(pointA, pointB, pointPP))
                    {
                        if (pointA.Y > pointPP.Y)
                        {
                            compressedSection.Add(pointA);
                            compressedSection.Add(pointPP);
                        }
                        else
                        {
                            compressedSection.Add(pointPP);
                            compressedSection.Add(pointB);
                        }
                    }
                    else
                    {
                        if ((pointA.Y >= a) && (pointB.Y >= a))
                        {
                            compressedSection.Add(pointA);
                            compressedSection.Add(pointB);
                        }
                    }
                }
            }
            CheckSection(compressedSection);

            return compressedSection;
        }

        private static void CheckSection(IList<PointD> parabolicSection)
        {
            var firstPoint = parabolicSection.FirstOrDefault();
            var lastPoint = parabolicSection.LastOrDefault();

            if (parabolicSection.Count > 0
                && !firstPoint.Equals(lastPoint))
            {
                parabolicSection.Add(firstPoint.Clone());
            }
        }

        private List<PointD> UpperSection(IList<PointD> compressedSection, double a)
        {
            var parabolicSection = new System.Collections.Generic.List<PointD>();

            for (int i = 0; i <= compressedSection.Count - 2; i++)
            {
                var pointA = compressedSection[i];
                var pointB = compressedSection[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y <= a) && (pointB.Y <= a))
                    {
                        parabolicSection.Add(pointA);
                        parabolicSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = this.IntersectionPoint(pointA, pointB, a);
                    if (this.IsPointInsideSection(pointA, pointB, pointPP))
                    {
                        if (pointA.Y > pointPP.Y)
                        {
                            parabolicSection.Add(pointPP);
                            parabolicSection.Add(pointB);
                        }
                        else
                        {
                            parabolicSection.Add(pointA);
                            parabolicSection.Add(pointPP);
                        }
                    }
                    else
                    {
                        if ((pointA.Y <= a) && (pointB.Y <= a))
                        {
                            parabolicSection.Add(pointA);
                            parabolicSection.Add(pointB);
                        }
                    }
                }
            }
            CheckSection(parabolicSection);
            return parabolicSection;
        }

        private PointD IntersectionPoint(PointD a1, PointD a2, double a)
        {
            var xa = a1.X;
            var xb = a2.X;
            var ya = a1.Y;
            var yb = a2.Y;
            PointD P = new PointD
            {
                Y = a,
                X = ((a - ya) * (xb - xa)) / (yb - ya) + xa
            };
            return P;
        }

        private bool IsPointInsideSection(PointD A, PointD B, PointD P)
        => ((Math.Min(A.X, B.X) <= P.X) && (P.X <= Math.Max(A.X, B.X)) && (Math.Min(A.Y, B.Y) <= P.Y)) && (P.Y <= Math.Max(A.Y, B.Y));
    }
}