using SectionsEC.Common.Geometry;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SectionsEC.Drawing
{
    public abstract class DrawingBase
    {
        protected PerimeterProperties perimeterProperties;
        protected Grid canvas;

        public DrawingBase(Grid canvas, PerimeterProperties perimeterProperties)
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
}