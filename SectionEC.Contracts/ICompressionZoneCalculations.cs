using SectionsEC.Helpers;

namespace SectionsEC.Contracts
{
    internal interface ICompressionZoneCalculations
    {
        CompressionZoneResult Calculate(double x, Section section);
    }
}