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
        public RightPanelViewModel RightPanelVM { get; private set; }

        public MainPanelViewModel()
        {
            this.LeftPanelVM = new LeftPanelViewModel();
            this.CentralPanelVM = new CentralPanelViewModel();
            this.RightPanelVM = new RightPanelViewModel();
        }
        

    }
}
