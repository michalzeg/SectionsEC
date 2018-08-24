using SectionsEC.Common.Results;
using SectionsEC.Common.Sections;

namespace SectionsEC.Common.Interfaces
{
    public interface ICompressionZoneCalculations
    {
        CompressionZoneResult Calculate(double x, Section section);
    }
}