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
using SectionsEC.WindowClasses;
using SectionsEC.Dimensioning;

namespace SectionsEC.ViewModel
{
    public class LeftPanelViewModel :ViewModelBase
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

            this.sectionCapacityResults = new Dictionary<LoadCase, CalculationResults>();
            this.detailedSectionCapacityResults = new Dictionary<LoadCase, StringBuilder>();
            this.LoadCaseList = new ObservableCollection<LoadCase>();

            Messenger.Default.Register<IList<LoadCase>>(this, updateLoadCaseList);
            Messenger.Default.Register<IDictionary<LoadCase, CalculationResults>>(this, updateResults);
            Messenger.Default.Register<IDictionary<LoadCase, StringBuilder>>(this, updateDetailedResults);
            Messenger.Default.Register<Concrete>(this, updateConcrete);
            Messenger.Default.Register<Steel>(this, updateSteel);
            Messenger.Default.Register<ResultViewModelMessage>(this, updateResultsSender);
            Messenger.Default.Register<IDictionary<LoadCase, IEnumerable<InteractionCurveResult>>>(this, updateInteractionResults);
            Messenger.Default.Register<IList<PointD>>(this, updateSectionCoordinates);
            Messenger.Default.Register<IList<Bar>>(this, updateBars);
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
            this.sectionCapacityResults = results;
            this.sendResults(this.SelectedLoadCase);
        }
        private void updateDetailedResults(IDictionary<LoadCase, StringBuilder> results)
        {
            this.detailedSectionCapacityResults = results;
            this.sendResults(this.SelectedLoadCase);
        }
        private void updateInteractionResults(IDictionary<LoadCase,IEnumerable<InteractionCurveResult>> results)
        {
            this.interactionResults = results;
            this.sendResults(this.SelectedLoadCase);
        }
        private void updateConcrete(Concrete concrete)
        {
            ConcreteVM.Concrete = concrete;
            calculateAxialCapacity();
        }
        private void updateSteel(Steel steel)
        {
            SteelVM.Steel = steel;
            calculateAxialCapacity();
        }
        private void updateBars(IList<Bar> bars)
        {
            this.bars = bars;
            calculateAxialCapacity();
        }
        private void updateSectionCoordinates(IList<PointD> sectionCoordinates)
        {
            this.sectionCoordinates = sectionCoordinates;
            calculateAxialCapacity();
        }
        private void updateResultsSender(ResultViewModelMessage message)
        {
            if (message == ResultViewModelMessage.InteractionCurveViewModel)
                this.sendResults = this.sendInteractionCurveResults;
            else if (message == ResultViewModelMessage.SectionCapacityViewModel)
                this.sendResults = this.sendSectionCapacityResults;
        }
        
        private void calculateAxialCapacity()
        {
            if (bars !=null && sectionCoordinates!=null && ConcreteVM.Concrete!= null && SteelVM.Steel !=null)
            {
                this.TensionCapacity = AxialCapacity.TensionCapacity(bars, SteelVM.Steel);
                this.CompressionCapacity = AxialCapacity.CompressionCapacity(sectionCoordinates, ConcreteVM.Concrete);
                RaisePropertyChanged(() => TensionCapacity);
                RaisePropertyChanged(() => CompressionCapacity);
            }

        }

        private IDictionary<LoadCase, StringBuilder> detailedSectionCapacityResults;
        private IDictionary<LoadCase, CalculationResults> sectionCapacityResults;
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
            if (interactionResults.TryGetValue(value,out currentResult))
            {
                Messenger.Default.Send(currentResult);
            }

        }
        private void sendSectionCapacityResults(LoadCase value)
        {
            CalculationResults currentResult;
            if (sectionCapacityResults.TryGetValue(value, out currentResult))
            {
                Messenger.Default.Send(currentResult);
                Messenger.Default.Send(currentResult.CompressionZone, MessangerTokens.CompressionZoneDrawing);
            }
            StringBuilder detailedResult;
            if (detailedSectionCapacityResults.TryGetValue(value, out detailedResult))
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
