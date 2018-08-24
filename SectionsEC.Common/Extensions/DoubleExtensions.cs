using System;
using System.Linq;
using System.Text;

namespace SectionsEC.Calculations.Extensions
{
    public static class DoubleExtensions
    {
        public const double MaximumDifferenceAllowed = 0.0000001;

        public static bool IsApproximatelyEqualTo(this double initialValue, double value)
        {
            var result = DoubleExtensions.IsApproximatelyEqualTo(initialValue, value, MaximumDifferenceAllowed);
            return result;
        }

        public static bool IsApproximatelyEqualTo(this double initialValue, double value, double maximumDifferenceAllowed)
        {
            return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
        }

        public static double Round(this double initialValue)
        {
            return DoubleExtensions.Round(initialValue, 2);
        }

        public static double Round(this double initialValue, int numberOfDigits)
        {
            return (Math.Round(initialValue, numberOfDigits));
        }

        public static bool IsNaN(this double initialValue)
        {
            return double.IsNaN(initialValue);
        }

        public static string ToFormatedString(this double initialValue)
        {
            return initialValue.Round().ToString("F");
        }
    }
}