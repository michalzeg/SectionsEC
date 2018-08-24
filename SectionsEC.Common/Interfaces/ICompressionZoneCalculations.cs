using SectionsEC.Helpers;

namespace SectionsEC.Contracts
{
    public interface ICompressionZoneCalculations
    {
        CompressionZoneResult Calculate(double x, Section section);
    }
}