using SectionsEC.Helpers;
using System;
using System.Collections.Generic;

namespace SectionsEC.WindowClasses
{
    public class CircularSectionCoordinates
    {
        public static IList<PointD> CalculateSectionCoordinates(double diameter, double cover)
        {
            var coordinates = new List<PointD>();
            for (int i = 0; i <= 360; i++)
            {
                var point = new PointD();
                var alfa = (i - 90) * Math.PI / 180;
                point.X = diameter / 2 * Math.Sin(alfa);
                point.Y = diameter / 2 * Math.Cos(alfa);
                coordinates.Add(point);
            }
            return coordinates;
        }

        public static IList<Bar> CalculateReinforcementCoordinates(double diameter, double cover, double fi, double n)
        {
            var deltaAlfa = 360 / n;
            var bars = new List<Bar>();

            for (int i = 1; i <= n; i++)
            {
                var alfa = (-180 + (i - 1) * deltaAlfa) * Math.PI / 180;

                var bar = new Bar
                {
                    X = (diameter / 2 - cover - fi / 2) * Math.Sin(alfa),
                    Y = (diameter / 2 - cover - fi / 2) * Math.Cos(alfa),
                    Area = Math.PI * fi * fi / 4
                };
                bars.Add(bar);
            }
            return bars;
        }
    }
}