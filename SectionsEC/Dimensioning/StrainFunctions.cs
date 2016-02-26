using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.Dimensioning
{
    static class StrainFunctions //klasa obslugujaca obliczanie osksztalcen z zasady plaskuch przekrojów
    {
        //_1 funckcja odkształceń odnosząca się do maksymalnego odkształcenia w stali zbrojeniowej e promili
        // _2 funkcja odkształceń odnoszaca sie do maksymalnego odksztalcenia w betonie ec = 3,5 promili
        //_3 funckja odkształcen odnoszaca sie do odksztalcenia w betonie w odleglosci 3/4h, ma zastosowanie jedynie
        // gdy caly przekroj jest sciskany

        //esi - odkształcenie w i-tym precie rozciaganym
        //es2i - odkształcenie w i-tym precie scisaknym
        //ecmax - maksymalne odkształcenie w betonie

        //ec2Y - odległość włókna dla którego odkształcenie wynosi 2 promile od osi obojetnej

        //funkcje odkształceń dla _1 (max odkształcenie w zbrojeniu)
        static public class AssumedMaxStrainInSteel
        {
            static public double Esi(double di, double x, double d, double eud)
            {
                //di - wysokosc uzyteczna i-tego preta
                //d - maksymalna wysokosc uzyteczna (najbardziej wytezony pret)
                // x - wysokosc strefy sciskanej
                //es - maksymalne odksztalcenie w stali = 10 promili
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
                //es - maksymalne odkształcenie w stali
                //maksymalne odkształcenie w betonie
                double ecmax;
                ecmax = eud / (d - x) * x;
                return ecmax;
            }
            static public double Ec2Y(double d, double x, double eud, double ec2)
            {
                //odległość od osi obojętnej na której odkształcenia wynoszą 2 promile
                //Ec2 - odksztalcenie dla ktorego wykres zmienia sie z parabolciznego na prostokatny
                double ec2Y;
                ec2Y = (d - x) / eud * ec2;
                return ec2Y;
            }
            static public double Ec(double d, double x, double di,double eud)
            {
                double ec = eud / (d - x) * (x - di);
                return ec;
            }
        }
        //funkcje odkształceń dla _2 (max odkształcenie w betonie);
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
            static public double Ec(double x,double di, double ecu2)
            {
                double ec = ecu2 / x * (x - di);
                return ec;
            }
        }
        static public class AssumedMaxStrainIn37H
        {
            //funkcje odksztalcen dla _3 (max odksztalcenie w 3/7h )
            static public double Ecmax(double x, double ec2, double ecu2, double h)
            {
                double c = (1 - ec2 / ecu2) * h;//zmienna pomocnicza
                double ecmax;
                ecmax = ec2 / (x - c) * x;
                return ecmax;
            }
            static public double Es2i(double x, double di, double ec2, double ecu2, double h)
            {
                //odksztalcenie w zbojeniu sciskanym
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
            static public double Ec(double x, double di,double h, double ec2,double ecu2)
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
                //whole section is in compression

                //checking strain in 3/7h
                if (StrainFunctions.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    //strain in 3/7h greater than Ec2 => assumed strain in 3/7h Ec2
                    ec2Y = StrainFunctions.AssumedMaxStrainIn37H.Ec2Y(this.section.H, this.concrete.Ec2, this.concrete.Ecu2);
                }
                else
                {
                    //extreme fiber decides about capacity of the section
                    ec2Y = StrainFunctions.AssumedMaxStrainInConcrete.Ec2Y(x, this.concrete.Ec2, this.concrete.Ecu2);
                }

            }
            else
            {
                //section also in tension
                //chcecking strain in stretched reinforcement
                if (StrainFunctions.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    ec2Y = StrainFunctions.AssumedMaxStrainInSteel.Ec2Y(this.section.D, x, this.steel.Eud, this.concrete.Ec2);
                }
                else
                {
                    //extreme cocnrete fiber decides on section capacity
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
                //whole section in compression

                //checking strain in 3/7h
                if (StrainFunctions.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    //strain in 3/7h greater than Ec2 => assumed in 3/7h Ec2
                    e = StrainFunctions.AssumedMaxStrainIn37H.Es2i(x, di, this.concrete.Ec2, this.concrete.Ecu2, this.section.H);
                }
                else
                {
                    //extreme concrete fibre decides on strain
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Es2i(di, x, this.concrete.Ecu2);
                }
            }
            else
            {
                //section in also in tension
                //chcecking strain in reinforcement in tension
                if (StrainFunctions.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    e = StrainFunctions.AssumedMaxStrainInSteel.Es2i(di, x, this.section.D, this.steel.Eud);
                }
                else
                {
                    //extreme concrete fibre decides on strain
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
                //whole section is in compression, no tension reinforcement
                e = 0;
            }
            else
            {
                //section also in tension
                //chceck strain in most tensioned reinforcement
                if (StrainFunctions.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    //strain in tensioned reinforcement decindes on section capacity
                    e = StrainFunctions.AssumedMaxStrainInSteel.Esi(di, x, this.section.D, this.steel.Eud);
                }
                else
                {
                    //extreme fibre decides on section capacity
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Esi(di, x, this.concrete.Ecu2);
                }
            }
            return e;
        }
        public double StrainInConcrete(double x,double di)
        {

            //di - distance from most compressed fibre to calculation point of concrete strain
            double e;
            if (x > this.section.D)
            {
                //whole section in compression
                
                //checking strain in 3/7h
                if (StrainFunctions.AssumedMaxStrainInConcrete.E37h(x, this.concrete.Ec2, this.concrete.Ecu2, this.section.H) > this.concrete.Ec2)
                {
                    //strain in 3/7h greater than Ec2 => assumed in 3/7h Ec2
                    e = StrainFunctions.AssumedMaxStrainIn37H.Ec(x, di, section.H, concrete.Ec2, concrete.Ecu2);
                }
                else
                {
                    //extreme concrete fibre decides on strain
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Ec(x, di, concrete.Ecu2);
                }
            }
            else
            {
                //section in also in tension
                //chcecking strain in reinforcement in tension
                if (StrainFunctions.AssumedMaxStrainInConcrete.Esi(this.section.D, x, this.concrete.Ecu2) > this.steel.Eud)
                {
                    e = StrainFunctions.AssumedMaxStrainInSteel.Ec(section.D, x, di, steel.Eud);
                }
                else
                {
                    //extreme concrete fibre decides on strain
                    e = StrainFunctions.AssumedMaxStrainInConcrete.Ec(x, di, concrete.Ecu2);
                }
            }

            return e;
        }

        
    }

}
