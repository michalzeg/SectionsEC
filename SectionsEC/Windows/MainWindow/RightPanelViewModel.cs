using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.Extensions;
using SectionsEC.Windows.WindowClasses;

namespace SectionsEC.ViewModel
{
    public class RightPanelViewModel :ViewModelBase
    {
        public RightPanelViewModel()
        {
            Messenger.Default.Register<CalculationResults>(this,updateResult);
            Messenger.Default.Register<StringBuilder>(this, updateDetailedResult);
        }

        public string Capacity { get; private set; }

        public string DetailedResults { get; private set; }

        private void updateResult(CalculationResults result)
        {
            Capacity = result.Mrd.ToFormatedString();
            RaisePropertyChanged(() => Capacity);
        }
        private void updateDetailedResult(StringBuilder text)
        {
            this.DetailedResults = text.ToString();
            RaisePropertyChanged(() => DetailedResults);
        }
        
    }
}
