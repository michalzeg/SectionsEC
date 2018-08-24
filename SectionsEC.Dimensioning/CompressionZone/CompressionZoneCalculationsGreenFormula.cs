using System;
using System.Collections.Generic;
using SectionsEC.Common.Geometry;
using SectionsEC.Common.Interfaces;
using SectionsEC.Common.Materials;
using SectionsEC.Common.Results;
using SectionsEC.Common.Sections;

namespace SectionsEC.Dimensioning.CompressionZone
{
    public class CompressionZoneCalculationsGreenFormula : ICompressionZoneCalculations
    {
        private Concrete concrete;
        private IStrainCalculations strainCalculations;
        private IList<PointD> compressionZone;
        private IList<PointD> parabolicZone;
        private IList<PointD> linearZone;
        private double yNeutralAxis;
        private double y2Promiles;

        public CompressionZoneCalculationsGreenFormula(Concrete concrete, IStrainCalculations strainCalculations)
        {
            this.concrete = concrete;
            this.strainCalculations = strainCalculations;
        }

        public CompressionZoneResult Calculate(double x, Section section)
        {
            yNeutralAxis = section.MaxY - x;
            var ec2Y = this.strainCalculations.Ec2Y(x);
            y2Promiles = yNeutralAxis + ec2Y;

            compressionZone = CompressionZoneCoordinates.CoordinatesOfCompressionZone(section.Coordinates, yNeutralAxis);
            parabolicZone = CompressionZoneCoordinates.CoordinatesOfParabolicSection(compressionZone, y2Promiles);
            linearZone = CompressionZoneCoordinates.CoordinatesOfLinearSection(compressionZone, y2Promiles);

            var result = new CompressionZoneResult
            {
                NormalForce = this.calculateResultantForce(),
                Moment = this.calculateResultantMoment(x, section)
            };
            return result;
        }

        private double calculateResultantForce()
        {
            return this.resultantOfLinearSection(linearZone) + this.resultantOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis);
        }

        private double calculateResultantMoment(double x, Section section)
        {
            var rLinear = 0d;
            var rParabolic = 0d;
            if (y2Promiles < section.MaxY)
            {
                rLinear = (this.firstMomentOfAreaOfLinearSection(linearZone) / this.resultantOfLinearSection(linearZone)) - section.MinY;
            }
            var volumeOfLinearZone = this.resultantOfLinearSection(linearZone);
            var volumeOfParabolicZone = this.resultantOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis);
            var momentParabolic = firstMomentOfAreaOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis);
            rParabolic = section.MaxY - x + (this.firstMomentOfAreaOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis) / this.resultantOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis)) - section.MinY;
            return this.resultantOfLinearSection(linearZone) * rLinear + this.resultantOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis) * rParabolic;
        }

        private double resultantOfParabolicSection(IList<PointD> parabolicSection, double ec2Y, double neutralAxisY)
        {
            double ymax = ec2Y - neutralAxisY;
            double factorA = this.concrete.Fcd * ymax / 3;
            double factorC = 1 / ymax;
            double factorV = 0.0;

            for (int i = 0; i <= parabolicSection.Count - 2; i++)
            {
                var dx = parabolicSection[i + 1].X - parabolicSection[i].X;
                var dy = parabolicSection[i + 1].Y - parabolicSection[i].Y;
                var xi = parabolicSection[i].X;
                var yi = parabolicSection[i].Y - neutralAxisY;
                factorV = factorV + factorA * dx * (factorC * factorC * factorC * dy * dy * dy / 4 + Math.Pow(factorC * yi - 1, 3) + factorC * factorC * dy * dy * (factorC * yi - 1) + 0.5 * 3 * factorC * dy * (factorC * yi - 1) * (factorC * yi - 1)) + this.concrete.Fcd * dy * (dx / 2 + xi);
            }
            return factorV;
        }

        private double firstMomentOfAreaOfParabolicSection(IList<PointD> parabolicSection, double ec2Y, double neutralAxisY)
        {
            double ymax = ec2Y - neutralAxisY;
            double factorC = 1 / ymax;
            double factorS = 0d;

            for (int i = 0; i <= parabolicSection.Count - 2; i++)
            {
                var dx = parabolicSection[i + 1].X - parabolicSection[i].X;
                var dy = parabolicSection[i + 1].Y - parabolicSection[i].Y;
                var xi = parabolicSection[i].X;
                var yi = parabolicSection[i].Y - neutralAxisY;
                factorS = factorS + this.concrete.Fcd / (factorC * factorC) * dx * (factorC * factorC * factorC * factorC * dy * dy * dy * dy / 20 + factorC * factorC * factorC * factorC * dy * dy * dy * yi / 4 + factorC * factorC * factorC * factorC * dy * dy * yi * yi / 2 + factorC * factorC * factorC * factorC * dy * yi * yi * yi / 2 + factorC * factorC * factorC * factorC * yi * yi * yi * yi / 4 - factorC * factorC * factorC * dy * dy * dy / 6 - 2 * factorC * factorC * factorC * dy * dy * yi / 3 - factorC * factorC * factorC * dy * yi * yi - 2 * factorC * factorC * factorC * yi * yi * yi / 3 + factorC * factorC * dy * dy / 6 + factorC * factorC * dy * yi / 2 + factorC * factorC * yi * yi / 2 - (1 / 12)) + this.concrete.Fcd * dy * (dx * dy / 3 + dy * xi / 2 + dx * yi / 2 + xi * yi);
            }
            return factorS;
        }

        private double resultantOfLinearSection(IList<PointD> linearSection)
        {
            double factorV = 0;
            for (int i = 0; i <= linearSection.Count - 2; i++)
            {
                var x1 = linearSection[i].X;
                var x2 = linearSection[i + 1].X;
                var y1 = linearSection[i].Y;
                var y2 = linearSection[i + 1].Y;
                factorV = factorV + (x1 - x2) * (y2 + y1);
            }
            factorV = 0.5 * factorV * this.concrete.Fcd;
            return factorV;
        }

        private double firstMomentOfAreaOfLinearSection(IList<PointD> linearSection)
        {
            double factorS = 0;
            for (int i = 0; i <= linearSection.Count - 2; i++)
            {
                var x1 = linearSection[i].X;
                var x2 = linearSection[i + 1].X;
                var y1 = linearSection[i].Y;
                var y2 = linearSection[i + 1].Y;
                factorS = factorS + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
            }
            factorS = factorS * this.concrete.Fcd / 6;
            return factorS;
        }
    }
}