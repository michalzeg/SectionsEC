﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Helpers;
using System.Collections.ObjectModel;
using SectionsEC.Extensions;
using SectionsEC.WindowClasses;

namespace SectionsEC.ViewModel
{
    public class LeftPanelViewModel :ViewModelBase
    {
        public ConcreteViewModel ConcreteVM { get; private set; }
        public SteelViewModel SteelVM { get; private set; }

        public LeftPanelViewModel()
        {
            ConcreteVM = new ConcreteViewModel();
            SteelVM = new SteelViewModel();

            this.results = new Dictionary<LoadCase, CalculationResults>();
            this.detailedResults = new Dictionary<LoadCase, StringBuilder>();
            this.LoadCaseList = new ObservableCollection<LoadCase>();
            Messenger.Default.Register<IList<LoadCase>>(this, updateLoadCaseList);
            Messenger.Default.Register<IDictionary<LoadCase, CalculationResults>>(this, updateResults);
            Messenger.Default.Register<IDictionary<LoadCase, StringBuilder>>(this, updateDetailedResults);
            Messenger.Default.Register<Concrete>(this, updateConcrete);
            Messenger.Default.Register<Steel>(this, updateSteel);
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
        private void updateConcrete(Concrete concrete)
        {
            ConcreteVM.Concrete = concrete;
        }
        private void updateSteel(Steel steel)
        {
            SteelVM.Steel = steel;
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
            {
                Messenger.Default.Send(currentResult);
                Messenger.Default.Send(currentResult.CompressionZone, MessangerTokens.CompressionZoneDrawing);
            }
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
