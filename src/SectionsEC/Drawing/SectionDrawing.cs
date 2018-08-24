using SectionsEC.Helpers;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace SectionsEC.Drawing
{
    public class SectionDrawing : DrawingBase
    {
        protected IList<PointD> perimeter;
        protected Polygon polygon;

        public SectionDrawing(Grid canvas, PerimeterProperties perimeterProperties) : base(canvas, perimeterProperties)
        {
            this.polygon = new Polygon();
            this.canvas.Children.Add(polygon);
            setPolygonProperties();
        }

        protected virtual void setPolygonProperties()
        {
            polygon.Stroke = Brushes.DimGray;

            polygon.Fill = new LinearGradientBrush(Brushes.LightGray.Color, Brushes.Gray.Color, 90);

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
}