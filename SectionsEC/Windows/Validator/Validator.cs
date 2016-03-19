using SectionsEC.Dimensioning;
using SectionsEC.Extensions;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.Windows.Validator
{
    public class Validators
    {
        public static string Validate(Concrete concrete,Steel steel, IList<LoadCase> loadCases,IList<Bar> bars,IList<PointD> sectionCoordinates)
        {
            StringBuilder result = new StringBuilder();

            if (concrete.isNull())
                result.AppendLine("Concrete has not been defined");
            if (steel.isNull())
                result.AppendLine("Steel has not been defined");
            if (loadCases.isNull())
                result.AppendLine("Loads have not been defined");
            if (bars.isNull())
                result.AppendLine("Bars have not been defined");
            if (sectionCoordinates.isNull())
                result.AppendLine("Section has not been defined");

            if (result.ToString() == string.Empty)
            {
                var loadCasesValidation = validateLoadCases(concrete, steel, loadCases, bars, sectionCoordinates);
                if (loadCasesValidation != string.Empty)
                    result.AppendLine(loadCasesValidation.ToString());
            }
            return result.ToString();
        }

        private static string validateLoadCases(Concrete concrete, Steel steel, IList<LoadCase> loadCases, IList<Bar> bars, IList<PointD> sectionCoordinates)
        {
            StringBuilder result = new StringBuilder();

            double tensionCapacity = AxialCapacity.TensionCapacity(bars, steel);
            double compressionCapacity = AxialCapacity.CompressionCapacity(sectionCoordinates, concrete);
            foreach (var load in loadCases)
            {
                if (load.NormalForce > compressionCapacity)
                    result.AppendLine(string.Format("Normal force in load case {0} exceedes axial compression capacity", load.Name));
                else if (load.NormalForce < tensionCapacity)
                    result.AppendLine(string.Format("Normal force in load case {0} exceedes axial tension capacity", load.Name));
            }
            return result.ToString();
        }
    }


}
