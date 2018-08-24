using GalaSoft.MvvmLight;
using System;
using LiveCharts;
using SectionsEC.Common.Geometry;
using SectionsEC.Common.Extensions;

namespace SectionsEC.ViewModel
{
    public class ChartPageViewModel : ViewModelBase
    {
        public ChartPageViewModel()
        {
            var config = new SeriesConfiguration<PointD>();
            config.X(e => e.X);
            config.Y(e => e.Y);
            Data = new SeriesCollection(config);

            characteristicSerie = new LineSeries();

            Data.Add(characteristicSerie);

            designSerie = new LineSeries();
            Data.Add(designSerie);
        }

        private LineSeries characteristicSerie;
        private LineSeries designSerie;

        public SeriesCollection Data { get; private set; }

        public void AddCharacteristicChart(string title, double maxX, Func<double, double> function)
        {
            this.AddChart(title, maxX, function, characteristicSerie);
        }

        public void AddDesignChart(string title, double maxX, Func<double, double> function)
        {
            this.AddChart(title, maxX, function, designSerie);
        }

        private void AddChart(string title, double maxX, Func<double, double> function, LineSeries serie)
        {
            if (maxX.IsApproximatelyEqualTo(0))
                return;

            var scale = 1000;

            serie.Title = title;
            serie.PointRadius = 0;
            serie.Values = new ChartValues<PointD>();

            var x = 0d;
            var delta = maxX / (scale / 10);
            while (x <= maxX)
            {
                var y = function(x);
                serie.Values.Add(new PointD((x * scale).Round(), (y / scale).Round()));
                x += delta;
            }
        }
    }
}