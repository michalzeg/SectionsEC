using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using SectionsEC.Dimensioning;
using SectionsEC.Helpers;

namespace CommonMethods
{
    internal static class SectionProperties
    {
        public static double CenterElevation(IList<PointD> coordinates)
        {
            double area = 0;
            double firstMomentOfArea = 0;

            for (int i = 0; i <= coordinates.Count - 2; i++)
            {
                var x1 = coordinates[i].X;
                var x2 = coordinates[i + 1].X;
                var y1 = coordinates[i].Y;
                var y2 = coordinates[i + 1].Y;
                area = area + (x1 - x2) * (y2 + y1);
                firstMomentOfArea = firstMomentOfArea + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            area = area / 2;
            firstMomentOfArea = firstMomentOfArea / 6;
            var result = firstMomentOfArea / area;
            return result;
        }

        public static double CenterElevation(IList<PointD> outerCoordinates, double maxy)
        {
            double area = 0;
            double firstMomentOfArea = 0;

            for (int i = 0; i <= outerCoordinates.Count - 2; i++)
            {
                var x1 = outerCoordinates[i].X;
                var x2 = outerCoordinates[i + 1].X;
                var y1 = outerCoordinates[i].Y;
                var y2 = outerCoordinates[i + 1].Y;
                area = area + (x1 - x2) * (y2 + y1);
                firstMomentOfArea = firstMomentOfArea + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            area = area / 2;
            firstMomentOfArea = firstMomentOfArea / 6;
            double center = firstMomentOfArea / area;
            var result = maxy - center;
            return result;
        }

        public static double CenterElevation(IList<PointD> outerCoordinates, IList<PointD> innerCoordinates, double maxy)
        {
            double area = 0;
            double firstMomentOfArea = 0;

            for (int i = 0; i <= outerCoordinates.Count - 2; i++)
            {
                var x1 = outerCoordinates[i].X;
                var x2 = outerCoordinates[i + 1].X;
                var y1 = outerCoordinates[i].Y;
                var y2 = outerCoordinates[i + 1].Y;
                area = area + (x1 - x2) * (y2 + y1);
                firstMomentOfArea = firstMomentOfArea + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            for (int i = 0; i <= innerCoordinates.Count - 2; i++)
            {
                var x1 = innerCoordinates[i].X;
                var x2 = innerCoordinates[i + 1].X;
                var y1 = innerCoordinates[i].Y;
                var y2 = innerCoordinates[i + 1].Y;
                area = area - (x1 - x2) * (y2 + y1);
                firstMomentOfArea = firstMomentOfArea - (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            area = area / 2;
            firstMomentOfArea = firstMomentOfArea / 6;
            double center = firstMomentOfArea / area;
            var result = maxy - center;
            return result;
        }

        public static double SecondMomentOfArea(IList<PointD> outerCoordinates, IList<PointD> innerCoordinates)
        {
            double result = 0;
            for (int i = 0; i <= outerCoordinates.Count - 2; i++)
            {
                var x1 = outerCoordinates[i].X;
                var x2 = outerCoordinates[i + 1].X;
                var y1 = outerCoordinates[i].Y;
                var y2 = outerCoordinates[i + 1].Y;
                result = result + (x1 - x2) * (y1 * y1 * y1 + y1 * y1 * y2 + y1 * y2 * y2 + y2 * y2 * y2);
            }
            for (int i = 0; i <= innerCoordinates.Count - 2; i++)
            {
                var x1 = innerCoordinates[i].X;
                var x2 = innerCoordinates[i + 1].X;
                var y1 = innerCoordinates[i].Y;
                var y2 = innerCoordinates[i + 1].Y;
                result = result - (x1 - x2) * (y1 * y1 * y1 + y1 * y1 * y2 + y1 * y2 * y2 + y2 * y2 * y2);
            }
            result = result / 12;
            return result;
        }

        public static double FirstMomentOfArea(IList<PointD> outerCoordinates)
        {
            double result = 0;

            for (int i = 0; i <= outerCoordinates.Count - 2; i++)
            {
                var x1 = outerCoordinates[i].X;
                var x2 = outerCoordinates[i + 1].X;
                var y1 = outerCoordinates[i].Y;
                var y2 = outerCoordinates[i + 1].Y;
                result = result + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            result = result / 6;
            return result;
        }

        public static double FirstMomentOfArea(IList<PointD> outerCoordinates, List<PointD> innerCoordinates)
        {
            double result = 0;

            for (int i = 0; i <= outerCoordinates.Count - 2; i++)
            {
                var x1 = outerCoordinates[i].X;
                var x2 = outerCoordinates[i + 1].X;
                var y1 = outerCoordinates[i].Y;
                var y2 = outerCoordinates[i + 1].Y;
                result = result + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            for (int i = 0; i <= innerCoordinates.Count - 2; i++)
            {
                var x1 = innerCoordinates[i].X;
                var x2 = innerCoordinates[i + 1].X;
                var y1 = innerCoordinates[i].Y;
                var y2 = innerCoordinates[i + 1].Y;
                result = result - (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            result = result / 6;
            return result;
        }

        public static double Area(IList<PointD> outerCoordinates)
        {
            double result = 0;

            for (int i = 0; i <= outerCoordinates.Count - 2; i++)
            {
                var x1 = outerCoordinates[i].X;
                var x2 = outerCoordinates[i + 1].X;
                var y1 = outerCoordinates[i].Y;
                var y2 = outerCoordinates[i + 1].Y;
                result = result + (x1 - x2) * (y2 + y1);
            }
            result = result / 2;
            return result;
        }

        public static double Area(IList<PointD> outerCoordinates, IList<PointD> innerCoordinates)
        {
            double result = 0;

            for (int i = 0; i <= outerCoordinates.Count - 2; i++)
            {
                var x1 = outerCoordinates[i].X;
                var x2 = outerCoordinates[i + 1].X;
                var y1 = outerCoordinates[i].Y;
                var y2 = outerCoordinates[i + 1].Y;
                result = result + (x1 - x2) * (y2 + y1);
            }
            for (int i = 0; i <= innerCoordinates.Count - 2; i++)
            {
                var x1 = innerCoordinates[i].X;
                var x2 = innerCoordinates[i + 1].X;
                var y1 = innerCoordinates[i].Y;
                var y2 = innerCoordinates[i + 1].Y;
                result = result - (x1 - x2) * (y2 + y1);
            }
            result = result / 2;
            return result;
        }
    }
}