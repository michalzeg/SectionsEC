using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.Materials;
using SectionsEC.Calculations.SectionProperties;
using SectionsEC.Calculations.Sections;
using System.Collections.Generic;

using System.Linq;

namespace SectionsEC.Dimensioning.Dimensioning
{
    public class AxialCapacity
    {
        public static double TensionCapacity(IList<Bar> bars, Steel steel)
        {
            var result = -bars.Sum(bar => bar.Area * steel.Fyd * steel.K);

            return result;
        }

        public static double CompressionCapacity(IList<PointD> sectionCoordinates, Concrete concrete)
        {
            if (sectionCoordinates.Count == 0)
                return 0;
            var section = new Section(sectionCoordinates);
            double areaOfConcrete = SectionPropertiesCalculator.Area(section.Coordinates);
            return areaOfConcrete * concrete.Fcd;
        }
    }
}