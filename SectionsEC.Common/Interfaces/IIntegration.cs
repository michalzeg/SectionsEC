using SectionsEC.Calculations.Results;
using System;

namespace SectionsEC.Calculations.Interfaces
{
    public interface IIntegration
    {
        CompressionZoneResult Integrate(IIntegrable section, Func<double, double> distributionFunction);
    }
}