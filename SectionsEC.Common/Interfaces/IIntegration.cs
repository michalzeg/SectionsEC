using System;

namespace SectionsEC.Contracts
{
    public interface IIntegration
    {
        CompressionZoneResult Integrate(IIntegrable section, Func<double, double> distributionFunction);
    }
}