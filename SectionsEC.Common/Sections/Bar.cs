using SectionsEC.Common.Extensions;
using System;

namespace SectionsEC.Common.Sections
{
    public class Bar : IEquatable<Bar>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Area { get; set; }

        public bool Equals(Bar other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return X.IsApproximatelyEqualTo(other.X) && Y.IsApproximatelyEqualTo(other.Y) && Area.IsApproximatelyEqualTo(other.Area);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var bar = obj as Bar;
            if (bar == null)
                return false;
            else
                return this.Equals(bar);
        }

        public override int GetHashCode()
        {
            int hashX = X.GetHashCode();
            int hashY = Y.GetHashCode();
            int hashAs = Area.GetHashCode();
            return hashX ^ hashY ^ hashAs;
        }
    }
}