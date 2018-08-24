using GalaSoft.MvvmLight;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.ViewModel
{
    public class ConcreteViewModel :ViewModelBase
    {
        public delegate void ConcreteUpdatedEventHandler();
        public event ConcreteUpdatedEventHandler ConcreteUpdated;

        public Concrete Concrete
        {
            get
            {
                Concrete concrete = new Concrete();
                concrete.Grade = Grade;
                concrete.Acc = Acc;
                concrete.Ec2 = Ec2;
                concrete.Ecu2 = Ecu2;
                concrete.Fck = Fck;
                concrete.Grade = Grade;
                concrete.N = N;
                concrete.GammaM = GammaM;
                return concrete;
            }
            set
            {
                Acc = value.Acc;
                Ec2 = value.Ec2;
                Ecu2 = value.Ecu2;
                Fck = value.Fck;
                Grade = value.Grade;
                N = value.N;
                GammaM = value.GammaM;
                Fcd = value.Fcd;
                Grade = value.Grade;
                if (ConcreteUpdated != null)
                    ConcreteUpdated();
            }
        }

        private string grade;
        public string Grade
        {
            get { return grade; }
            set
            {
                if (value!=grade)
                {
                    grade = value;
                    RaisePropertyChanged(() => Grade);
                    if (ConcreteUpdated != null)
                        ConcreteUpdated();
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
                    if (ConcreteUpdated != null)
                        ConcreteUpdated();
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
                    if (ConcreteUpdated != null)
                        ConcreteUpdated();
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
                    if (ConcreteUpdated != null)
                        ConcreteUpdated();
                }
            }
        }
        private double fcd;
        public double Fcd
        {
            get { return fcd; }
            set
            {
                if (value != fcd)
                {
                    fcd = value;
                    RaisePropertyChanged(() => Fcd);
                    if (ConcreteUpdated != null)
                        ConcreteUpdated();
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
                    if (ConcreteUpdated != null)
                        ConcreteUpdated();
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
                    if (ConcreteUpdated != null)
                        ConcreteUpdated(); 
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
                    if (ConcreteUpdated != null)
                        ConcreteUpdated();
                }
            }
        }
    }
}
