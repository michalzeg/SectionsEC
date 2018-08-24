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
        public static double Cz(IList<PointD> coordinates)
        {
            double A = 0;
            double S = 0;
            double x1, x2, y1, y2;
            for (int i = 0; i <= coordinates.Count - 2; i++)
            {
                x1 = coordinates[i].X;
                x2 = coordinates[i + 1].X;
                y1 = coordinates[i].Y;
                y2 = coordinates[i + 1].Y;
                A = A + (x1 - x2) * (y2 + y1);
                S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            A = A / 2;
            S = S / 6;
            double zy = S / A;
            return zy;
        }

        public static double Cz(IList<PointD> OS, double maxy)
        {
            double z = 0;
            double A = 0;
            double S = 0;
            double x1, x2, y1, y2;
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                A = A + (x1 - x2) * (y2 + y1);
                S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            A = A / 2;
            S = S / 6;
            double zy = S / A;
            z = maxy - zy;
            return z;
        }

        public static double Cz(List<PointD> OS, List<PointD> IS, double maxy)
        {
            double z = 0;
            double A = 0;
            double S = 0;
            double x1, x2, y1, y2;
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                A = A + (x1 - x2) * (y2 + y1);
                S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            for (int i = 0; i <= IS.Count - 2; i++)
            {
                x1 = IS[i].X;
                x2 = IS[i + 1].X;
                y1 = IS[i].Y;
                y2 = IS[i + 1].Y;
                A = A - (x1 - x2) * (y2 + y1);
                S = S - (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            A = A / 2;
            S = S / 6;
            double zy = S / A;
            z = maxy - zy;
            return z;
        }

        public static double Ix(List<PointD> OS, List<PointD> IS)
        {
            double Ix = 0;
            double x1, x2, y1, y2;
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                Ix = Ix + (x1 - x2) * (y1 * y1 * y1 + y1 * y1 * y2 + y1 * y2 * y2 + y2 * y2 * y2);
            }
            for (int i = 0; i <= IS.Count - 2; i++)
            {
                x1 = IS[i].X;
                x2 = IS[i + 1].X;
                y1 = IS[i].Y;
                y2 = IS[i + 1].Y;
                Ix = Ix - (x1 - x2) * (y1 * y1 * y1 + y1 * y1 * y2 + y1 * y2 * y2 + y2 * y2 * y2);
            }
            Ix = Ix / 12;
            return Ix;
        }

        public static double Sx(List<PointD> OS)
        {
            double S = 0; ;
            double x1, x2, y1, y2;
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            S = S / 6;
            return S;
        }

        public static double Sx(List<PointD> OS, List<PointD> IS)
        {
            double S = 0; ;
            double x1, x2, y1, y2;
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            for (int i = 0; i <= IS.Count - 2; i++)
            {
                x1 = IS[i].X;
                x2 = IS[i + 1].X;
                y1 = IS[i].Y;
                y2 = IS[i + 1].Y;
                S = S - (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            S = S / 6;
            return S;
        }

        public static double A(IList<PointD> OS)
        {
            double A = 0;
            double x1, x2, y1, y2;
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                A = A + (x1 - x2) * (y2 + y1);
            }
            A = A / 2;
            return A;
        }

        public static double A(List<PointD> OS, List<PointD> IS)
        {
            double A = 0;
            double x1, x2, y1, y2;
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                A = A + (x1 - x2) * (y2 + y1);
            }
            for (int i = 0; i <= IS.Count - 2; i++)
            {
                x1 = IS[i].X;
                x2 = IS[i + 1].X;
                y1 = IS[i].Y;
                y2 = IS[i + 1].Y;
                A = A - (x1 - x2) * (y2 + y1);
            }
            A = A / 2;
            return A;
        }

        public static void yMaxAndMin(List<PointD> OS, out double ymax, out double ymin)
        {
            int n = OS.Count;
            double[] tab = new double[n];
            for (int i = 0; i <= n - 1; i++)
            {
                tab[i] = OS[i].Y;
            }
            ymax = tab.Max();
            ymin = tab.Min();
        }
    }
}