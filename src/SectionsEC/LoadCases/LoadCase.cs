using SectionsEC.Extensions;
using System;

namespace SectionsEC.Helpers
{
    public class LoadCase : IEquatable<LoadCase>
    {
        private static int IdCounter;

        public string Name { get; set; }
        public double NormalForce { get; set; }
        public int Id { get; private set; }

        public LoadCase()
        {
            IdCounter++;
            this.Id = IdCounter;
            this.Name = string.Empty;
            this.NormalForce = 0d;
        }

        public bool Equals(LoadCase other)
        {
            if (other == null)
                return false;

            if (this.Name == other.Name
                && this.NormalForce.IsApproximatelyEqualTo(other.NormalForce)
                && this.Id == other.Id)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            int hashName = Name.GetHashCode();
            int hashNormalForce = NormalForce.GetHashCode();
            return hashName ^ hashNormalForce;
        }
    }
}