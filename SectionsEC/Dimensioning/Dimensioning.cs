using System;
using System.Linq;

using System.Collections.Generic;
using CommonMethods;
using SectionsEC.Helpers;
using SectionsEC.StressCalculations;



//all units in [m] and [kN]
namespace SectionsEC.Dimensioning
{

    public static class CapacityCalculator
    {
        public static Dictionary<LoadCase,CalculationResults> CalculateSectionCapacity(Concrete concrete,Steel steel,IList<PointD> sectionCoordinates,IList<Bar> bars,IList<LoadCase> loadCases)
        {
            var capacity = new SectionCapacity(concrete, steel);
            var section = new Section(sectionCoordinates);
            var resultDictionary = new Dictionary<LoadCase, CalculationResults>();
            foreach (var load in loadCases)
            {
                var result = capacity.CalculateCapacity(load.NormalForce, section, bars);
                resultDictionary.Add(load, result);
            }

            return resultDictionary;
        }
    }


    public class SectionCapacity
    {
        // equlibrium equation
        // Fc - Fs = 0;

        private ICompressionZoneCalculations compressionZoneCalculations;
        private IStrainCalculations strainCalculations;
        private IList<Reinforcement> reinforcement;
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
            foreach (var bar in this.reinforcement)
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
            
            for (int i = 0; i <= this.reinforcement.Count - 1; i++)
            {
                if (this.reinforcement[i].Bar.Y < yNeutralAxis)
                {
                    var di = this.reinforcement[i].D;
                    
                    var e = this.strainCalculations.StrainInAs1(x, di);
                    resultantForce = resultantForce + this.reinforcement[i].Bar.As * StressFunctions.SteelStressDesign(e, this.steel);
                    var barsTemp = this.reinforcement[i];
                    barsTemp.E = e;
                    barsTemp.IsCompressed = false;
                    this.reinforcement[i] = barsTemp;

                }
            }
            return resultantForce;
        }
        private double forceInAs2(double x) //funkcja wyliczajaca wypadkową w zbrojeniu sciskanym
        {
            double resultantForce = 0; //wartosc wypadkowej sily z zbrojeniu sciskanym
                
            double yNeutralAxis = this.section.MaxY - x; //wspolrzedna osi obojetnej
            
            for (int i = 0; i <= this.reinforcement.Count - 1; i++)
            {
                if (this.reinforcement[i].Bar.Y > yNeutralAxis)
                {
                    var di = this.reinforcement[i].D;
                    
                    var e = this.strainCalculations.StrainInAs2(x, di);
                    resultantForce = resultantForce + this.reinforcement[i].Bar.As * StressFunctions.SteelStressDesign(e, this.steel);
                    var barsTemp = this.reinforcement[i];
                    barsTemp.E = e;
                    barsTemp.IsCompressed = true;
                    this.reinforcement[i] = barsTemp;
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
        

        public CalculationResults CalculateCapacity(double nEd, Section section, IList<Bar> bars) //funkcja wyznaczajaca zasieg strefy sciskanej
        {
            this.section = section;
            this.strainCalculations = new StrainCalculations(this.concrete, this.steel, section);
            //this.compressionZoneCalculations = new CompressionZoneCalculationsGreenFormula(this.concrete, this.strainCalculations);
            if (this.concrete.N == 2d)
                this.compressionZoneCalculations = new CompressionZoneCalculationsGreenFormula(this.concrete, this.strainCalculations);
            else
                this.compressionZoneCalculations = new CompressionZoneCalculationsNumericalFormula(this.concrete, this.strainCalculations);

            createReinforcement(bars);

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
            result.Mrd = mrdReinforcement(result.X) + result.MrdConcrete - this.nEd * (this.section.H - this.section.Cz);

            //wyznaczenie obwiedni strefy sciskanej do wykresu

            result.CompressionZone = CompressionZoneCoordinates.CoordinatesOfCompressionZone(this.section.Coordinates, this.section.MaxY - result.X);

            result.Bars = this.reinforcement;
            //wyznaczenie maksymalnych odksztalcen w stali i betonie

            result.Ec = this.strainCalculations.StrainInConcrete(result.X, 0);

            return result;
        }

        private void createReinforcement(IList<Bar> bars)
        {
            this.reinforcement = new List<Reinforcement>();
            foreach (var bar in bars)
            {
                this.reinforcement.Add(new Reinforcement() { Bar = bar });
            }
        }

        private double mrdReinforcement(double x) //funkcja wyznacza moment (nośność) od zbrojenia
        {
            //moment jest obliczany wzgledem dolnej krawedzi przekroju (ymin)
            double Mrd = 0;
            double yOsi = this.section.MaxY - x;
            double Mz=0;
            Reinforcement barsTemp;
            for (int i = 0; i <= this.reinforcement.Count-1; i++)
            {
                //jeżeli zbrojenie znajduje się ponad osią obojętną to zbrojenie jest ściskane(+)
                //jeżeli pod osia obojętną to zbrojenie jest rozciąganie (-)
                barsTemp = this.reinforcement[i];
                if (this.reinforcement[i].Bar.Y > yOsi)
                {
                    //zbrojenie sciskane
                    Mz = reinforcement[i].Bar.As * StressFunctions.SteelStressDesign(reinforcement[i].E, this.steel) * (reinforcement[i].Bar.Y - this.section.MinY);
                    Mrd = Mrd + Mz;
                }
                else
                {
                    //zbrojenie rozciagane(-)
                    Mz = reinforcement[i].Bar.As * StressFunctions.SteelStressDesign(reinforcement[i].E, this.steel) * (reinforcement[i].Bar.Y - this.section.MinY);
                    Mrd = Mrd - Mz;
                }
                barsTemp.My = Mz;
                this.reinforcement[i] = barsTemp;

            }
            return Mrd;
        }
        
    }

    
    

    
}
