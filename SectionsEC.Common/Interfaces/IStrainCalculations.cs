namespace SectionsEC.Common.Interfaces
{
    public interface IStrainCalculations
    {
        double Ec2Y(double x);

        double StrainInAs1(double x, double di);

        double StrainInAs2(double x, double di);

        double StrainInConcrete(double x, double di);
    }
}