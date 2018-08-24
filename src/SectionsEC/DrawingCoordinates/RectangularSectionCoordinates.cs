using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.WindowClasses
{
    public class RectangularSectionCoordinates
    {
        public static IList<PointD> CalculateSectionCoordinates(double b, double h)
        {
            IList<PointD> coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(0.5 * b, 0),
                new PointD(0.5 * b, -h),
                new PointD(-b / 2, -h),
                new PointD(-b / 2, 0),
                new PointD(0, 0)
            };
            return coordinates;
        }

        public static IList<Bar> CalculateReinforcementCoordinates(double b, double h, double topBarsDiameter, double bottomBarsDiameter, long topBarsNumber, long bottomBarsNumber, double cover)
        {
            double distanceBetweenBars = (b - 2 * cover - topBarsDiameter) / (topBarsNumber + 1);
            IList<Bar> bars = new List<Bar>();
            for (int i = 1; i <= topBarsNumber; i++)
            {
                var x = i * distanceBetweenBars - (b / 2 - cover - topBarsDiameter / 2);
                var y = -cover - topBarsDiameter / 2;
                var area = Math.PI * topBarsDiameter * topBarsDiameter / 4;
                var bar = new Bar
                {
                    X = x,
                    Y = y,
                    Area = area
                };
                bars.Add(bar);
            }

            distanceBetweenBars = (b - 2 * cover - bottomBarsDiameter) / (bottomBarsNumber + 1);

            for (int i = 1; i <= bottomBarsNumber; i++)
            {
                var x = i * distanceBetweenBars - (b / 2 - cover - bottomBarsDiameter / 2);
                var y = -h + cover + bottomBarsDiameter / 2;
                var area = Math.PI * bottomBarsDiameter * bottomBarsDiameter / 4;
                var bar = new Bar
                {
                    X = x,
                    Y = y,
                    Area = area
                };
                bars.Add(bar);
            }
            return bars;
        }
    }
}