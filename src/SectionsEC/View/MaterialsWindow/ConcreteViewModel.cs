using GalaSoft.MvvmLight;
using SectionsEC.Common.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.ViewModel
{
    public class ConcreteViewModel : ViewModelBase
    {
        public delegate void ConcreteUpdatedEventHandler();

        public event ConcreteUpdatedEventHandler ConcreteUpdated;

        public Concrete Concrete
        {
            get
            {
                Concrete concrete = new Concrete
                {
                    Grade = Grade,
                    Acc = Acc,
                    Ec2 = Ec2,
                    Ecu2 = Ecu2,
                    Fck = Fck
                };
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
                ConcreteUpdated?.Invoke();
            }
        }

        private string grade;

        public string Grade
        {
            get { return grade; }
            set
            {
                if (value != grade)
                {
                    grade = value;
                    RaisePropertyChanged(() => Grade);
                    ConcreteUpdated?.Invoke();
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
                    ConcreteUpdated?.Invoke();
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
                    ConcreteUpdated?.Invoke();
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
                    ConcreteUpdated?.Invoke();
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
                    ConcreteUpdated?.Invoke();
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
                    ConcreteUpdated?.Invoke();
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
                    ConcreteUpdated?.Invoke();
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
                    ConcreteUpdated?.Invoke();
                }
            }
        }
    }
}