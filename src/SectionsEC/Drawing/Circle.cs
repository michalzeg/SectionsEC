using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace SectionsEC.Drawing
{
    public class Circle : Shape
    {
        public Circle()
        {
        }

        public double Diameter { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        private IEnumerable<PathFigure> CreatePolyline()
        {
            var basePoint = new Point(Diameter / 2, 0d);
            double deltaAngle = Math.PI / 4;

            var segments = new List<PathSegment>();
            for (int i = 1; i <= 8; i++)
            {
                var point = rotatePoint(basePoint, i * deltaAngle);
                point.X = point.X + X;
                point.Y = point.Y + Y;
                var lineSegment = new LineSegment(point, true);
                segments.Add(lineSegment);
            }
            var startPoint = new Point
            {
                X = X + Diameter / 2,
                Y = Y
            };
            var pathFigure = new PathFigure(startPoint, segments, false);

            var figures = new[] { pathFigure };
            return figures;
        }

        private Point rotatePoint(Point point, double angle)
        {
            var result = new Point();
            result.X = point.X * Math.Cos(angle) - point.Y * Math.Sin(angle);
            result.Y = point.X * Math.Sin(angle) + point.Y * Math.Cos(angle);
            return result;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return new PathGeometry(CreatePolyline());
            }
        }
    }
}