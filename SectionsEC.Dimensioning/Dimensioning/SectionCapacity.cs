﻿using System;
using System.Linq;
using System.Collections.Generic;
using SectionsEC.Helpers;

using SectionsEC.Extensions;
using SectionsEC.Contracts;

namespace SectionsEC.Dimensioning
{
    public class SectionCapacity
    {
        private ICompressionZoneCalculations compressionZoneCalculations;
        private IStrainCalculations strainCalculations;
        private IList<Reinforcement> reinforcement;
        private readonly Concrete concrete;
        private readonly Steel steel;
        private Section section;
        private double nEd;

        public SectionCapacity(Concrete concrete, Steel steel)
        {
            this.concrete = concrete;
            this.steel = steel;
        }

        private void CalculateEffectiveDepthOfSection()
        {
            foreach (var bar in this.reinforcement)
            {
                bar.D = section.MaxY - bar.Bar.Y;
            }
        }

        private double EqulibriumEquation(double x)
        {
            var forceInConcrete = this.ForceInConcrete(x);
            var forceInAs1 = this.ForceInTensionSteel(x);
            var forceInAs2 = this.ForceInCompressedSteel(x);
            var result = forceInConcrete + forceInAs2 - forceInAs1 - this.nEd;
            return result;
        }

        private double ForceInTensionSteel(double x)
        {
            var resultantForce = 0d;
            var yNeutralAxis = this.section.MaxY - x;
            for (int i = 0; i <= this.reinforcement.Count - 1; i++)
            {
                if (this.reinforcement[i].Bar.Y < yNeutralAxis)
                {
                    var effectiveHeight = this.reinforcement[i].D;
                    var strain = this.strainCalculations.StrainInAs1(x, effectiveHeight);
                    resultantForce = resultantForce + this.reinforcement[i].Bar.Area * StressFunction.SteelStressDesign(strain, this.steel);
                    var barsTemp = this.reinforcement[i];
                    barsTemp.E = strain;
                    barsTemp.IsCompressed = false;
                    this.reinforcement[i] = barsTemp;
                }
            }
            return resultantForce;
        }

        private double ForceInCompressedSteel(double x)
        {
            var resultantForce = 0d;
            var yNeutralAxis = this.section.MaxY - x;
            for (int i = 0; i <= this.reinforcement.Count - 1; i++)
            {
                if (this.reinforcement[i].Bar.Y > yNeutralAxis)
                {
                    var effectiveHeight = this.reinforcement[i].D;
                    var strain = this.strainCalculations.StrainInAs2(x, effectiveHeight);
                    resultantForce = resultantForce + this.reinforcement[i].Bar.Area * StressFunction.SteelStressDesign(strain, this.steel);
                    var barsTemp = this.reinforcement[i];
                    barsTemp.E = strain;
                    barsTemp.IsCompressed = true;
                    this.reinforcement[i] = barsTemp;
                }
            }
            return resultantForce;
        }

        private double ForceInConcrete(double x)
        {
            var result = this.compressionZoneCalculations.Calculate(x, this.section);
            return result.NormalForce;
        }

        private double SolveEqulibriumEquation()
        {
            double error = 0.00000000001;

            double xLeft = 0.000001 * this.section.H;
            double xRight = 10 * this.section.H;
            double xMedium = (xLeft + xRight) / 2;
            double result;
            int tryCount = 0;
            while ((Math.Abs(xLeft - xRight) > error) && (tryCount < 10000))
            {
                tryCount++;
                xMedium = (xRight + xLeft) / 2;
                var fL = this.EqulibriumEquation(xLeft);
                var fR = this.EqulibriumEquation(xRight);
                var fM = this.EqulibriumEquation(xMedium);
                if (fL * fM < 0)
                {
                    xRight = xMedium;
                }
                if (fR * fM < 0)
                {
                    xLeft = xMedium;
                }
            }
            if (tryCount > 1000)
            {
                result = double.NaN;
            }
            else
            {
                result = (xRight + xLeft) / 2;
            }
            return result;
        }

        public CalculationResults CalculateCapacity(double axialForce, Section section, IList<Bar> bars)
        {
            this.section = section;
            this.strainCalculations = new StrainCalculations(this.concrete, this.steel, section);
            SetSolver();

            CreateReinforcement(bars);
            this.CalculateEffectiveDepthOfSection();
            this.section.D = this.reinforcement.Max(bar => bar.D);
            this.nEd = axialForce;
            var resultX = this.SolveEqulibriumEquation();
            var forces = this.compressionZoneCalculations.Calculate(resultX, this.section);

            var result = new CalculationResults();
            result.D = this.section.D;
            result.X = resultX;
            result.MrdConcrete = forces.Moment;
            result.ForceConcrete = forces.NormalForce;
            result.Mrd = MomentReinforcement(resultX) + result.MrdConcrete - this.nEd * (this.section.H - this.section.Cz);
            result.CompressionZone = CompressionZoneCoordinates.CoordinatesOfCompressionZone(this.section.Coordinates, this.section.MaxY - resultX);
            result.Bars = this.reinforcement;
            result.Ec = this.strainCalculations.StrainInConcrete(resultX, 0);
            result.H = section.H;
            result.Cz = section.Cz;
            return result;
        }

        private void SetSolver()
        {
            if (this.concrete.N.IsApproximatelyEqualTo(2d))
                this.compressionZoneCalculations = new CompressionZoneCalculationsGreenFormula(this.concrete, this.strainCalculations);
            else
                this.compressionZoneCalculations = new CompressionZoneCalculationsNumericalFormula(this.concrete, this.strainCalculations);
        }

        private void CreateReinforcement(IList<Bar> bars)
        {
            this.reinforcement = new List<Reinforcement>();
            foreach (var bar in bars)
            {
                this.reinforcement.Add(new Reinforcement() { Bar = bar });
            }
        }

        private double MomentReinforcement(double x)
        {
            double moment = 0;
            double elevationY = this.section.MaxY - x;
            double momentZ = 0;
            Reinforcement barsTemp;
            for (int i = 0; i <= this.reinforcement.Count - 1; i++)
            {
                barsTemp = this.reinforcement[i];
                if (this.reinforcement[i].Bar.Y > elevationY)
                {
                    momentZ = reinforcement[i].Bar.Area * StressFunction.SteelStressDesign(reinforcement[i].E, this.steel) * (reinforcement[i].Bar.Y - this.section.MinY);
                    moment = moment + momentZ;
                }
                else
                {
                    momentZ = reinforcement[i].Bar.Area * StressFunction.SteelStressDesign(reinforcement[i].E, this.steel) * (reinforcement[i].Bar.Y - this.section.MinY);
                    moment = moment - momentZ;
                }
                barsTemp.My = momentZ;
                this.reinforcement[i] = barsTemp;
            }
            return moment;
        }
    }
}