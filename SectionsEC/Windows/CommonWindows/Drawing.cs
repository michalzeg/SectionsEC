using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace SectionsEC.Drawing
{

    public class PerimeterProperties
    {
        public double Scale { get; private set; }
        public PointD Centre { get; private set; }

        private IList<IList<PointD>> perimeters;
        private Func<double> actualWidth;
        private Func<double> actualHeight;
  

        public PerimeterProperties(Func<double> actualWidth, Func<double> actualHeight)
        {
            this.perimeters = new List<IList<PointD>>();
            this.actualHeight = actualHeight;
            this.actualWidth = actualWidth;
        }


        public void AddPerimeter(IList<PointD> perimeter)
        {
            if (perimeter == null || perimeter.Count == 0)
                return;
            if (this.contains(perimeter))
                return;
            perimeters.Add(perimeter);
            UpdateProperties();
        }
        public void RemovePerimeter(IList<PointD> perimeter)
        {
            if (!this.contains(perimeter))
                return;
            perimeters.Remove(perimeter);
            UpdateProperties();
        }
        public void ChangePerimeter(IList<PointD> perimeter)
        {
            this.RemovePerimeter(perimeter);
            this.AddPerimeter(perimeter);
        }

        public void UpdateProperties()
        {
            if (perimeters == null || perimeters.Count == 0)
                return;

            var xMax = perimeters.Max(e => e.Max(g => g.X));
            var xMin = perimeters.Min(e => e.Min(g => g.X));
            var yMax = perimeters.Max(e => e.Max(g => g.Y));
            var yMin = perimeters.Min(e => e.Min(g => g.Y));

            var drawingWidth = xMax - xMin;
            var drawingHeight = yMax - yMin;

            this.Centre = new PointD(drawingWidth / 2 + xMin, drawingHeight / 2 + yMin);
            var scaleX = this.actualWidth() / drawingWidth;
            var scaleY = this.actualHeight() / drawingHeight;

            this.Scale = Math.Min(scaleX, scaleY);

        }

        private bool contains(IList<PointD> perimeter)
        {
            foreach (var p in perimeters)
            {
                if (p.OrderBy(e => e.X).SequenceEqual(perimeter.OrderBy(e => e.X)))
                    return true;
            }
            return false;
        }

    }

    public abstract class Drawing
    {

        protected PerimeterProperties perimeterProperties;
        protected Grid canvas;

        public Drawing(Grid canvas,PerimeterProperties perimeterProperties)
        {
            this.canvas = canvas;
            this.perimeterProperties = perimeterProperties;
        }

        protected IList<PointD> transformCoordinatesToCentreOfGrid(IList<PointD> coordinates)
        {

            var pointList = new List<PointD>();
            foreach (var point in coordinates)
            {
                var newPoint = transformCoordinatesToCentreOfGrid(point);

                //newPoint.X = ((point.X - this.perimeterProperties.Centre.X) * perimeterProperties.Scale) + this.canvas.ActualWidth / 2;
                //newPoint.Y = (-(point.Y - this.perimeterProperties.Centre.Y) * perimeterProperties.Scale) + this.canvas.ActualHeight / 2;
                pointList.Add(newPoint);
            }
            return pointList;
        }
        protected PointD transformCoordinatesToCentreOfGrid(PointD point)
        {
            var newPoint = new PointD();
            newPoint.X = ((point.X - this.perimeterProperties.Centre.X) * perimeterProperties.Scale) + this.canvas.ActualWidth / 2;
            newPoint.Y = (-(point.Y - this.perimeterProperties.Centre.Y) * perimeterProperties.Scale) + this.canvas.ActualHeight / 2;
            return newPoint;
        }
        protected PointD transformCoordinatesToCentreOfGrid(double x, double y)
        {
            return transformCoordinatesToCentreOfGrid(new PointD(x, y));
        }
        public abstract void Redraw();
        
    }

    public class SectionDrawing:Drawing
    {

        private IList<PointD> perimeter;
        private Polygon polygon;

        public SectionDrawing(Grid canvas,PerimeterProperties perimeterProperties):base(canvas,perimeterProperties)
        {
            this.polygon = new Polygon();
            this.canvas.Children.Add(polygon);
            polygon.Stroke = Brushes.Red;
            polygon.StrokeThickness = 2;
        }

        public void Perimeter(IList<PointD> perimeter)
        {
            if (perimeter == null || perimeter.Count == 0)
                return;
            this.perimeter = perimeter;
            this.perimeterProperties.ChangePerimeter(perimeter);
            this.Redraw();
        }

        public override void Redraw()
        {
            if (perimeter == null || perimeter.Count == 0)
                return;

            this.polygon.Points.Clear();
            var transferedCoordinates = base.transformCoordinatesToCentreOfGrid(this.perimeter);
            foreach (var point in transferedCoordinates)
            {
                this.polygon.Points.Add(new Point(point.X, point.Y));
            }

        }
    }

    public class BarsDrawing:Drawing
    {
        private IList<Bar> bars;
        private IList<Circle> circles;

        public BarsDrawing(Grid canvas,PerimeterProperties perimeterProperties):base(canvas,perimeterProperties)
        {
            this.circles = new List<Circle>();
            
        }

        public void Bars(IList<Bar> bars)
        {
            if (bars == null || bars.Count == 0)
                return;
            this.bars = bars;
            Redraw();
        }

        private void createCircles()
        {
            foreach (var bar in bars)
            {
                var circle = new Circle();
                var point = transformCoordinatesToCentreOfGrid(bar.X, bar.Y);
                setCircleProperties(circle);
                circle.Diameter = getDiameter(bar.As);
                circle.X = point.X;
                circle.Y = point.Y;
                this.circles.Add(circle);
                canvas.Children.Add(circle);
            }
        }

        private double getDiameter(double area)
        {
            return Math.Sqrt(4 * area / Math.PI) * this.perimeterProperties.Scale;
        }
        private void setCircleProperties(Circle circle)
        {
            circle.Stroke = Brushes.Blue;
            circle.StrokeThickness = 2;
        }

        private void removeCircles()
        {
            foreach (var circle in circles)
            {
                canvas.Children.Remove(circle);
            }
            circles.Clear();
        }

        public override void Redraw()
        {
            if (bars == null || bars.Count == 0)
                return;
            removeCircles();
            createCircles();
        }
    }

    public class Circle : Shape
    {
        public Circle()
        {
            //this.figures = new List<PathFigure>();
        }

        //private IList<PathFigure> figures;

        public double Diameter { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        private IEnumerable<PathFigure> createPolyline()
        {

            var basePoint = new Point(Diameter / 2, 0d);
            double deltaAngle = Math.PI / 4;

            var segments = new List<PathSegment>();
            for (int i = 1;i<=8;i++)
            {
                var point = rotatePoint(basePoint, i * deltaAngle);
                point.X = point.X + X;
                point.Y = point.Y + Y;
                var lineSegment = new LineSegment(point, true);
                segments.Add(lineSegment);
            }
            var startPoint = new Point();
            startPoint.X = X + Diameter / 2;
            startPoint.Y = Y;
            var pathFigure = new PathFigure(startPoint, segments, false);

            var figures = new List<PathFigure>();
            figures.Add(pathFigure);
            return figures;
        }
        private Point rotatePoint(Point point,double angle)
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

                return new PathGeometry(createPolyline());
            }
        }
    }
}
