using GalaSoft.MvvmLight;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.StressCalculations;

namespace SectionsEC.ViewModel

{
    public class ConcretePageViewModel:ViewModelBase
    {
        public ConcretePageViewModel()
        {
        }

        public delegate void UpdatingConcreteEventHandler(Concrete concrete);
        public event UpdatingConcreteEventHandler UpdateConcrete;

        private Concrete selectedMaterial;
        public Concrete SelectedMaterial
        {
            get
            {
                Concrete concrete = new Concrete();
                concrete.Acc = Acc;
                concrete.Ec2 = Ec2;
                concrete.Ecu2 = Ecu2;
                concrete.Fck = Fck;
                concrete.N = N;
                concrete.GammaM = GammaM;
                return concrete;
                //return selectedMaterial;
            }
            set
            {
                if (value !=selectedMaterial)
                {
                    selectedMaterial = value;
                    Fck = selectedMaterial.Fck;
                    GammaM = selectedMaterial.GammaM;
                    Acc = selectedMaterial.Acc;
                    Fcd = selectedMaterial.Fcd;
                    N = selectedMaterial.N;
                    Ec2 = selectedMaterial.Ec2;
                    Ecu2 = selectedMaterial.Ecu2;
                    UpdateConcrete(SelectedMaterial);
                    RaisePropertyChanged(() => SelectedMaterial);
                }
            }
        }
        

        private double fck;
        public double Fck
        {
            get { return fck; }
            set
            {
                if (value != fck)
                {
                    fck = value;
                    Fcd = Acc * value / GammaM;
                    RaisePropertyChanged(() => Fck);
                    UpdateConcrete(SelectedMaterial);
                }
            }
        }
        private double gammaM;
        public double GammaM
        {
            get { return gammaM; }
            set
            {
                if (value != gammaM)
                {
                    gammaM = value;
                    Fcd = Acc * Fck / value;
                    RaisePropertyChanged(() => GammaM);
                    UpdateConcrete(SelectedMaterial);
                }
            }
        }
        private double acc;
        public double Acc
        {
            get { return acc; }
            set
            {
                if (value != acc)
                {
                    acc = value;
                    Fcd = value * Fck / GammaM;
                    RaisePropertyChanged(() => Acc);
                    UpdateConcrete(SelectedMaterial);
                }
            }
        }
        private double fcd;
        public double Fcd
        {
            get { return fcd; }
            set
            {
                if (value !=fcd)
                {
                    fcd = value;
                    RaisePropertyChanged(() => Fcd);
                    UpdateConcrete(SelectedMaterial);
                }
            }
        }
        private double n;
        public double N
        {
            get { return n; }
            set
            {
                if (value != n)
                {
                    n = value;
                    RaisePropertyChanged(() => N);
                    UpdateConcrete(SelectedMaterial);
                }
            }
        }
        private double ec2;
        public double Ec2
        {
            get { return ec2; }
            set
            {
                if (value != ec2)
                {
                    ec2 = value;
                    RaisePropertyChanged(() => Ec2);
                    UpdateConcrete(SelectedMaterial);
                }
            }
        }
        private double ecu2;
        public double Ecu2
        {
            get { return ecu2; }
            set
            {
                if (value != ecu2)
                {
                    ecu2 = value;
                    RaisePropertyChanged(() => Ecu2);
                    UpdateConcrete(SelectedMaterial);
                }
            }
        }
    }
}
