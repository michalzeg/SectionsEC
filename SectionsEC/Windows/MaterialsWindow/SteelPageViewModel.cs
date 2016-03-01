using GalaSoft.MvvmLight;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.ViewModel
{
    public class SteelPageViewModel :ViewModelBase
    {
        public delegate void UpdatingEventHandler(Steel steel);
        public event UpdatingEventHandler UpdateSteel;

        private Steel selectedMaterial;
        public Steel SelectedMaterial
        {
            get
            {
                Steel steel = new Steel();
                steel.Es = Es;
                //steel.Eud = Eud;
                steel.Euk = Euk;
                steel.Fyk = Fyk;
                steel.EukToEud = EukToEud;
                steel.GammaS = GammaS;
                steel.K = K;
                steel.Grade = selectedMaterial.Grade;
                return steel;
                //return selectedMaterial;
            }
            set
            {
                if (value != selectedMaterial)
                {
                    selectedMaterial = value;
                    K = selectedMaterial.K;
                    Fyk = selectedMaterial.Fyk;
                    Fyd = selectedMaterial.Fyd;
                    Es = selectedMaterial.Es;
                    Euk = selectedMaterial.Euk;
                    Eud = selectedMaterial.Eud;
                    EukToEud = selectedMaterial.EukToEud;
                    GammaS = selectedMaterial.GammaS;
                    UpdateSteel(SelectedMaterial);
                    RaisePropertyChanged(() => SelectedMaterial);
                    
                }
            }
        }

        private double fyk;
        public double Fyk
        {
            get { return fyk; }
            set
            {
                if (value!= fyk)
                {
                    fyk = value;
                    Fyd = value / GammaS;
                    RaisePropertyChanged(() => Fyk);
                    UpdateSteel(SelectedMaterial);
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
                    UpdateSteel(SelectedMaterial);
                }
            }
        }
        private double gammaS;
        public double GammaS
        {
            get { return gammaS; }
            set
            {
                if (value!= gammaS)
                {
                    gammaS = value;
                    Fyd = Fyk / value;
                    RaisePropertyChanged(() => GammaS);
                    UpdateSteel(SelectedMaterial);
                }
            }
        }
        private double es;
        public double Es
        {
            get { return es; }
            set
            {
                if (value!=es)
                {
                    es = value;
                    RaisePropertyChanged(() => Es);
                    UpdateSteel(SelectedMaterial);
                }

            }
        }
        private double euk;
        public double Euk
        {
            get { return euk; }
            set
            {
                if (value!=euk)
                {
                    euk = value;
                    Eud = EukToEud * value;
                    RaisePropertyChanged(() => Euk);
                    UpdateSteel(SelectedMaterial);
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
                    UpdateSteel(SelectedMaterial);
                }

            }
        }
        private double eukToEud;
        public double EukToEud
        {
            get { return eukToEud; }
            set
            {
                if (value!=eukToEud)
                {
                    eukToEud = value;
                    Eud = value * Euk;
                    RaisePropertyChanged(() => EukToEud);
                    UpdateSteel(SelectedMaterial);
                }
            }
        }
        private double k;
        public double K
        {
            get { return k; }
            set
            {
                if (value !=k)
                {
                    k = value;
                    RaisePropertyChanged(() => K);
                    UpdateSteel(SelectedMaterial);
                }
            }
        }
    }
}
