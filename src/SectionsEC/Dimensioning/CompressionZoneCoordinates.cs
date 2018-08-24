using SectionsEC.Extensions;
using System;
using System.Collections.Generic;
using SectionsEC.Helpers;
using System.Linq;

namespace SectionsEC.Dimensioning
{
    internal class CompressionZoneCoordinates
    {
        public static IList<PointD> CoordinatesOfCompressionZone(IList<PointD> section, double neutralAxisY)
        {
            IList<PointD> compressedSection = new List<PointD>();

            for (int i = 0; i <= section.Count - 2; i++)
            {
                var pointA = section[i];
                var pointB = section[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y >= neutralAxisY) && (pointB.Y >= neutralAxisY))
                    {
                        compressedSection.Add(pointA);
                        compressedSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = IntersectionPoint(pointA, pointB, neutralAxisY);
                    if (IsPointInsideSection(pointA, pointB, pointPP))
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
                        if ((pointA.Y >= neutralAxisY) && (pointB.Y >= neutralAxisY))
                        {
                            compressedSection.Add(pointA);
                            compressedSection.Add(pointB);
                        }
                    }
                }
            }
            if (!((compressedSection[0].X.IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].X)) && (compressedSection[0].Y.IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].Y))))
            {
                PointD P = new PointD();
                P.X = compressedSection[0].X;
                P.Y = compressedSection[0].Y;
                compressedSection.Add(P);
            }
            return compressedSection;
        }

        public static IList<PointD> CoordinatesOfLinearSection(IList<PointD> compressedSection, double ec2Y)
        {
            var linearSection = new List<PointD>();

            for (int i = 0; i <= compressedSection.Count - 2; i++)
            {
                var pointA = compressedSection[i];
                var pointB = compressedSection[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y >= ec2Y) && (pointB.Y >= ec2Y))
                    {
                        linearSection.Add(pointA);
                        linearSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = IntersectionPoint(pointA, pointB, ec2Y);
                    if (IsPointInsideSection(pointA, pointB, pointPP))
                    {
                        if (pointA.Y > pointPP.Y)
                        {
                            linearSection.Add(pointA);
                            linearSection.Add(pointPP);
                        }
                        else
                        {
                            linearSection.Add(pointPP);
                            linearSection.Add(pointB);
                        }
                    }
                    else
                    {
                        if ((pointA.Y >= ec2Y) && (pointB.Y >= ec2Y))
                        {
                            linearSection.Add(pointA);
                            linearSection.Add(pointB);
                        }
                    }
                }
            }
            CheckSection(linearSection);

            return linearSection;
        }

        public static IList<PointD> CoordinatesOfParabolicSection(IList<PointD> compressedSection, double ec2Y)
        {
            IList<PointD> parabolicSection = new List<PointD>();

            for (int i = 0; i <= compressedSection.Count - 2; i++)
            {
                var pointA = compressedSection[i];
                var pointB = compressedSection[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y <= ec2Y) && (pointB.Y <= ec2Y))
                    {
                        parabolicSection.Add(pointA);
                        parabolicSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = IntersectionPoint(pointA, pointB, ec2Y);
                    if (IsPointInsideSection(pointA, pointB, pointPP))
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
                        if ((pointA.Y <= ec2Y) && (pointB.Y <= ec2Y))
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

        private static PointD IntersectionPoint(PointD pointA, PointD pointB, double elevation)
        {
            var xa = pointA.X;
            var xb = pointB.X;
            var ya = pointA.Y;
            var yb = pointB.Y;
            PointD P = new PointD
            {
                Y = elevation,
                X = ((elevation - ya) * (xb - xa)) / (yb - ya) + xa
            };
            return P;
        }

        private static bool IsPointInsideSection(PointD pointA, PointD pointB, PointD pointP)
        => (((Math.Min(pointA.X, pointB.X) <= pointP.X) && (pointP.X <= Math.Max(pointA.X, pointB.X)) && (Math.Min(pointA.Y, pointB.Y) <= pointP.Y)) && (pointP.Y <= Math.Max(pointA.Y, pointB.Y)));
    }
}