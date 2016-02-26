using System;
using System.Linq;

using System.Collections.Generic;
using System.Drawing;
using SectionsEC.Dimensioning;
using SectionsEC.Helpers;

namespace CommonMethods
{
    
    static class SectionProperties
    {
        public static double Cz(IList<PointD> coordinates)
        {
            //function calculates the distance from the centere of the gravity to the most compressed fibre
            //maxy - the highest coordinate of the section
            //OS - outer section
            //IS - inner section
            double z = 0; //distance from top 
            double A = 0; //area of section
            double S = 0; //first moment of area of section
            double x1, x2, y1, y2; //auxiliary variables
            //calculation of first moment of area and area of outer section
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

            //coordinate of centre of gravity
            double zy = S / A;
             
            return zy;
        }
        public static double Cz(IList<PointD> OS, double maxy)
        {
            //function calculates the distance from the centere of the gravity to the most compressed fibre
            //maxy - the highest coordinate of the section
            //OS - outer section
            //IS - inner section
            double z = 0; //distance from top 
            double A = 0; //area of section
            double S = 0; //first moment of area of section
            double x1, x2, y1, y2; //auxiliary variables
            //calculation of first moment of area and area of outer section
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

            //coordinate of centre of gravity
            double zy = S / A;
            z = maxy - zy; //distance from centre of gravity of section to upper fibre
            return z;
        }
        public static double Cz(List<PointD> OS, List<PointD> IS, double maxy)
        {
            //maxy - the highest coordinate of the section
            //OS - outer section
            //IS - inner section
            double z = 0; //distance from top 
            double A = 0; //area of section
            double S = 0; //first moment of area of section
            double x1, x2, y1, y2; //auxiliary variables
            //calculation of first moment of area and area of outer section
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                A = A + (x1 - x2) * (y2 + y1);
                S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }

            //calculation of first moment of area and area of inner section
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

            //coordinate of centre of gravity
            double zy = S / A;
            z = maxy - zy; //distance from centre of gravity of section to upper fibre
            return z;
        }

        public static double Ix(List<PointD> OS, List<PointD> IS) //second moment of area
        {
            double Ix = 0; //moment of inertia about X axis (not principal axis)
            double x1, x2, y1, y2; //auxiliary variables
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                Ix = Ix + (x1 - x2) * (y1 * y1 * y1 + y1 * y1 * y2 + y1 * y2 * y2 + y2 * y2 * y2);

            }

            //calculation of first moment of area and area of inner section
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
        public static double Sx(List<PointD> OS) // first moment of area
        {
            double S = 0; ;//first moment of area of section
            double x1, x2, y1, y2; //auxiliary variables
            //calculation of first moment of area and area of outer section
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
        public static double Sx(List<PointD> OS, List<PointD> IS) // first moment of area
        {
            double S = 0; ;//first moment of area of section
            double x1, x2, y1, y2; //auxiliary variables
            //calculation of first moment of area and area of outer section
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }

            //calculation of first moment of area and area of inner section
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
        public static double A(IList<PointD> OS) //area
        {
            //maxy - the highest coordinate of the section

            double A = 0; //area of section

            double x1, x2, y1, y2; //auxiliary variables
            //calculation of first moment of area and area of outer section
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
        public static double A(List<PointD> OS, List<PointD> IS) //area
        {
            //maxy - the highest coordinate of the section

            double A = 0; //area of section

            double x1, x2, y1, y2; //auxiliary variables
            //calculation of first moment of area and area of outer section
            for (int i = 0; i <= OS.Count - 2; i++)
            {
                x1 = OS[i].X;
                x2 = OS[i + 1].X;
                y1 = OS[i].Y;
                y2 = OS[i + 1].Y;
                A = A + (x1 - x2) * (y2 + y1);
            }

            //calculation of first moment of area and area of inner section
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
        public static void yMaxAndMin(List<PointD> OS, out double ymax, out double ymin) //maximum and minimum y coordinate
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