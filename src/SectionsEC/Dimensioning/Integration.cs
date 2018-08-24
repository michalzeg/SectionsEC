using System;

namespace SectionsEC.Dimensioning
{
    public class Integration : IIntegration
    {
        private readonly int numberOfSlices = 1000;

        public Integration()
        {
        }

        public CompressionZoneResult Integrate(IIntegrable section, Func<double, double> distributionFunction)
        {
            var slicing = new Slicing();
            var deltaY = (section.MaxY - section.MinY) / this.numberOfSlices;
            var currentY = section.MinY;
            var resultantMoment = 0d;
            var resultantNormalForce = 0d;
            while (currentY <= section.MaxY)
            {
                var slice = slicing.GetSlice(section.Coordinates, currentY + deltaY, currentY);
                currentY = currentY + deltaY;
                var value = distributionFunction(slice.CentreOfGravityY);
                var normalForce = value * slice.Area;
                var leverArm = Math.Abs(section.IntegrationPointY - slice.CentreOfGravityY);
                var moment = leverArm * value * slice.Area;
                resultantMoment = resultantMoment + moment;
                resultantNormalForce = resultantNormalForce + normalForce;
            }
            var result = new CompressionZoneResult
            {
                NormalForce = resultantNormalForce,
                Moment = resultantMoment
            };
            return result;
        }
    }
}