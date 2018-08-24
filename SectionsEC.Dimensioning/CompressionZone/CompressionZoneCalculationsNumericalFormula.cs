using System;
using SectionsEC.Common.Interfaces;
using SectionsEC.Common.Materials;
using SectionsEC.Common.Results;
using SectionsEC.Common.Sections;
using SectionsEC.Common.StressFunctions;
using SectionsEC.Dimensioning.Integration;

namespace SectionsEC.Dimensioning.CompressionZone
{
    public class CompressionZoneCalculationsNumericalFormula : ICompressionZoneCalculations
    {
        private IStrainCalculations strainCalculations;
        private Concrete concrete;

        public CompressionZoneCalculationsNumericalFormula(Concrete concrete, IStrainCalculations strainCalculations)
        {
            this.strainCalculations = strainCalculations;
            this.concrete = concrete;
        }

        public CompressionZoneResult Calculate(double x, Section section)
        {
            var compressionZoneCoordinates = CompressionZoneCoordinates.CoordinatesOfCompressionZone(section.Coordinates, section.MaxY - x);
            var compressionZone = new Section(compressionZoneCoordinates);
            compressionZone.IntegrationPointY = section.IntegrationPointY;

            Func<double, double> distance = y => section.MaxY - y;
            Func<double, double> strain = di => this.strainCalculations.StrainInConcrete(x, distance(di));
            Func<double, double> stress = e => StressFunction.ConcreteStressDesign(strain(e), this.concrete);

            var integration = new IntegrationCalculator();
            var result = integration.Integrate(compressionZone, stress);
            return result;
        }
    }
}