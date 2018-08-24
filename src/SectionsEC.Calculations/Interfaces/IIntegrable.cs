using System.Collections.Generic;
using SectionsEC.Calculations.Geometry;

namespace SectionsEC.Calculations.Interfaces
{
    public interface IIntegrable
    {
        IList<PointD> Coordinates { get; }
        double IntegrationPointY { get; }
        double MaxY { get; }
        double MinY { get; }
    }
}