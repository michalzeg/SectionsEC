using SectionsEC.Calculations.Results;
using SectionsEC.Calculations.Sections;

namespace SectionsEC.Calculations.Interfaces
{
    public interface ICompressionZoneCalculations
    {
        CompressionZoneResult Calculate(double x, Section section);
    }
}