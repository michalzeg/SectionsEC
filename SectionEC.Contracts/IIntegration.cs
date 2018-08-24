using System;

namespace SectionsEC.Dimensioning
{
    public interface IIntegration
    {
        CompressionZoneResult Integrate(IIntegrable section, Func<double, double> distributionFunction);
    }
}