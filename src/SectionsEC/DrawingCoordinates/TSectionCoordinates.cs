using SectionsEC.Common.Geometry;
using SectionsEC.Common.Sections;
using System;
using System.Collections.Generic;

namespace SectionsEC.WindowClasses
{
    public class TSectionCoordinates
    {
        public static IList<PointD> CalculateSectionCoordinates(double bf, double bw, double hf, double hw)
        {
            List<PointD> coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(0.5 * bf, 0),
                new PointD(0.5 * bf, -hf),
                new PointD(bw / 2, -hf),
                new PointD(bw / 2, -hf - hw),
                new PointD(-bw / 2, -hf - hw),
                new PointD(-bw / 2, -hf),
                new PointD(-0.5 * bf, -hf),
                new PointD(-bf / 2, 0),
                new PointD(0, 0)
            };
            return coordinates;
        }

        public static IList<Bar> CalculateReinforcementCoordinates(double bf, double bw, double hf, double hw, double topBarsDiameter, double bottomBarsDiameter, long topBarsNumber, long bottomBarsNumber, double cover)
        {
            var distanceBetweenBars = (bf - 2 * cover - topBarsDiameter) / (topBarsNumber + 1);
            var bars = new List<Bar>();

            for (int i = 1; i <= topBarsNumber; i++)
            {
                var x = i * distanceBetweenBars - (bf / 2 - cover - topBarsDiameter / 2);
                var y = -cover - topBarsDiameter / 2;
                var As = Math.PI * topBarsDiameter * topBarsDiameter / 4;
                var bar = new Bar
                {
                    X = x,
                    Y = y,
                    Area = As
                };
                bars.Add(bar);
            }

            distanceBetweenBars = (bw - 2 * cover - bottomBarsDiameter) / (bottomBarsNumber + 1);

            for (int i = 1; i <= bottomBarsNumber; i++)
            {
                var x = i * distanceBetweenBars - (bw / 2 - cover - bottomBarsDiameter / 2);
                var y = -hw - hf + cover + bottomBarsDiameter / 2;
                var As = Math.PI * bottomBarsDiameter * bottomBarsDiameter / 4;
                var bar = new Bar
                {
                    X = x,
                    Y = y,
                    Area = As
                };
                bars.Add(bar);
            }
            return bars;
        }
    }
}