using SectionsEC.Contracts;
using SectionsEC.Dimensioning;
using SectionsEC.Helpers;

namespace SectionsEC
{
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
                if (StrainFunction.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    ec2Y = StrainFunction.AssumedMaxStrainIn37H.Ec2Y(this.section.H, this.concrete.Ec2, this.concrete.Ecu2);
                }
                else
                {
                    ec2Y = StrainFunction.AssumedMaxStrainInConcrete.Ec2Y(x, this.concrete.Ec2, this.concrete.Ecu2);
                }
            }
            else
            {
                if (StrainFunction.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    ec2Y = StrainFunction.AssumedMaxStrainInSteel.Ec2Y(this.section.D, x, this.steel.Eud, this.concrete.Ec2);
                }
                else
                {
                    ec2Y = StrainFunction.AssumedMaxStrainInConcrete.Ec2Y(x, this.concrete.Ec2, this.concrete.Ecu2);
                }
            }
            return ec2Y;
        }

        public double StrainInAs2(double x, double di)
        {
            double e;
            if (x > this.section.D)
            {
                if (StrainFunction.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    e = StrainFunction.AssumedMaxStrainIn37H.Es2i(x, di, this.concrete.Ec2, this.concrete.Ecu2, this.section.H);
                }
                else
                {
                    e = StrainFunction.AssumedMaxStrainInConcrete.Es2i(di, x, this.concrete.Ecu2);
                }
            }
            else
            {
                if (StrainFunction.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    e = StrainFunction.AssumedMaxStrainInSteel.Es2i(di, x, this.section.D, this.steel.Eud);
                }
                else
                {
                    e = StrainFunction.AssumedMaxStrainInConcrete.Es2i(di, x, this.concrete.Ecu2);
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
                if (StrainFunction.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    e = StrainFunction.AssumedMaxStrainInSteel.Esi(di, x, this.section.D, this.steel.Eud);
                }
                else
                {
                    e = StrainFunction.AssumedMaxStrainInConcrete.Esi(di, x, this.concrete.Ecu2);
                }
            }
            return e;
        }

        public double StrainInConcrete(double x, double di)
        {
            double e;
            if (x > this.section.D)
            {
                if (StrainFunction.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    e = StrainFunction.AssumedMaxStrainIn37H.Ec(x, di, section.H, concrete.Ec2, concrete.Ecu2);
                }
                else
                {
                    e = StrainFunction.AssumedMaxStrainInConcrete.Ec(x, di, concrete.Ecu2);
                }
            }
            else
            {
                if (StrainFunction.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    e = StrainFunction.AssumedMaxStrainInSteel.Ec(section.D, x, di, steel.Eud);
                }
                else
                {
                    e = StrainFunction.AssumedMaxStrainInConcrete.Ec(x, di, concrete.Ecu2);
                }
            }
            return e;
        }
    }
}