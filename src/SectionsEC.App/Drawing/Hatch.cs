using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace SectionsEC.Drawing
{
    public class Hatch
    {
        public static VisualBrush GetHatch()
        {
            var visualBrush = new VisualBrush();
            visualBrush.TileMode = TileMode.Tile;
            visualBrush.Viewport = new Rect(new Point(0, 0), new Point(15, 15));
            visualBrush.ViewportUnits = BrushMappingMode.Absolute;
            visualBrush.Viewbox = new Rect(new Point(0, 0), new Point(15, 15));
            visualBrush.ViewboxUnits = BrushMappingMode.Absolute;

            var grid = new Grid();
            grid.Background = Brushes.Transparent;

            grid.Children.Add(new Path() { Data = getLine(new Point(0, 0), new Point(15, 15)), Stroke = Brushes.Red });
            grid.Children.Add(new Path() { Data = getLine(new Point(0, 15), new Point(15, 0)), Stroke = Brushes.Red });

            visualBrush.Visual = grid;
            return visualBrush;
        }

        private static PathGeometry getLine(Point point1, Point point2)
        {
            var pathFigure = new PathFigure
            {
                StartPoint = point1
            };
            var lineSegment = new LineSegment
            {
                Point = point2
            };

            var pathSegmentCollection = new PathSegmentCollection
            {
                lineSegment
            };
            pathFigure.Segments = pathSegmentCollection;

            var pathFigureCollection = new PathFigureCollection
            {
                pathFigure
            };

            var geometry1 = new PathGeometry
            {
                Figures = pathFigureCollection
            };

            return geometry1;
        }
    }
}