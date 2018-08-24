using System;

namespace SectionsEC.Calculations.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this Object item)
        {
            return item == null;
        }
    }
}