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
using SectionsEC.WindowClasses;

namespace SectionsEC.ViewModel
{
    public class MainPanelViewModel : ViewModelBase
    {
        public LeftPanelViewModel LeftPanelVM { get; private set; }
        public CentralPanelViewModel CentralPanelVM { get; private set; }
        public SectionCapacityResultViewModel SectionCapacityResultVM { get; private set; }
        public InteractionCurvePageViewModel InteractionCurveResultVM { get; private set; }

        public ViewModelBase ResultViewModel { get; private set; }

        public MainPanelViewModel()
        {
            this.LeftPanelVM = new LeftPanelViewModel();
            this.CentralPanelVM = new CentralPanelViewModel();
            this.SectionCapacityResultVM = new SectionCapacityResultViewModel();
            this.InteractionCurveResultVM = new InteractionCurvePageViewModel();

            Messenger.Default.Register<ResultViewModelMessage>(this, changeResultViewModel);
        }

        private void changeResultViewModel(ResultViewModelMessage message)
        {
            if (message == ResultViewModelMessage.InteractionCurveViewModel)

                this.ResultViewModel = InteractionCurveResultVM;
            else if (message == ResultViewModelMessage.SectionCapacityViewModel)
                this.ResultViewModel = SectionCapacityResultVM;

            RaisePropertyChanged(() => ResultViewModel);
        }
    }
}