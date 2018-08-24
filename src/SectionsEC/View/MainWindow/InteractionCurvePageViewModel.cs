using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using SectionsEC.Extensions;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SectionsEC.ViewModel
{
    public class InteractionCurvePageViewModel : ViewModelBase
    {
        public InteractionCurvePageViewModel()
        {
            var config = new SeriesConfiguration<InteractionCurveResult>();
            config.X(e => e.Mx);
            config.Y(e => e.My);
            Data = new SeriesCollection(config);

            interactionSerie = new LineSeries();

            Data.Add(interactionSerie);

            Messenger.Default.Register<IEnumerable<InteractionCurveResult>>(this, addChart);
        }

        private LineSeries interactionSerie;

        public SeriesCollection Data { get; private set; }

        private void addChart(IEnumerable<InteractionCurveResult> interactionCurve)
        {
            interactionSerie.Title = "Interaction Curve";
            interactionSerie.PointRadius = 0;
            interactionSerie.Fill = Brushes.Transparent;
            interactionSerie.Foreground = Brushes.Red;
            interactionSerie.Values = new ChartValues<InteractionCurveResult>();

            foreach (var interactionPoint in interactionCurve)
            {
                interactionSerie.Values.Add(new InteractionCurveResult() { Mx = interactionPoint.Mx, My = interactionPoint.My });
            }
        }
    }
}