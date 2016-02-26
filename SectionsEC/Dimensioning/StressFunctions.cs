using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.StressCalculations
{
    static class StressFunctions //klasa obslugujaca obliczanie naprezen w stali i betonei na podstawie odksztalcen
    {
        static public double ConcreteStressDesign(double ec, Concrete concrete)
        {
            //funkcja wyznaczajaca naprezenia w betonie (obliczeniowe)
            //e - odkształcenie w betonie
            //Ec2 - odksztalcenie przy ktorym wyrkes zmienia sie z parabolicznego na prostokatny
            // fcd - wytrzymalosc na sciskanie betonu
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
            //funkcja wyznaczajaca naprezenia w betonie (charakterystyczna)
            //e - odkształcenie w betonie
            //Ec2 - odksztalcenie przy ktorym wyrkes zmienia sie z parabolicznego na prostokatny
            // fcd - wytrzymalosc na sciskanie betonu
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
            //funkcja okreslajaca naprezenie w stali w zaleznosci od odkształcenia (obliczeniowa)
            //e - odksztalcenie w stali
            // E - modul Young stali
            // fyd - granica plastycznosci stali
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
                //s = Es * fyd * (k - 1) / (Es * eud - fyd) * e + fyd * (1 - (k - 1) * fyd / (Es * eud - fyd));
            }

            return stress;
        }

        static public double SteelStressCharacteristic(double e, Steel steel)
        {
            //funkcja okreslajaca naprezenie w stali w zaleznosci od odkształcenia (charakterystyczna)
            //e - odksztalcenie w stali
            // E - modul Young stali
            // fyd - granica plastycznosci stali
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
                //s = Es * fyd * (k - 1) / (Es * eud - fyd) * e + fyd * (1 - (k - 1) * fyd / (Es * eud - fyd));
            }
            else
            {
                //zerwanie stali, odksztlacenie rowne 0
                s = 0;
            }
            return s;
        }
    }
}
