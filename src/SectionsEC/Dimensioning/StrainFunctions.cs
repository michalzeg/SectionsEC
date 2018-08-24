using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.Dimensioning
{
    internal static class StrainFunctions
    {
        static public class AssumedMaxStrainInSteel
        {
            static public double Esi(double di, double x, double d, double eud)
            {
                double esi;
                esi = eud / (d - x) * (di - x);
                return esi;
            }

            static public double Es2i(double di, double x, double d, double eud)
            {
                double es2i;
                es2i = eud / (d - x) * (x - di);
                return es2i;
            }

            static public double Ecmax(double d, double x, double eud)
            {
                double ecmax;
                ecmax = eud / (d - x) * x;
                return ecmax;
            }

            static public double Ec2Y(double d, double x, double eud, double ec2)
            {
                double ec2Y;
                ec2Y = (d - x) / eud * ec2;
                return ec2Y;
            }

            static public double Ec(double d, double x, double di, double eud)
            {
                double ec = eud / (d - x) * (x - di);
                return ec;
            }
        }

        static public class AssumedMaxStrainInConcrete
        {
            static public double Esi(double di, double x, double ecu2)
            {
                double esi;
                esi = ecu2 * (di - x) / x;
                return esi;
            }

            static public double Es2i(double di, double x, double ecu2)
            {
                double es2i;
                es2i = ecu2 / x * (x - di);
                return es2i;
            }

            static public double Ec2Y(double x, double ec2, double ecu2)
            {
                double ec2Y;
                ec2Y = x / ecu2 * ec2;
                return ec2Y;
            }

            static public double E37h(double x, double ec2, double ecu2, double h)
            {
                double c = (1 - ec2 / ecu2) * h;
                double ec;
                ec = ecu2 / x * (x - c);
                return ec;
            }

            static public double Ec(double x, double di, double ecu2)
            {
                double ec = ecu2 / x * (x - di);
                return ec;
            }
        }

        static public class AssumedMaxStrainIn37H
        {
            static public double Ecmax(double x, double ec2, double ecu2, double h)
            {
                double c = (1 - ec2 / ecu2) * h;
                double ecmax;
                ecmax = ec2 / (x - c) * x;
                return ecmax;
            }

            static public double Es2i(double x, double di, double ec2, double ecu2, double h)
            {
                double c = (1 - ec2 / ecu2) * h;
                double es2i;
                es2i = ec2 / (x - c) * (x - di);
                return es2i;
            }

            static public double Ec2Y(double h, double ec2, double ecu2)
            {
                double ec2Y = (1 - ec2 / ecu2) * h;
                return ec2Y;
            }

            static public double Ec(double x, double di, double h, double ec2, double ecu2)
            {
                double c = (1 - ec2 / ecu2) * h;
                double ec = ec2 / (x - c) * (x - di);
                return ec;
            }
        }
    }

    public interface IStrainCalculations
    {
        double Ec2Y(double x);

        double StrainInAs1(double x, double di);

        double StrainInAs2(double x, double di);

        double StrainInConcrete(double x, double di);
    }

    public class StrainCalculations : IStrainCalculations
    {
        private Concrete concrete;
        private Steel steel;
        private Section section;

        public StrainCalculations(Concrete concrete, Steel steel, Section section)
        {
            this.concrete = concrete;
            this.steel = steel;
            this.section = section;
        }

        public double Ec2Y(double x)
        {
            double ec2Y;
            if (x > this.section.D)
            {
                if (StrainFunctions.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    ec2Y = StrainFunctions.AssumedMaxStrainIn37H.Ec2Y(this.section.H, this.concrete.Ec2, this.concrete.Ecu2);
                }
                else
                {
                    ec2Y = StrainFunctions.AssumedMaxStrainInConcrete.Ec2Y(x, this.concrete.Ec2, this.concrete.Ecu2);
                }
            }
            else
            {
                if (StrainFunctions.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    ec2Y = StrainFunctions.AssumedMaxStrainInSteel.Ec2Y(this.section.D, x, this.steel.Eud, this.concrete.Ec2);
                }
                else
                {
                    ec2Y = StrainFunctions.AssumedMaxStrainInConcrete.Ec2Y(x, this.concrete.Ec2, this.concrete.Ecu2);
                }
            }
            return ec2Y;
        }

        public double StrainInAs2(double x, double di)
        {
            double e;
            if (x > this.section.D)
            {
                if (StrainFunctions.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    e = StrainFunctions.AssumedMaxStrainIn37H.Es2i(x, di, this.concrete.Ec2, this.concrete.Ecu2, this.section.H);
                }
                else
                {
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Es2i(di, x, this.concrete.Ecu2);
                }
            }
            else
            {
                if (StrainFunctions.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    e = StrainFunctions.AssumedMaxStrainInSteel.Es2i(di, x, this.section.D, this.steel.Eud);
                }
                else
                {
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Es2i(di, x, this.concrete.Ecu2);
                }
            }
            return e;
        }

        public double StrainInAs1(double x, double di)
        {
            double e;
            if (x > this.section.D)
            {
                e = 0;
            }
            else
            {
                if (StrainFunctions.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    e = StrainFunctions.AssumedMaxStrainInSteel.Esi(di, x, this.section.D, this.steel.Eud);
                }
                else
                {
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Esi(di, x, this.concrete.Ecu2);
                }
            }
            return e;
        }

        public double StrainInConcrete(double x, double di)
        {
            double e;
            if (x > this.section.D)
            {
                if (StrainFunctions.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    e = StrainFunctions.AssumedMaxStrainIn37H.Ec(x, di, section.H, concrete.Ec2, concrete.Ecu2);
                }
                else
                {
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Ec(x, di, concrete.Ecu2);
                }
            }
            else
            {
                if (StrainFunctions.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    e = StrainFunctions.AssumedMaxStrainInSteel.Ec(section.D, x, di, steel.Eud);
                }
                else
                {
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Ec(x, di, concrete.Ecu2);
                }
            }
            return e;
        }
    }
}