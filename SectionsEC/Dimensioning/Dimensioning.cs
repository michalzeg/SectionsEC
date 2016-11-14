﻿using System;
using System.Linq;
using System.Collections.Generic;
using CommonMethods;
using SectionsEC.Helpers;
using SectionsEC.StressCalculations;
using SectionsEC.Windows.WindowClasses;
using System.Text;
using SectionsEC.Dimensioning;
using SectionsEC.WindowClasses;
namespace SectionsEC.Dimensioning
{
    public static class CapacityCalculator
    {
        public static IDictionary<LoadCase, CalculationResults> GetSectionCapacity(Concrete concrete, Steel steel, IList<PointD> sectionCoordinates, IList<Bar> bars, IList<LoadCase> loadCases, IProgress<ProgressArgument> progressIndicatior)
        {
            var capacity = new SectionCapacity(concrete, steel);
            var section = new Section(sectionCoordinates);
            var resultDictionary = new Dictionary<LoadCase, CalculationResults>();
            for (int i = 0; i <= loadCases.Count - 1; i++) 
            {
                var loadCase = loadCases[i];
                progressIndicatior.Report(ProgressArgument.CalculateProgress(i, loadCases.Count, loadCase.Name));
                var result = capacity.CalculateCapacity(loadCase.NormalForce, section, bars);
                resultDictionary.Add(loadCase, result);
            }
            return resultDictionary;
        }
        public static IDictionary<LoadCase, StringBuilder> GetDetailedResults(Concrete concrete, Steel steel, IDictionary<LoadCase, CalculationResults> calcualtionResults)
        {
            var result = new Dictionary<LoadCase, StringBuilder>();
            foreach (var item in calcualtionResults)
            {
                var detailedResult = DetailedResults.PrepareDetailedResults(item.Value, item.Key.NormalForce, steel, concrete);
                result.Add(item.Key, detailedResult);
            }
            return result;
        }
    }
    public class InteractionCurveCalculator
    {
        private readonly int deltaAngle = 5;
        private IList<Bar> bars;
        private IList<PointD> coordinates;
        private Concrete concrete;
        private Steel steel;
        IList<LoadCase> loadCases;
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
                    var rotatedCoordinates = this.rotateSectionCoordinates(angle);
                    var rotatedSection = new Section(rotatedCoordinates);
                    var rotatedBars = this.rotateBarCoordinates(angle);
                    var capacityResult = sectionCapacity.CalculateCapacity(loadCase.NormalForce, rotatedSection, rotatedBars);
                    if (double.IsNaN(capacityResult.X))
                    {
                        throw new NotImplementedException();
                    }
                    double mx, my;
                    calculatePrincipalMoments(angle, capacityResult.Mrd, out mx, out my);
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
        private IList<Bar> rotateBarCoordinates(double angle)
        {
            List<Bar> newBars = new List<Bar>();
            foreach (var bar in this.bars)
            {
                double x = bar.X * Math.Cos(angle * Math.PI / 180) - bar.Y * Math.Sin(angle * Math.PI / 180);
                double y = bar.X * Math.Sin(angle * Math.PI / 180) + bar.Y * Math.Cos(angle * Math.PI / 180);
                var newBar = new Bar() { X = x, Y = y, As = bar.As };
                newBars.Add(newBar);
            }
            return newBars;
        }
        private IList<PointD> rotateSectionCoordinates(double angle)
        {
            List<PointD> coordinates = new List<PointD>();
            foreach (var point in this.coordinates)
            {
                double x = point.X * Math.Cos(angle * Math.PI / 180) - point.Y * Math.Sin(angle * Math.PI / 180);
                double y = point.X * Math.Sin(angle * Math.PI / 180) + point.Y * Math.Cos(angle * Math.PI / 180);
                coordinates.Add(new PointD(x, y));
            }
            return coordinates;
        }
        private void calculatePrincipalMoments(double angle, double moment, out double mx, out double my)
        {
            my = moment * Math.Cos((90 - angle) * Math.PI / 180);
            mx = moment * Math.Sin((90 - angle) * Math.PI / 180);
        }
    }
    public class AxialCapacity
    {
        public static double TensionCapacity(IList<Bar> bars,Steel steel)
        {
            if (bars.Count == 0)
                return 0;
            double capacity = 0;
            foreach (var bar in bars)
            {
                capacity += bar.As * steel.Fyd * steel.K;
            }
            return -capacity;
        }
        public static double CompressionCapacity(IList<PointD> sectionCoordinates,Concrete concrete)
        {
            if (sectionCoordinates.Count == 0)
                return 0;
            var section = new Section(sectionCoordinates); 
            double areaOfConcrete = SectionProperties.A(section.Coordinates);
            return areaOfConcrete * concrete.Fcd;
        }
    }
    public class SectionCapacity
    {
        private ICompressionZoneCalculations compressionZoneCalculations;
        private IStrainCalculations strainCalculations;
        private IList<Reinforcement> reinforcement;
        private Concrete concrete; 
        private Steel steel; 
        private Section section; 
        private double nEd;
        public SectionCapacity(Concrete concrete, Steel steel)
        {
            this.concrete = concrete;
            this.steel = steel;
        }
        private double calculateEffectiveDepthOfSectionAndBars()
        {
            /*Reinforcement barsTemp;
            double[] tab = new double[this.bars.Count];
            for (int i = 0; i <= this.bars.Count - 1; i++)
            {
                barsTemp = this.bars[i];
                barsTemp.D = this.section.MaxY - this.bars[i].Y;
                this.bars[i] = barsTemp;
                tab[i] = barsTemp.D;
            }*/
            var tempD = new List<double>();
            foreach (var bar in this.reinforcement)
            {
                double d = section.MaxY - bar.Bar.Y;
                tempD.Add(d);
                bar.D = d;
            }
            return tempD.Max();
        }
        private double equlibriumEquation(double x) 
        {
            var forceInConcrete = this.forceInConcrete(x);
            var forceInAs1 = this.forceInAs1(x);
            var forceInAs2 = this.forceInAs2(x);
            var result = forceInConcrete + forceInAs2 - forceInAs1 - this.nEd;
            return result;
        }
        private double forceInAs1(double x) 
        {
            var resultantForce = 0d; 
            var yNeutralAxis = this.section.MaxY - x; 
            for (int i = 0; i <= this.reinforcement.Count - 1; i++)
            {
                if (this.reinforcement[i].Bar.Y < yNeutralAxis)
                {
                    var di = this.reinforcement[i].D;
                    var e = this.strainCalculations.StrainInAs1(x, di);
                    resultantForce = resultantForce + this.reinforcement[i].Bar.As * StressFunctions.SteelStressDesign(e, this.steel);
                    var barsTemp = this.reinforcement[i];
                    barsTemp.E = e;
                    barsTemp.IsCompressed = false;
                    this.reinforcement[i] = barsTemp;
                }
            }
            return resultantForce;
        }
        private double forceInAs2(double x) 
        {
            var resultantForce = 0d; 
            var yNeutralAxis = this.section.MaxY - x; 
            for (int i = 0; i <= this.reinforcement.Count - 1; i++)
            {
                if (this.reinforcement[i].Bar.Y > yNeutralAxis)
                {
                    var di = this.reinforcement[i].D;
                    var e = this.strainCalculations.StrainInAs2(x, di);
                    resultantForce = resultantForce + this.reinforcement[i].Bar.As * StressFunctions.SteelStressDesign(e, this.steel);
                    var barsTemp = this.reinforcement[i];
                    barsTemp.E = e;
                    barsTemp.IsCompressed = true;
                    this.reinforcement[i] = barsTemp;
                }
            }
            return resultantForce;
        }
        private double forceInConcrete(double x) 
        {
            var result = this.compressionZoneCalculations.Calculate(x, this.section);
            return result.NormalForce;
        }
        private double solveEqulibriumEquation() 
        {
            double EPS = 0.00000000001; 
            double fL, fR, fM; 
            double xL = 0.000001 * this.section.H; 
            double xR = 10 * this.section.H; 
            double xM = (xL + xR) / 2;
            double x0;
            int k = 0;
            while ((Math.Abs(xL - xR) > EPS) && (k < 10000))
            {
                k++;
                xM = (xR + xL) / 2;
                fL = this.equlibriumEquation(xL);
                fR = this.equlibriumEquation(xR);
                fM = this.equlibriumEquation(xM);
                if (fL * fM < 0)
                {
                    xR = xM;
                }
                if (fR * fM < 0)
                {
                    xL = xM;
                }
            }
            if (k > 1000)
            {
                x0 = double.NaN;
            }
            else
            {
                x0 = (xR + xL) / 2;
            }
            return x0;
        }
        public CalculationResults CalculateCapacity(double nEd, Section section, IList<Bar> bars) 
        {
            this.section = section;
            this.strainCalculations = new StrainCalculations(this.concrete, this.steel, section);

            if (this.concrete.N == 2d)
                this.compressionZoneCalculations = new CompressionZoneCalculationsGreenFormula(this.concrete, this.strainCalculations);
            else
                this.compressionZoneCalculations = new CompressionZoneCalculationsNumericalFormula(this.concrete, this.strainCalculations);

            createReinforcement(bars);
            this.section.D = this.calculateEffectiveDepthOfSectionAndBars();
            this.nEd = nEd;
            var result = new CalculationResults();
            result.D = this.section.D;
            result.X = this.solveEqulibriumEquation();
            if (double.IsNaN(result.X))
            {
                return result; 
            }
            var forces = this.compressionZoneCalculations.Calculate(result.X, this.section);
            result.MrdConcrete = forces.Moment; 
            result.ForceConcrete = forces.NormalForce;
            result.Mrd = mrdReinforcement(result.X) + result.MrdConcrete - this.nEd * (this.section.H - this.section.Cz);
            result.CompressionZone = CompressionZoneCoordinates.CoordinatesOfCompressionZone(this.section.Coordinates, this.section.MaxY - result.X);
            result.Bars = this.reinforcement;
            result.Ec = this.strainCalculations.StrainInConcrete(result.X, 0);
            result.H = section.H;
            result.Cz = section.Cz;
            return result;
        }
        private void createReinforcement(IList<Bar> bars)
        {
            this.reinforcement = new List<Reinforcement>();
            foreach (var bar in bars)
            {
                this.reinforcement.Add(new Reinforcement() { Bar = bar });
            }
        }
        private double mrdReinforcement(double x) 
        {
            double Mrd = 0;
            double yOsi = this.section.MaxY - x;
            double Mz = 0;
            Reinforcement barsTemp;
            for (int i = 0; i <= this.reinforcement.Count - 1; i++)
            {
                barsTemp = this.reinforcement[i];
                if (this.reinforcement[i].Bar.Y > yOsi)
                {
                    Mz = reinforcement[i].Bar.As * StressFunctions.SteelStressDesign(reinforcement[i].E, this.steel) * (reinforcement[i].Bar.Y - this.section.MinY);
                    Mrd = Mrd + Mz;
                }
                else
                {
                    Mz = reinforcement[i].Bar.As * StressFunctions.SteelStressDesign(reinforcement[i].E, this.steel) * (reinforcement[i].Bar.Y - this.section.MinY);
                    Mrd = Mrd - Mz;
                }
                barsTemp.My = Mz;
                this.reinforcement[i] = barsTemp;
            }
            return Mrd;
        }
    }
}
