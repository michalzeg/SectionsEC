using GalaSoft.MvvmLight;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.ViewModel
{
    public class SteelViewModel:ViewModelBase
    {
        public delegate void SteelUpdatedEventHandler();
        public event SteelUpdatedEventHandler SteelUpdated;

        public SteelViewModel() { }

        public Steel Steel
        {
            get
            {
                Steel steel = new Steel();
                steel.Es = Es;
                steel.Euk = Euk;
                steel.EukToEud = EukToEud;
                steel.Fyk = Fyk;
                steel.GammaS = GammaS;
                steel.Grade = Grade;
                steel.K = K;
                return steel;
            }
            set
            {
                Es = value.Es;
                Eud = value.Eud;
                Euk = value.Euk;
                EukToEud = value.EukToEud;
                Fyd = value.Fyd;
                Fyk = value.Fyk;
                GammaS = value.GammaS;
                Grade = value.Grade;
                K = value.K;
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
                    SteelUpdated();
                }
            }
        }
        private double fyk;
        public double Fyk
        {
            get { return fyk; }
            set
            {
                if (value != fyk)
                {
                    fyk = value;
                    Fyd = value / GammaS;
                    RaisePropertyChanged(() => Fyk);
                    SteelUpdated();
                }
            }
        }
        private double fyd;
        public double Fyd
        {
            get { return fyd; }
            set
            {
                if (value != fyd)
                {
                    fyd = value;
                    RaisePropertyChanged(() => Fyd);
                    SteelUpdated();
                }
            }
        }
        private double gammaS;
        public double GammaS
        {
            get { return gammaS; }
            set
            {
                if (value != gammaS)
                {
                    gammaS = value;
                    Fyd = Fyk / value;
                    RaisePropertyChanged(() => GammaS);
                    SteelUpdated();
                }
            }
        }
        private double es;
        public double Es
        {
            get { return es; }
            set
            {
                if (value != es)
                {
                    es = value;
                    RaisePropertyChanged(() => Es);
                    SteelUpdated();
                }

            }
        }
        private double euk;
        public double Euk
        {
            get { return euk; }
            set
            {
                if (value != euk)
                {
                    euk = value;
                    Eud = EukToEud * value;
                    RaisePropertyChanged(() => Euk);
                    SteelUpdated();
                }
            }
        }
        private double eud;
        public double Eud
        {
            get { return eud; }
            set
            {
                if (value != eud)
                {
                    eud = value;
                    RaisePropertyChanged(() => Eud);
                    SteelUpdated();
                }

            }
        }
        private double eukToEud;
        public double EukToEud
        {
            get { return eukToEud; }
            set
            {
                if (value != eukToEud)
                {
                    eukToEud = value;
                    Eud = value * Euk;
                    RaisePropertyChanged(() => EukToEud);
                    SteelUpdated();
                }
            }
        }
        private double k;
        public double K
        {
            get { return k; }
            set
            {
                if (value != k)
                {
                    k = value;
                    RaisePropertyChanged(() => K);
                    SteelUpdated();
                }
            }
        }
    }
}
