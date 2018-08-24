using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.StressCalculations
{
    internal static class StressFunctions
    {
        static public double ConcreteStressDesign(double ec, Concrete concrete)
        {
            double stress = 0;
            if (ec < 0)
                stress = 0;
            if (ec <= concrete.Ec2)
            {
                stress = concrete.Fcd * (1 - Math.Pow((1 - ec / concrete.Ec2), concrete.N));
            }
            else if (ec <= concrete.Ecu2)
            {
                stress = concrete.Fcd;
            }
            return stress;
        }

        static public double ConcreteStressCharacteristic(double ec, Concrete concrete)
        {
            double s = 0;
            if (ec <= concrete.Ec2)
            {
                s = concrete.Fck * (1 - Math.Pow((1 - ec / concrete.Ec2), concrete.N));
            }
            else
            {
                s = concrete.Fck;
            }
            return s;
        }

        static public double SteelStressDesign(double e, Steel steel)
        {
            double a;
            double b;
            double stress = 0;
            if (e < 0)
                stress = 0;
            else if (e <= (steel.Fyd / steel.Es))
            {
                stress = e * steel.Es;
            }
            else if (e <= steel.Eud)
            {
                a = (steel.K * steel.Fyd - steel.Fyd) / (steel.Euk - steel.Fyd / steel.Es);
                b = steel.Fyd - a * steel.Fyd / steel.Es;
                stress = a * e + b;
            }
            return stress;
        }

        static public double SteelStressCharacteristic(double e, Steel steel)
        {
            double s;
            if (e <= (steel.Fyk / steel.Es))
            {
                s = e * steel.Es;
            }
            else if (e <= steel.Euk)
            {
                var a = (steel.K * steel.Fyk - steel.Fyk) / (steel.Euk - steel.Fyk / steel.Es);
                var b = steel.Fyk - a * steel.Fyk / steel.Es;
                s = a * e + b;
            }
            else
            {
                s = 0;
            }
            return s;
        }
    }
}