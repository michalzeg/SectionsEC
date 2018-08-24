using System;
using System.Collections.Generic;
using System.Text;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.LoadCases;
using SectionsEC.Calculations.Materials;
using SectionsEC.Calculations.Progress;
using SectionsEC.Calculations.Results;
using SectionsEC.Calculations.Sections;
using SectionsEC.Dimensioning;

namespace SectionsEC.Dimensioning.Dimensioning
{
    public static class CapacityCalculator
    {
        public static IEnumerable<CalculationResults> GetSectionCapacity(Concrete concrete, Steel steel, IList<PointD> sectionCoordinates, IList<Bar> bars, IList<LoadCase> loadCases, IProgress<ProgressArgument> progressIndicatior)
        {
            var capacity = new SectionCapacity(concrete, steel);
            var section = new Section(sectionCoordinates);
            var results = CalculateCapacity(bars, loadCases, progressIndicatior, capacity, section);
            return results;
        }

        private static List<CalculationResults> CalculateCapacity(IList<Bar> bars, IList<LoadCase> loadCases, IProgress<ProgressArgument> progressIndicatior, SectionCapacity capacity, Section section)
        {
            var results = new List<CalculationResults>();
            for (int i = 0; i <= loadCases.Count - 1; i++)
            {
                var loadCase = loadCases[i];
                progressIndicatior.Report(ProgressArgument.CalculateProgress(i, loadCases.Count, loadCase.Name));
                var result = capacity.CalculateCapacity(loadCase.NormalForce, section, bars);
                result.LoadCase = loadCase;
                results.Add(result);
            }

            return results;
        }

        public static IEnumerable<DetailedResult> GetDetailedResults(Concrete concrete, Steel steel, IEnumerable<CalculationResults> calcualtionResults)
        {
            var resultList = new List<DetailedResult>();
            foreach (var calculationResult in calcualtionResults)
            {
                var result = DetailedResult.PrepareDetailedResults(calculationResult, steel, concrete);
                resultList.Add(result);
            }
            return resultList;
        }
    }
}