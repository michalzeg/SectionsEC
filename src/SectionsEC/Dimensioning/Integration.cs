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
            double resultantMoment = 0;
            double resultantNormalForce = 0;
            Slicing slicing = new Slicing();
            double currentY = section.MinY;
            double deltaY = (section.MaxY - section.MinY) / this.numberOfSlices;
            while (currentY <= section.MaxY)
            {
                SectionSlice slice = slicing.GetSlice(section.Coordinates, currentY + deltaY, currentY);
                currentY = currentY + deltaY;
                double value = distributionFunction(slice.CentreOfGravityY);
                double normalForce = value * slice.Area;
                double leverArm = Math.Abs(section.IntegrationPointY - slice.CentreOfGravityY);
                double moment = leverArm * value * slice.Area;
                resultantMoment = resultantMoment + moment;
                resultantNormalForce = resultantNormalForce + normalForce;
            }
            var result = new CompressionZoneResult();
            result.NormalForce = resultantNormalForce;
            result.Moment = resultantMoment;
            return result;
        }
    }
}