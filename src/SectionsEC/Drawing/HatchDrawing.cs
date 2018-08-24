using System.Windows.Controls;
using System.Windows.Media;

namespace SectionsEC.Drawing
{
    public class HatchDrawing : SectionDrawing
    {
        private const int thickness = 1;

        public HatchDrawing(Grid canvas, PerimeterProperties perimeterProperties)
            : base(canvas, perimeterProperties)
        { }

        protected override void setPolygonProperties()
        {
            polygon.Stroke = Brushes.Transparent;

            polygon.Fill = Hatch.GetHatch();

            polygon.StrokeThickness = thickness;
        }
    }
}