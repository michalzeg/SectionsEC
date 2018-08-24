using System.Collections.Generic;
using SectionsEC.Common.Geometry;

namespace SectionsEC.Common.Interfaces
{
    public interface IIntegrable
    {
        IList<PointD> Coordinates { get; }
        double IntegrationPointY { get; }
        double MaxY { get; }
        double MinY { get; }
    }
}