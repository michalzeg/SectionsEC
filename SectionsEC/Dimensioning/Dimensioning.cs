using System;
using System.Linq;

using System.Collections.Generic;
using CommonMethods;
using SectionsEC.Helpers;
using SectionsEC.StressCalculations;



//all units in [m] and [kN]
namespace SectionsEC.Dimensioning
{
    /*public class Concrete
    {
        public string Grade { get; set; } 
        public double Fck { get; set; }
        public double Acc { get; set; }
        public double GammaM { get; set; }
        public double N { get; set; }
        public double Ec2 { get; set; }
        public double Ecu2 { get; set; } 
        public double Fcd 
        {
            get
            {
                return Acc * Fck/GammaM;
            }
        }
    }
    public class Steel
    {
        public string Grade { get; set; }
        public double Fyk { get; set; }
        public double GammaS { get; set; }
        public double K { get; set; }
        public double Es { get; set; }
        public double Euk { get; set; }
        public double EudToEuk { get; set; }
        public double Fyd 
        {
            get
            {
                return Fyk / GammaS;
            }
        }
        public double Eud 
        {
            get
            {
                return Euk * EudToEuk;
            }
        }
    }*/
    
    public class Reinforcement
    {
        public double E { get; set; } //odkształcenie w zbrojeniu
        public Bar Bar { get; set; }
        public double D { get; set; } //odległość zbrojenia od krawędzi najbardziej ściskanej (wysokość użytkowa dla danego pręta)
        public double Mz { get; set; } //moment od zbrojenia
        
        public bool IsCompressed { get; set; }//okresla czy zbrojenie jest sciskane czy rozciagane
        
    }
    public class Section:IIntegrable
    {
        public IList<PointD> Coordinates { get; private set;} //wspolrzedne przekroju
        public double D { get; set; }      //wysokosc uzyteczna przekroju
        public double MaxY { get; private set; } //najwieksza wspolrzedna y
        public double MinY { get; private set; } // najmniejsza wsplrzedna y
        public double H { get; private set; } //wysokosc przekroju
        public double B { get; private set; }//szerokosc przekroju;
        public double Cz { get; private set; }// odleglosc srodka ciezkosci od najbardziej sciskanego wlokna
        public double IntegrationPointY { get; set; }

        public Section(IList<PointD> coordinates)
        {
            Coordinates = checkIfCoordinatesAreClockwise(coordinates);

            calculateExtrementsAndDepth();
            Cz = SectionProperties.Cz(Coordinates, MaxY);
            IntegrationPointY = MinY;
        }
        
        private IList<PointD> checkIfCoordinatesAreClockwise(IList<PointD> coordinates) //procedura sprawdza czy wspolrzedne przekroju sa wprowadzone zgodnie ze wskazowkami zegara
        {
            //procedura bierze dwa pierwsze punkty i liczy iloczyn wektorowy. Jezeli wynik jest dodatni(wspolrzedna "z" to układ jest prawoskretny
            // co oznacza ze wspolrzedne wprowadzone sa przeciwnie do ruchu wskazowek zegara
            //jesli iloczyn wektorowy jest rowny 0 tzn ze wektory sa rownolegle ->nalezy wziasc kolejny punkt
            double iw; //iloczyn wektorowy
            try
            {
                for (int i = 0; i <= coordinates.Count - 3; i++)
                {
                    iw = crossProduct(coordinates[i], coordinates[i + 1], coordinates[i + 2]);
                    if (iw > 0)
                    {
                        //uklad prawoskretny
                        break;
                    }
                    else if (iw < 0)
                    {
                        //uklad lewoskretny, nalezy odwrocic wspolrzedne
                        coordinates.Reverse();
                        break;
                    }
                    else
                    {
                        //dwa rownolegle wektory, nie rob nic, wez kolejne punkty
                    }
                }
            }
            catch
            {
            }

            return coordinates;
        }

        private double crossProduct(PointD p0, PointD p1, PointD p2)//funckja oblicza iloczyn wektorowy
        {
            double[] vector1 = new double[2]; //wspolrzedne wektora 1 (0 - X, 1 - y) 
            double[] vector2 = new double[2];//wspolrzedne wektora 2

            vector1[0] = p1.X - p0.X;
            vector1[1] = p1.Y - p0.Y;
            vector2[0] = p2.X - p1.X;
            vector2[1] = p2.Y - p1.Y;

            double wynik; //ax*by-ay*bz
            wynik = vector1[0] * vector2[1] - vector1[1] * vector2[0];
            return wynik;
        }
        private void calculateExtrementsAndDepth() //procedura wyznacza maksymalne wartosci wspolrzdnej y oraz wysokosc przekroju
        {
            /*double[] taby = new double[Coordinates.Count];//pomocnicza tablica
            double[] tabx = new double[Coordinates.Count];
            for (int i = 0; i < Coordinates.Count; i++)
            {
                taby[i] = Coordinates[i].Y;
                tabx[i] = Coordinates[i].X;
            }*/
            MinY = this.Coordinates.Min(p => p.Y);
            MaxY = this.Coordinates.Max(p => p.Y);
            H = MaxY - MinY; //wysokosc
        }
    }

    

    public class CalculationResults
    {
        //opisuje wszystkie wyniki
        public double Mrd { get; set; } //nosnosc
        public double X { get; set; } //zasieg strefy sciskanej
        public double Ec { get; set; } //odksztalcenie w betonie
        public double Es { get; set; } //odksztalcenie w stali
        public IList<PointD> CompressionZone { get; set; }
        public double D { get; set; } //wysokosc uzyteczna przekroju
        public double MrdConcrete { get; set; }//nosnosc ze wzgledu na beton
        public double ForceConcrete { get; set; }// sila w betonie
        public IEnumerable<Reinforcement> Bars { get; set; } //wyniki dla zbrojenia
    }
    public class SectionCapacity
    {
        // equlibrium equation
        // Fc - Fs = 0;

        private ICompressionZoneCalculations compressionZoneCalculations;
        private IStrainCalculations strainCalculations;
        private IList<Reinforcement> bars;
        private Concrete concrete; //wlasciwosci betonu
        private Steel steel; //wslasciwosci stali
        private Section section; //wlasciwosci przekroju
        private double nEd;

        public SectionCapacity(Concrete concrete, Steel steel)
        {
            this.concrete = concrete;
            this.steel = steel;
        }
        private double calculateEffectiveDepthOfSectionAndBars()
        {

            /*Reinforcement barsTemp;

            double[] tab = new double[this.bars.Count];
            for (int i = 0; i <= this.bars.Count - 1; i++)
            {
                barsTemp = this.bars[i];
                barsTemp.D = this.section.MaxY - this.bars[i].Y;
                this.bars[i] = barsTemp;
                tab[i] = barsTemp.D;
            }*/

            var tempD = new List<double>();
            foreach (var bar in this.bars)
            {
                double d = section.MaxY - bar.Bar.Y;
                tempD.Add(d);
                bar.D = d;
            }
            return tempD.Max();
        }
        private double equlibriumEquation(double x) //równanie równowagi sily w zbrojeniu i w betonie, funkcja ktora bedzie rozwiazywana
        {
            //Rownianie równowagi
            // Fc(x) + Fs2(x) - Fs1(x) - Nsd = 0;
            //Fc(x) - resultant force in concrete
            // Fs2(x) - resultant force in compressed reinforcement
            // Fs1(x) - resultant force in tension reinforcement
            // Nsd - normal force, + compression, -tension

            
            double forceInConcrete = this.forceInConcrete(x);
            double forceInAs1 = this.forceInAs1(x);
            double forceInAs2 = this.forceInAs2(x);
            double result = forceInConcrete + forceInAs2 - forceInAs1 - this.nEd;
            return result;
        }

        private double forceInAs1(double x) //funkcja wyliczajaca wypadkowa w zbrojeniu rozciaganym
        {
            double resultantForce = 0; //wartosc wypadkowej sily z zbrojeniu rozciaganym
            
            double yNeutralAxis = this.section.MaxY - x; //wspolrzedna osi obojetnej
            
            for (int i = 0; i <= this.bars.Count - 1; i++)
            {
                if (this.bars[i].Bar.Y < yNeutralAxis)
                {
                    var di = this.bars[i].D;
                    
                    var e = this.strainCalculations.StrainInAs1(x, di);
                    resultantForce = resultantForce + this.bars[i].Bar.As * StressFunctions.SteelStressDesign(e, this.steel);
                    var barsTemp = this.bars[i];
                    barsTemp.E = e;
                    barsTemp.IsCompressed = false;
                    this.bars[i] = barsTemp;

                }
            }
            return resultantForce;
        }
        private double forceInAs2(double x) //funkcja wyliczajaca wypadkową w zbrojeniu sciskanym
        {
            double resultantForce = 0; //wartosc wypadkowej sily z zbrojeniu sciskanym
                
            double yNeutralAxis = this.section.MaxY - x; //wspolrzedna osi obojetnej
            
            for (int i = 0; i <= this.bars.Count - 1; i++)
            {
                if (this.bars[i].Bar.Y > yNeutralAxis)
                {
                    var di = this.bars[i].D;
                    
                    var e = this.strainCalculations.StrainInAs2(x, di);
                    resultantForce = resultantForce + this.bars[i].Bar.As * StressFunctions.SteelStressDesign(e, this.steel);
                    var barsTemp = this.bars[i];
                    barsTemp.E = e;
                    barsTemp.IsCompressed = true;
                    this.bars[i] = barsTemp;
                }
            }
            return resultantForce;
        }
        private double forceInConcrete(double x) //funkcja wylcizajaca wypadkowa w betonie
        {
            var result = this.compressionZoneCalculations.Calculate(x, this.section);

            return result.NormalForce;
            

        }
        private double solveEqulibriumEquation() //rozwiazywanie rowniaina rownowagi metoda polowienia, zgodnie z wikipedia
        {
            double EPS = 0.00000000001; //dokladnosc poszukwiania wyniku
        
            double fL,fR,fM; //wartosc rownania rownowagi w 3 punktach przedzialu


            double xL =0.000001*this.section.H; //granica lewa przedzialu

            double xR =10*this.section.H; //granica prawa przedzialu, strefa sciskana nie moze byc wieksza od h
            double xM = (xL+xR)/2;
            double x0;
            int k = 0;
            while ((Math.Abs(xL - xR) > EPS) && (k <10000))
            {
                k++;
                xM = (xR + xL) / 2; 
                fL = this.equlibriumEquation(xL);
                fR = this.equlibriumEquation(xR);
                fM = this.equlibriumEquation(xM);
                if (fL * fM < 0)
                {
                    xR = xM;
                }
                if (fR * fM < 0)
                {
                    xL = xM;
                }
            }
            if (k > 1000)
            {
                x0 = double.NaN;
            }
            else
            {
                x0 = (xR + xL) / 2;
            }
            
            return x0;
        }
        

        public CalculationResults CalculateCapacity(double nEd, Section section, IList<Reinforcement> reinforcement) //funkcja wyznaczajaca zasieg strefy sciskanej
        {
            this.section = section;
            this.strainCalculations = new StrainCalculations(this.concrete, this.steel, section);
            //this.compressionZoneCalculations = new CompressionZoneCalculationsGreenFormula(this.concrete, this.strainCalculations);
            if (this.concrete.N == 2d)
                this.compressionZoneCalculations = new CompressionZoneCalculationsGreenFormula(this.concrete, this.strainCalculations);
            else
                this.compressionZoneCalculations = new CompressionZoneCalculationsNumericalFormula(this.concrete, this.strainCalculations);

            this.bars = reinforcement;
            
            this.section.D = this.calculateEffectiveDepthOfSectionAndBars();
            this.nEd = nEd;
            CalculationResults result = new CalculationResults();
            
            result.D = this.section.D;
            result.X = this.solveEqulibriumEquation();
            if (double.IsNaN(result.X))
            {
                return result; //przerwanie kiedy brak wyniku
            }

            //wyznaczanie wypadkowej sily w betonie

            var forces = this.compressionZoneCalculations.Calculate(result.X, this.section);

            result.MrdConcrete = forces.Moment; //moment wzgledem dolnej krawedzi
            result.ForceConcrete = forces.NormalForce;
            result.Mrd = mrdReinforcement(result.X) + result.MrdConcrete - this.nEd * (this.section.H -this.section.Cz);
            
            //wyznaczenie obwiedni strefy sciskanej do wykresu

            result.CompressionZone = CompressionZoneCoordinates.CoordinatesOfCompressionZone(this.section.Coordinates, this.section.MaxY - result.X);
            
            result.Bars = this.bars;
            //wyznaczenie maksymalnych odksztalcen w stali i betonie

            result.Ec = this.strainCalculations.StrainInConcrete(result.X, 0);
      
            return result;
        }
        private double mrdReinforcement(double x) //funkcja wyznacza moment (nośność) od zbrojenia
        {
            //moment jest obliczany wzgledem dolnej krawedzi przekroju (ymin)
            double Mrd = 0;
            double yOsi = this.section.MaxY - x;
            double Mz=0;
            Reinforcement barsTemp;
            for (int i = 0; i <= this.bars.Count-1; i++)
            {
                //jeżeli zbrojenie znajduje się ponad osią obojętną to zbrojenie jest ściskane(+)
                //jeżeli pod osia obojętną to zbrojenie jest rozciąganie (-)
                barsTemp = this.bars[i];
                if (this.bars[i].Bar.Y > yOsi)
                {
                    //zbrojenie sciskane
                    Mz = bars[i].Bar.As * StressFunctions.SteelStressDesign(bars[i].E, this.steel) * (bars[i].Bar.Y - this.section.MinY);
                    Mrd = Mrd + Mz;
                }
                else
                {
                    //zbrojenie rozciagane(-)
                    Mz = bars[i].Bar.As * StressFunctions.SteelStressDesign(bars[i].E, this.steel) * (bars[i].Bar.Y - this.section.MinY);
                    Mrd = Mrd - Mz;
                }
                barsTemp.Mz = Mz;
                this.bars[i] = barsTemp;

            }
            return Mrd;
        }
        
    }

    
    

    
}
