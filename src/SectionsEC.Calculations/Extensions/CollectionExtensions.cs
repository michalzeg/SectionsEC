using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SectionsEC.Calculations.Extensions
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            var result = new ObservableCollection<T>();
            foreach (var item in collection)
            {
                result.Add(item);
            }
            return result;
        }
    }
}