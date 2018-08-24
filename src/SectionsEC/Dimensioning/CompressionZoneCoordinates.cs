using SectionsEC.Extensions;
using System;
using System.Collections.Generic;
using SectionsEC.Helpers;

namespace SectionsEC.Dimensioning
{
    internal class CompressionZoneCoordinates
    {
        public static IList<PointD> CoordinatesOfCompressionZone(IList<PointD> section, double neutralAxisY)
        {
            IList<PointD> compressedSection = new System.Collections.Generic.List<PointD>();
            PointD A;
            PointD B;
            PointD PP;
            for (int i = 0; i <= section.Count - 2; i++)
            {
                A = section[i];
                B = section[i + 1];
                if ((A.Y - B.Y).IsApproximatelyEqualTo(0))
                {
                    if ((A.Y >= neutralAxisY) && (B.Y >= neutralAxisY))
                    {
                        compressedSection.Add(A);
                        compressedSection.Add(B);
                    }
                }
                else
                {
                    PP = intersectionPoint(A, B, neutralAxisY);
                    if (isPointInsideSection(A, B, PP))
                    {
                        if (A.Y > PP.Y)
                        {
                            compressedSection.Add(A);
                            compressedSection.Add(PP);
                        }
                        else
                        {
                            compressedSection.Add(PP);
                            compressedSection.Add(B);
                        }
                    }
                    else
                    {
                        if ((A.Y >= neutralAxisY) && (B.Y >= neutralAxisY))
                        {
                            compressedSection.Add(A);
                            compressedSection.Add(B);
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
            IList<PointD> linearSection = new List<PointD>();
            PointD A;
            PointD B;
            PointD PP;
            for (int i = 0; i <= compressedSection.Count - 2; i++)
            {
                A = compressedSection[i];
                B = compressedSection[i + 1];
                if ((A.Y - B.Y).IsApproximatelyEqualTo(0))
                {
                    if ((A.Y >= ec2Y) && (B.Y >= ec2Y))
                    {
                        linearSection.Add(A);
                        linearSection.Add(B);
                    }
                }
                else
                {
                    PP = intersectionPoint(A, B, ec2Y);
                    if (isPointInsideSection(A, B, PP))
                    {
                        if (A.Y > PP.Y)
                        {
                            linearSection.Add(A);
                            linearSection.Add(PP);
                        }
                        else
                        {
                            linearSection.Add(PP);
                            linearSection.Add(B);
                        }
                    }
                    else
                    {
                        if ((A.Y >= ec2Y) && (B.Y >= ec2Y))
                        {
                            linearSection.Add(A);
                            linearSection.Add(B);
                        }
                    }
                }
            }
            if (linearSection.Count > 0)
            {
                if (!((linearSection[0].X.IsApproximatelyEqualTo(linearSection[linearSection.Count - 1].X)) && (linearSection[0].Y.IsApproximatelyEqualTo(linearSection[linearSection.Count - 1].Y))))
                {
                    PointD P = new PointD();
                    P.X = linearSection[0].X;
                    P.Y = linearSection[0].Y;
                    linearSection.Add(P);
                }
            }
            return linearSection;
        }

        public static IList<PointD> CoordinatesOfParabolicSection(IList<PointD> compressedSection, double ec2Y)
        {
            IList<PointD> parabolicSection = new List<PointD>();
            PointD A;
            PointD B;
            PointD PP;
            for (int i = 0; i <= compressedSection.Count - 2; i++)
            {
                A = compressedSection[i];
                B = compressedSection[i + 1];
                if ((A.Y - B.Y).IsApproximatelyEqualTo(0))
                {
                    if ((A.Y <= ec2Y) && (B.Y <= ec2Y))
                    {
                        parabolicSection.Add(A);
                        parabolicSection.Add(B);
                    }
                }
                else
                {
                    PP = intersectionPoint(A, B, ec2Y);
                    if (isPointInsideSection(A, B, PP))
                    {
                        if (A.Y > PP.Y)
                        {
                            parabolicSection.Add(PP);
                            parabolicSection.Add(B);
                        }
                        else
                        {
                            parabolicSection.Add(A);
                            parabolicSection.Add(PP);
                        }
                    }
                    else
                    {
                        if ((A.Y <= ec2Y) && (B.Y <= ec2Y))
                        {
                            parabolicSection.Add(A);
                            parabolicSection.Add(B);
                        }
                    }
                }
            }
            if (parabolicSection.Count > 0)
            {
                if (!((parabolicSection[0].X.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].X)) && (parabolicSection[0].Y.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].Y))))
                {
                    PointD P = new PointD();
                    P.X = parabolicSection[0].X;
                    P.Y = parabolicSection[0].Y;
                    parabolicSection.Add(P);
                }
            }
            return parabolicSection;
        }

        private static PointD intersectionPoint(PointD a1, PointD a2, double a)
        {
            double xa, xb, ya, yb;
            xa = a1.X;
            xb = a2.X;
            ya = a1.Y;
            yb = a2.Y;
            PointD P = new PointD();
            P.Y = a;
            P.X = ((a - ya) * (xb - xa)) / (yb - ya) + xa;
            return P;
        }

        private static bool isPointInsideSection(PointD A, PointD B, PointD P)
        {
            /*double AB, AP, PB;
			AB = Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y,2));
			AP = Math.Sqrt(Math.Pow(A.X - P.X, 2) + Math.Pow(A.Y - P.Y, 2));
			PB = Math.Sqrt(Math.Pow(P.X - B.X, 2) + Math.Pow(P.Y - B.Y, 2));
			if (Math.Round(AB,6) == Math.Round(AP,6) + Math.Round(PB,6))
			{
				return true;
			}
			else
			{
				return false;
			}*/
            if (((Math.Min(A.X, B.X) <= P.X) && (P.X <= Math.Max(A.X, B.X)) && (Math.Min(A.Y, B.Y) <= P.Y)) && (P.Y <= Math.Max(A.Y, B.Y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}