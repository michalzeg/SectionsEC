﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using SectionsEC.WindowClasses;
using SectionsEC.Dimensioning;
using SectionsEC.Calculations.Sections;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.Results;
using SectionsEC.Calculations.LoadCases;
using SectionsEC.Calculations.Materials;
using SectionsEC.Calculations.Extensions;
using SectionsEC.Dimensioning.Dimensioning;

namespace SectionsEC.ViewModel
{
    public class LeftPanelViewModel : ViewModelBase
    {
        public ConcreteViewModel ConcreteVM { get; private set; }
        public SteelViewModel SteelVM { get; private set; }
        public double CompressionCapacity { get; private set; }
        public double TensionCapacity { get; private set; }

        private IList<Bar> bars;
        private IList<PointD> sectionCoordinates;

        public LeftPanelViewModel()
        {
            ConcreteVM = new ConcreteViewModel();
            SteelVM = new SteelViewModel();

            this.sectionCapacityResults = new List<CalculationResults>();
            this.detailedSectionCapacityResults = new List<DetailedResult>();
            this.LoadCaseList = new ObservableCollection<LoadCase>();

            Messenger.Default.Register<IList<LoadCase>>(this, UpdateLoadCaseList);
            Messenger.Default.Register<IEnumerable<CalculationResults>>(this, UpdateResults);
            Messenger.Default.Register<IEnumerable<DetailedResult>>(this, UpdateDetailedResults);
            Messenger.Default.Register<Concrete>(this, UpdateConcrete);
            Messenger.Default.Register<Steel>(this, UpdateSteel);
            Messenger.Default.Register<ResultViewModelMessage>(this, UpdateResultsSender);
            Messenger.Default.Register<IDictionary<LoadCase, IEnumerable<InteractionCurveResult>>>(this, UpdateInteractionResults);
            Messenger.Default.Register<IList<PointD>>(this, UpdateSectionCoordinates);
            Messenger.Default.Register<IList<Bar>>(this, UpdateBars);
        }

        private void UpdateLoadCaseList(IEnumerable<LoadCase> loadCaseList)
        {
            this.LoadCaseList = loadCaseList.ToObservableCollection();

            this.SelectedLoadCase = loadCaseList.FirstOrDefault();

            RaisePropertyChanged(() => LoadCaseList);
        }

        private void UpdateResults(IEnumerable<CalculationResults> results)
        {
            this.sectionCapacityResults = results;
            this.sendResults(this.SelectedLoadCase);
        }

        private void UpdateDetailedResults(IEnumerable<DetailedResult> results)
        {
            this.detailedSectionCapacityResults = results;
            this.sendResults(this.SelectedLoadCase);
        }

        private void UpdateInteractionResults(IDictionary<LoadCase, IEnumerable<InteractionCurveResult>> results)
        {
            this.interactionResults = results;
            this.sendResults(this.SelectedLoadCase);
        }

        private void UpdateConcrete(Concrete concrete)
        {
            ConcreteVM.Concrete = concrete;
            CalculateAxialCapacity();
        }

        private void UpdateSteel(Steel steel)
        {
            SteelVM.Steel = steel;
            CalculateAxialCapacity();
        }

        private void UpdateBars(IList<Bar> bars)
        {
            this.bars = bars;
            CalculateAxialCapacity();
        }

        private void UpdateSectionCoordinates(IList<PointD> sectionCoordinates)
        {
            this.sectionCoordinates = sectionCoordinates;
            CalculateAxialCapacity();
        }

        private void UpdateResultsSender(ResultViewModelMessage message)
        {
            if (message == ResultViewModelMessage.InteractionCurveViewModel)
                this.sendResults = this.sendInteractionCurveResults;
            else if (message == ResultViewModelMessage.SectionCapacityViewModel)
                this.sendResults = this.sendSectionCapacityResults;
        }

        private void CalculateAxialCapacity()
        {
            if (bars != null && sectionCoordinates != null && ConcreteVM.Concrete != null && SteelVM.Steel != null)
            {
                this.TensionCapacity = AxialCapacity.TensionCapacity(bars, SteelVM.Steel);
                this.CompressionCapacity = AxialCapacity.CompressionCapacity(sectionCoordinates, ConcreteVM.Concrete);
                RaisePropertyChanged(() => TensionCapacity);
                RaisePropertyChanged(() => CompressionCapacity);
            }
        }

        private IEnumerable<DetailedResult> detailedSectionCapacityResults;
        private IEnumerable<CalculationResults> sectionCapacityResults;
        private IDictionary<LoadCase, IEnumerable<InteractionCurveResult>> interactionResults;

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
                    if (sendResults != null)
                        sendResults(value);
                }
            }
        }

        private Action<LoadCase> sendResults;

        private void sendInteractionCurveResults(LoadCase value)
        {
            IEnumerable<InteractionCurveResult> currentResult;
            if (interactionResults.TryGetValue(value, out currentResult))
            {
                Messenger.Default.Send(currentResult);
            }
        }

        private void sendSectionCapacityResults(LoadCase value)
        {
            CalculationResults currentResult = this.sectionCapacityResults.FirstOrDefault(e => e.LoadCase == value);
            if (currentResult != null)
            {
                Messenger.Default.Send(currentResult);
                Messenger.Default.Send(currentResult.CompressionZone, MessangerTokens.CompressionZoneDrawing);
            }
            var detailedResult = this.detailedSectionCapacityResults.FirstOrDefault(e => e.LoadCase == value);
            if (detailedResult != null)
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