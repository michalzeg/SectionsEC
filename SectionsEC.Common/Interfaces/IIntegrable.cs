using System.Collections.Generic;
using SectionsEC.Helpers;

namespace SectionsEC.Contracts
{
    public interface IIntegrable
    {
        IList<PointD> Coordinates { get; }
        double IntegrationPointY { get; }
        double MaxY { get; }
        double MinY { get; }
    }
}