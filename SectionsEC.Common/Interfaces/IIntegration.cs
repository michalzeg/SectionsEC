using SectionsEC.Common.Results;
using System;

namespace SectionsEC.Common.Interfaces
{
    public interface IIntegration
    {
        CompressionZoneResult Integrate(IIntegrable section, Func<double, double> distributionFunction);
    }
}