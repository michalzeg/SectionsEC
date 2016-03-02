using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Helpers;
using System.Collections.ObjectModel;
using SectionsEC.Extensions;

namespace SectionsEC.ViewModel
{
    public class LeftPanelViewModel :ViewModelBase
    {
        public LeftPanelViewModel()
        {
            this.LoadCaseList = new ObservableCollection<LoadCase>();
            Messenger.Default.Register<IEnumerable<LoadCase>>(this, updateLoadCaseList);

        }

        private void updateLoadCaseList(IEnumerable<LoadCase> loadCaseList)
        {
            this.LoadCaseList = loadCaseList.ToObservableCollection();
            RaisePropertyChanged(() => LoadCaseList);

            this.SelectedLoadCase = loadCaseList.FirstOrDefault();
            //RaisePropertyChanged(() => SelectedLoadCase);

        }
        public ObservableCollection<LoadCase> LoadCaseList { get; set; }

        private LoadCase selectedLoadCase;
        public LoadCase SelectedLoadCase
        {
            get { return selectedLoadCase; }
            set
            {
                if (value != selectedLoadCase)
                {
                    selectedLoadCase = value;
                    this.NormalForce = value.NormalForce;
                    RaisePropertyChanged(() => SelectedLoadCase);
                }
            }
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
