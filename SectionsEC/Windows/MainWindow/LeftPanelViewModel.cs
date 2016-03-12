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
            this.results = new Dictionary<LoadCase, CalculationResults>();
            this.detailedResults = new Dictionary<LoadCase, StringBuilder>();
            this.LoadCaseList = new ObservableCollection<LoadCase>();
            Messenger.Default.Register<IList<LoadCase>>(this, updateLoadCaseList);
            Messenger.Default.Register<IDictionary<LoadCase, CalculationResults>>(this, updateResults);
            Messenger.Default.Register<IDictionary<LoadCase, StringBuilder>>(this, updateDetailedResults);
        }

        private void updateLoadCaseList(IEnumerable<LoadCase> loadCaseList)
        {
            this.LoadCaseList = loadCaseList.ToObservableCollection();
            //

            this.SelectedLoadCase = loadCaseList.FirstOrDefault();
            //RaisePropertyChanged(() => SelectedLoadCase);
            RaisePropertyChanged(() => LoadCaseList);

        }
        private void updateResults(IDictionary<LoadCase,CalculationResults> results)
        {
            this.results = results;
            this.sendResults(this.SelectedLoadCase);
        }
        private void updateDetailedResults(IDictionary<LoadCase, StringBuilder> results)
        {
            this.detailedResults = results;
            this.sendResults(this.SelectedLoadCase);
        }

        private IDictionary<LoadCase, StringBuilder> detailedResults;
        private IDictionary<LoadCase, CalculationResults> results;

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
                    sendResults(value);

                }
            }
        }

        private void sendResults(LoadCase value)
        {
            CalculationResults currentResult;
            if (results.TryGetValue(value, out currentResult))
                Messenger.Default.Send(currentResult);
            StringBuilder detailedResult;
            if (detailedResults.TryGetValue(value, out detailedResult))
                Messenger.Default.Send(detailedResult);

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
