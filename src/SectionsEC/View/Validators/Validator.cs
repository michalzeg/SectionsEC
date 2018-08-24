using SectionsEC.Common.Extensions;
using SectionsEC.Common.Geometry;
using SectionsEC.Common.LoadCases;
using SectionsEC.Common.Materials;
using SectionsEC.Common.Sections;
using SectionsEC.Dimensioning;
using SectionsEC.Dimensioning.Dimensioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.View.Validators
{
    public class Validators
    {
        public static string Validate(Concrete concrete, Steel steel, IList<LoadCase> loadCases, IList<Bar> bars, IList<PointD> sectionCoordinates)
        {
            StringBuilder result = new StringBuilder();

            if (concrete.IsNull())
                result.AppendLine("Concrete has not been defined");
            if (steel.IsNull())
                result.AppendLine("Steel has not been defined");
            if (loadCases.IsNull())
                result.AppendLine("Loads have not been defined");
            if (bars.IsNull())
                result.AppendLine("Bars have not been defined");
            if (sectionCoordinates.IsNull())
                result.AppendLine("Section has not been defined");

            if (result.ToString() == string.Empty)
            {
                var loadCasesValidation = ValidateLoadCases(concrete, steel, loadCases, bars, sectionCoordinates);
                if (loadCasesValidation != string.Empty)
                    result.AppendLine(loadCasesValidation);

                var loadCasesDuplication = CheckDuplicatedLoadCases(loadCases);
                if (loadCasesDuplication != string.Empty)
                    result.AppendLine(loadCasesDuplication);
            }
            return result.ToString();
        }

        private static string ValidateLoadCases(Concrete concrete, Steel steel, IList<LoadCase> loadCases, IList<Bar> bars, IList<PointD> sectionCoordinates)
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

        private static string CheckDuplicatedLoadCases(IList<LoadCase> loadCases)
        {
            var result = new StringBuilder();
            var duplicatedLoadCases = loadCases.GroupBy(e => e.Name).Where(e => e.Count() > 1).Select(e => e.Key);
            if (duplicatedLoadCases.Count() > 0)
            {
                foreach (var loadCase in duplicatedLoadCases)
                    result.AppendLine(string.Format("{0} is duplicated", loadCase));
            }
            return result.ToString();
        }
    }
}