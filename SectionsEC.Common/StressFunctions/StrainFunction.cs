using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC
{
    /// <summary>
    /// All variables according to Eurocode 5
    /// </summary>
    internal static class StrainFunction
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
}