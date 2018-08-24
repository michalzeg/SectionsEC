using System.Collections.Generic;
using SectionsEC.Helpers;

namespace SectionsEC.Dimensioning
{
    public interface IIntegrable
    {
        IList<PointD> Coordinates { get; }
        double IntegrationPointY { get; }
        double MaxY { get; }
        double MinY { get; }
    }
}