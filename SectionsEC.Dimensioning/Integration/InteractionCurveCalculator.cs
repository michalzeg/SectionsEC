using System;
using System.Collections.Generic;
using SectionsEC.Helpers;
using SectionsEC.Progress;

namespace SectionsEC.Dimensioning
{
    public class InteractionCurveCalculator
    {
        private readonly int deltaAngle = 5;
        private IList<Bar> bars;
        private IList<PointD> coordinates;
        private Concrete concrete;
        private Steel steel;
        private IList<LoadCase> loadCases;

        public InteractionCurveCalculator(Concrete concrete, Steel steel, IList<Bar> bars, IList<PointD> coordinates, IList<LoadCase> loadCases)
        {
            this.concrete = concrete;
            this.steel = steel;
            this.concrete = concrete;
            this.steel = steel;
            this.loadCases = loadCases;
            this.bars = bars;
            this.coordinates = coordinates;
        }

        public IDictionary<LoadCase, IEnumerable<InteractionCurveResult>> GetInteractionCurve(IProgress<ProgressArgument> progress)
        {
            var sectionCapacity = new SectionCapacity(concrete, steel);
            var result = new Dictionary<LoadCase, IEnumerable<InteractionCurveResult>>();
            for (int i = 0; i <= loadCases.Count - 1; i++)
            {
                var loadCase = loadCases[i];
                progress.Report(ProgressArgument.CalculateProgress(i, loadCases.Count, loadCase.Name));
                var interactionResult = new List<InteractionCurveResult>();
                int angle = 0;
                while (angle <= 360)
                {
                    var rotatedCoordinates = this.RotateSectionCoordinates(angle);
                    var rotatedSection = new Section(rotatedCoordinates);
                    var rotatedBars = this.RotateBarCoordinates(angle);
                    var capacityResult = sectionCapacity.CalculateCapacity(loadCase.NormalForce, rotatedSection, rotatedBars);
                    if (double.IsNaN(capacityResult.X))
                    {
                        throw new InvalidOperationException();
                    }

                    CalculatePrincipalMoments(angle, capacityResult.Mrd, out double mx, out double my);
                    var interactionMoments = new InteractionCurveResult();
                    interactionMoments.Mx = mx;
                    interactionMoments.My = my;
                    interactionResult.Add(interactionMoments);
                    angle = angle + this.deltaAngle;
                }
                result.Add(loadCase, interactionResult);
            }
            return result;
        }

        private IList<Bar> RotateBarCoordinates(double angle)
        {
            var newBars = new List<Bar>();
            foreach (var bar in this.bars)
            {
                var x = bar.X * Math.Cos(angle * Math.PI / 180) - bar.Y * Math.Sin(angle * Math.PI / 180);
                var y = bar.X * Math.Sin(angle * Math.PI / 180) + bar.Y * Math.Cos(angle * Math.PI / 180);
                var newBar = new Bar() { X = x, Y = y, Area = bar.Area };
                newBars.Add(newBar);
            }
            return newBars;
        }

        private IList<PointD> RotateSectionCoordinates(double angle)
        {
            List<PointD> coordinates = new List<PointD>();
            foreach (var point in this.coordinates)
            {
                var x = point.X * Math.Cos(angle * Math.PI / 180) - point.Y * Math.Sin(angle * Math.PI / 180);
                var y = point.X * Math.Sin(angle * Math.PI / 180) + point.Y * Math.Cos(angle * Math.PI / 180);
                coordinates.Add(new PointD(x, y));
            }
            return coordinates;
        }

        private void CalculatePrincipalMoments(double angle, double moment, out double mx, out double my)
        {
            my = moment * Math.Cos((90 - angle) * Math.PI / 180);
            mx = moment * Math.Sin((90 - angle) * Math.PI / 180);
        }
    }
}