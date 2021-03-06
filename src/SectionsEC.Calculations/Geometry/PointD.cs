﻿using SectionsEC.Calculations.Extensions;
using System;

namespace SectionsEC.Calculations.Geometry
{
    public class PointD : IEquatable<PointD>
    {
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointD()
        {
        }

        public double X { get; set; }
        public double Y { get; set; }

        public bool Equals(PointD other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return X.IsApproximatelyEqualTo(other.X) && Y.IsApproximatelyEqualTo(other.Y);
        }

        public override int GetHashCode()
        {
            int hashY = X.GetHashCode();
            int hashValue = Y.GetHashCode();
            return hashY ^ hashValue;
        }

        public PointD Clone() => new PointD(this.X, this.Y);
    }
}