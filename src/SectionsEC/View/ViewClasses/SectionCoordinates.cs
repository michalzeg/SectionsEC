using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.WindowClasses
{
    public class CircularSectionCoordinates
    {
        public static IList<PointD> CalculateSectionCoordinates(double diameter, double cover)
        {
            var coordinates = new List<PointD>();
            for (int i = 0; i <= 360; i++)
            {
                double alfa;
                PointD point = new PointD();
                alfa = (i - 90) * Math.PI / 180;
                point.X = diameter / 2 * Math.Sin(alfa);
                point.Y = diameter / 2 * Math.Cos(alfa);
                coordinates.Add(point);
            }
            return coordinates;
        }

        public static IList<Bar> CalculateReinforcementCoordinates(double diameter, double cover, double fi, double n)
        {
            double deltaAlfa = 360 / n;
            var bars = new List<Bar>();

            for (int i = 1; i <= n; i++)
            {
                double alfa = (-180 + (i - 1) * deltaAlfa) * Math.PI / 180; //dla i =1 kat jest rowny zero
                //jeden pret zawsze sie znajduje na dole przekroju
                Bar bar = new Bar();
                bar.X = (diameter / 2 - cover - fi / 2) * Math.Sin(alfa);
                bar.Y = (diameter / 2 - cover - fi / 2) * Math.Cos(alfa);
                bar.As = Math.PI * fi * fi / 4;//pole przekroju zbrojenia
                bars.Add(bar);
            }
            return bars;
        }
    }

    public class TSectionCoordinates
    {
        public static IList<PointD> CalculateSectionCoordinates(double bf, double bw, double hf, double hw)
        {
            List<PointD> coordinates = new List<PointD>();

            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(0.5 * bf, 0));
            coordinates.Add(new PointD(0.5 * bf, -hf));
            coordinates.Add(new PointD(bw / 2, -hf));
            coordinates.Add(new PointD(bw / 2, -hf - hw));
            coordinates.Add(new PointD(-bw / 2, -hf - hw));
            coordinates.Add(new PointD(-bw / 2, -hf));
            coordinates.Add(new PointD(-0.5 * bf, -hf));
            coordinates.Add(new PointD(-bf / 2, 0));
            coordinates.Add(new PointD(0, 0));
            return coordinates;
        }

        public static IList<Bar> CalculateReinforcementCoordinates(double bf, double bw, double hf, double hw, double topBarsDiameter, double bottomBarsDiameter, long topBarsNumber, long bottomBarsNumber, double cover)
        {
            double distanceBetweenBars = (bf - 2 * cover - topBarsDiameter) / (topBarsNumber + 1);
            IList<Bar> bars = new List<Bar>();
            Reinforcement tempReinf = new Reinforcement();
            for (int i = 1; i <= topBarsNumber; i++)
            {
                double x = i * distanceBetweenBars - (bf / 2 - cover - topBarsDiameter / 2);
                double y = -cover - topBarsDiameter / 2;
                double As = Math.PI * topBarsDiameter * topBarsDiameter / 4;
                var bar = new Bar();
                bar.X = x;
                bar.Y = y;
                bar.As = As;
                bars.Add(bar);
            }

            distanceBetweenBars = (bw - 2 * cover - bottomBarsDiameter) / (bottomBarsNumber + 1);

            for (int i = 1; i <= bottomBarsNumber; i++)
            {
                double x = i * distanceBetweenBars - (bw / 2 - cover - bottomBarsDiameter / 2);
                double y = -hw - hf + cover + bottomBarsDiameter / 2;
                double As = Math.PI * bottomBarsDiameter * bottomBarsDiameter / 4;
                var bar = new Bar();
                bar.X = x;
                bar.Y = y;
                bar.As = As;
                bars.Add(bar);
            }
            return bars;
        }
    }

    public class RectangularSectionCoordinates
    {
        public static IList<PointD> CalculateSectionCoordinates(double b, double h)
        {
            IList<PointD> coordinates = new List<PointD>();

            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(0.5 * b, 0));
            coordinates.Add(new PointD(0.5 * b, -h));
            coordinates.Add(new PointD(-b / 2, -h));
            coordinates.Add(new PointD(-b / 2, 0));
            coordinates.Add(new PointD(0, 0));
            return coordinates;
        }

        public static IList<Bar> CalculateReinforcementCoordinates(double b, double h, double topBarsDiameter, double bottomBarsDiameter, long topBarsNumber, long bottomBarsNumber, double cover)
        {
            double distanceBetweenBars = (b - 2 * cover - topBarsDiameter) / (topBarsNumber + 1);
            IList<Bar> bars = new List<Bar>();
            Reinforcement tempReinf = new Reinforcement();
            for (int i = 1; i <= topBarsNumber; i++)
            {
                double x = i * distanceBetweenBars - (b / 2 - cover - topBarsDiameter / 2);
                double y = -cover - topBarsDiameter / 2;
                double As = Math.PI * topBarsDiameter * topBarsDiameter / 4;
                var bar = new Bar();
                bar.X = x;
                bar.Y = y;
                bar.As = As;
                bars.Add(bar);
            }

            distanceBetweenBars = (b - 2 * cover - bottomBarsDiameter) / (bottomBarsNumber + 1);

            for (int i = 1; i <= bottomBarsNumber; i++)
            {
                double x = i * distanceBetweenBars - (b / 2 - cover - bottomBarsDiameter / 2);
                double y = -h + cover + bottomBarsDiameter / 2;
                double As = Math.PI * bottomBarsDiameter * bottomBarsDiameter / 4;
                var bar = new Bar();
                bar.X = x;
                bar.Y = y;
                bar.As = As;
                bars.Add(bar);
            }
            return bars;
        }
    }
}