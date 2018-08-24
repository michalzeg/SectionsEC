using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SectionsEC.Drawing;
using SectionsEC.Helpers;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.WindowClasses;

namespace SectionsEC.Views
{
    /// <summary>
    /// Interaction logic for DrawingPage.xaml
    /// </summary>
    public partial class DrawingPage : UserControl
    {
        private PerimeterProperties perimeterProperties;
        private SectionDrawing sectionDrawing;
        private BarsDrawing barsDrawing;
        private HatchDrawing compressionZoneDrawing;

        public DrawingPage()
        {
            InitializeComponent();
            Messenger.Default.Register<IList<PointD>>(this, updatePerimeter);
            Messenger.Default.Register<IList<Bar>>(this, updateBars);
            Messenger.Default.Register<IList<PointD>>(this, MessangerTokens.CompressionZoneDrawing, updateCompressionZone);
            createCanvas();
        }

        private void createCanvas()
        {
            this.perimeterProperties = new PerimeterProperties(() => canvas.ActualWidth, () => canvas.ActualHeight);
            this.sectionDrawing = new SectionDrawing(this.canvas, this.perimeterProperties);
            this.barsDrawing = new BarsDrawing(this.canvas, this.perimeterProperties);
            this.compressionZoneDrawing = new HatchDrawing(this.canvas, this.perimeterProperties);
        }

        private void updatePerimeter(IList<PointD> perimeter)
        {
            sectionDrawing.Perimeter(perimeter);
        }

        private void updateBars(IList<Bar> bars)
        {
            barsDrawing.Bars(bars);
        }

        private void updateCompressionZone(IList<PointD> compressionZone)
        {
            compressionZoneDrawing.Perimeter(compressionZone);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.perimeterProperties.UpdateProperties();
            this.sectionDrawing.Redraw();
            this.barsDrawing.Redraw();
            this.compressionZoneDrawing.Redraw();
        }
    }
}