using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Helpers;
using System.Collections.ObjectModel;

namespace SectionsEC.ViewModel
{
    public class LeftPanelViewModel :ViewModelBase
    {
        public LeftPanelViewModel()
        {
            this.NormalForce = 100d;

        }

        

        private double normalForce;
        public double NormalForce
        {
            get { return normalForce; }
            set
            {
                if (value != normalForce)
                {
                    normalForce = value;
                    RaisePropertyChanged(() => NormalForce);
                }
            }
        }

        

    }
}
