using SectionsEC.Helpers;

namespace SectionsEC.Dimensioning
{
    internal interface ICompressionZoneCalculations
    {
        CompressionZoneResult Calculate(double x, Section section);
    }
}