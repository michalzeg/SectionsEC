using SectionsEC.Common.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SectionsEC.Drawing
{
    public class PerimeterProperties
    {
        public double Scale { get; private set; }
        public PointD Centre { get; private set; }

        private IList<IList<PointD>> perimeters;
        private readonly Func<double> actualWidth;
        private readonly Func<double> actualHeight;

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
}