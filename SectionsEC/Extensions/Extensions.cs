using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SectionsEC.Extensions
{
    public static class ExtensionMethods
    {
        public const double MaximumDifferenceAllowed = 0.0000001;

        public static bool IsApproximatelyEqualTo(this double initialValue, double value)
        {
            var result = ExtensionMethods.IsApproximatelyEqualTo(initialValue, value, MaximumDifferenceAllowed);
            return result;
        }

        public static bool IsApproximatelyEqualTo(this double initialValue, double value, double maximumDifferenceAllowed)
        {
            return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
        }

        public static double Round(this double initialValue)
        {
            return ExtensionMethods.Round(initialValue, 2);
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

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            var col = new ObservableCollection<T>();
            foreach (var cur in enumerable)
            {
                col.Add(cur);
            }
            return col;
        }

    }
}
