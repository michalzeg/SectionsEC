using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace SectionsEC.Drawing
{
    public class BarsDrawing : DrawingBase
    {
        private IList<Bar> bars;
        private IList<Circle> circles;

        public BarsDrawing(Grid canvas, PerimeterProperties perimeterProperties) : base(canvas, perimeterProperties)
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

        private void CreateCircles()
        {
            foreach (var bar in bars)
            {
                var circle = new Circle();
                var point = transformCoordinatesToCentreOfGrid(bar.X, bar.Y);
                SetCircleProperties(circle);
                circle.Diameter = GetDiameter(bar.Area);
                circle.X = point.X;
                circle.Y = point.Y;
                this.circles.Add(circle);
                canvas.Children.Add(circle);
            }
        }

        private double GetDiameter(double area)
        {
            return Math.Sqrt(4 * area / Math.PI) * this.perimeterProperties.Scale;
        }

        private void SetCircleProperties(Circle circle)
        {
            circle.Stroke = Brushes.Blue;
            circle.StrokeThickness = 2;
        }

        private void RemoveCircles()
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
            RemoveCircles();
            CreateCircles();
        }
    }
}