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
                var newPoint = new PointD();

                newPoint.X = ((point.X - this.perimeterProperties.Centre.X) * perimeterProperties.Scale) + this.canvas.ActualWidth / 2;
                newPoint.Y = (-(point.Y - this.perimeterProperties.Centre.Y) * perimeterProperties.Scale) + this.canvas.ActualHeight / 2;
                pointList.Add(newPoint);
            }

            return pointList;

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
            this.perimeter = perimeter;
            this.perimeterProperties.ChangePerimeter(perimeter);
            this.Redraw();
        }

        public override void Redraw()
        {
            
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

        public BarsDrawing(Grid canvas,PerimeterProperties perimeterProperties):base(canvas,perimeterProperties)
        { }

        public override void Redraw()
        {
            throw new NotImplementedException();
        }
    }
}
