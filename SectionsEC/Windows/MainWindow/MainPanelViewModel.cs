using GalaSoft.MvvmLight;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.Extensions;
using GalaSoft.MvvmLight.Messaging;

namespace SectionsEC.ViewModel
{
    public class MainPanelViewModel :ViewModelBase
    {
        public LeftPanelViewModel LeftPanelVM { get; private set; }
        public CentralPanelViewModel CentralPanelVM { get; private set; }

        public MainPanelViewModel()
        {
            this.LoadCaseList = new ObservableCollection<LoadCase>();
            this.LeftPanelVM = new LeftPanelViewModel();
            this.CentralPanelVM = new CentralPanelViewModel();

            Messenger.Default.Register<Concrete>(this, (c) => CurrentConcrete = c);
            Messenger.Default.Register<Steel>(this, (s) => CurrentSteel = s);
            Messenger.Default.Register<IEnumerable<LoadCase>>(this, (l) => LoadCaseList = l.ToObservableCollection());

        }

        private Concrete currentConcrete;
        public Concrete CurrentConcrete
        {
            get { return currentConcrete; }
            set
            {
                if (value!=currentConcrete)
                {
                    currentConcrete = value;
                    RaisePropertyChanged(() => CurrentConcrete);
                }
            }
        }

        private Steel currentSteel;
        public Steel CurrentSteel
        {
            get { return currentSteel; }
            set
            {
                if (value!=currentSteel)
                {
                    currentSteel = value;
                    RaisePropertyChanged(() => CurrentSteel);
                }
            }
        }

        public ObservableCollection<LoadCase> LoadCaseList { get; set; }


        /*private void updateConcrete(Concrete concrete)
        {
            this.CurrentConcrete = concrete;
        }
        private void updateSteel(Steel steel)
        {
            this.CurrentSteel = steel;
        }
        private void updateLoadCaseList(IEnumerable<LoadCase> loadCases)
        {
            this.LoadCaseList = loadCases.ToObservableCollection<LoadCase>();
        }*/
    }
}
