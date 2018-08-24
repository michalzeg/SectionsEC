using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Views;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using SectionsEC.Dimensioning;
using System.Text;
using Xceed.Wpf.Toolkit;
using SectionsEC.WindowClasses;
using System.Threading.Tasks;
using System;
using SectionsEC.Calculations.Progress;
using SectionsEC.Calculations.LoadCases;
using SectionsEC.Calculations.Materials;
using SectionsEC.Calculations.Sections;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Dimensioning.Dimensioning;
using SectionsEC.Dimensioning.Integration;
using SectionsEC.View.Validators;

namespace SectionsEC.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainPanelViewModel MainPanelVM { get; set; }

        public MainWindowViewModel()
        {
            this.MainPanelVM = new MainPanelViewModel();

            this.progressIndicator = new Progress<ProgressArgument>(updateProgress);

            New = new RelayCommand(this.NewCommand);
            Close = new RelayCommand(CloseCommand);

            ShowMaterials = new RelayCommand(ShowMaterialsCommand);
            ShowCustomSection = new RelayCommand(ShowCustomSectionCommand);
            ShowCircularSection = new RelayCommand(ShowCircularSectionCommand);
            ShowRectangularSection = new RelayCommand(ShowRectangularSectionCommand);
            ShowTSection = new RelayCommand(ShowTSectionCommand);
            ShowLoadCases = new RelayCommand(ShowLoadCasesCommand);

            Run = new RelayCommand(RunCommand);
            InteractionCurve = new RelayCommand(InteractionCurveCommand);

            Messenger.Default.Register<Concrete>(this, c => concrete = c);
            Messenger.Default.Register<Steel>(this, s => steel = s);
            Messenger.Default.Register<IList<LoadCase>>(this, l => loadCases = l);
            Messenger.Default.Register<IList<PointD>>(this, p => sectionCoordinates = p);
            Messenger.Default.Register<IList<Bar>>(this, b => bars = b);
        }

        public RelayCommand New { get; private set; }
        public RelayCommand Close { get; private set; }

        public RelayCommand ShowMaterials { get; private set; }
        public RelayCommand ShowCustomSection { get; private set; }
        public RelayCommand ShowCircularSection { get; private set; }
        public RelayCommand ShowRectangularSection { get; private set; }
        public RelayCommand ShowTSection { get; private set; }
        public RelayCommand ShowLoadCases { get; private set; }

        public RelayCommand Run { get; private set; }
        public RelayCommand InteractionCurve { get; private set; }

        private void NewCommand()
        {
        }

        private void CloseCommand()
        {
        }

        private void ShowMaterialsCommand()
        {
            var materialWindow = new MaterialWindow();
            materialWindow.ShowDialog();
            materialWindow.DataContext = SimpleIoc.Default.GetInstance<MaterialWindowViewModel>();
        }

        private void ShowCustomSectionCommand()
        {
            var customSectionWindow = new CustomWindow();
            customSectionWindow.DataContext = SimpleIoc.Default.GetInstance<CustomSectionWindowViewModel>();
            customSectionWindow.Show();
        }

        private void ShowCircularSectionCommand()
        {
            var circularSectionWindow = new CircularSectionWindow();
            circularSectionWindow.Show();
            circularSectionWindow.DataContext = SimpleIoc.Default.GetInstance<CircularSectionViewModel>();
        }

        private void ShowRectangularSectionCommand()
        {
            var rectangularSectionWindow = new RectangularSectionWindow();
            rectangularSectionWindow.Show();
            rectangularSectionWindow.DataContext = SimpleIoc.Default.GetInstance<RectangularSectionViewModel>();
        }

        private void ShowTSectionCommand()
        {
            var tSectionWindow = new TSectionWindow();
            tSectionWindow.Show();
            tSectionWindow.DataContext = SimpleIoc.Default.GetInstance<TSectionViewModel>();
        }

        private void ShowLoadCasesCommand()
        {
            var loadCasesWindow = new LoadCasesWindow();
            loadCasesWindow.ShowDialog();
            loadCasesWindow.DataContext = SimpleIoc.Default.GetInstance<LoadCaseWindowViewModel>();
        }

        private async void RunCommand()
        {
            if (ValidateData())
            {
                Busy = true;
                var capacityResults = await Task.Run(() => CapacityCalculator.GetSectionCapacity(concrete, steel, sectionCoordinates, bars, loadCases, progressIndicator));
                var detailedResults = CapacityCalculator.GetDetailedResults(concrete, steel, capacityResults);

                Messenger.Default.Send(ResultViewModelMessage.SectionCapacityViewModel);
                Messenger.Default.Send(capacityResults);
                Messenger.Default.Send(detailedResults);
                Busy = false;
            }
        }

        private async void InteractionCurveCommand()
        {
            if (ValidateData())
            {
                Busy = true;
                var curve = new InteractionCurveCalculator(concrete, steel, bars, this.sectionCoordinates, loadCases);
                var interactionResult = await Task.Run(() => curve.GetInteractionCurve(progressIndicator));
                Messenger.Default.Send(ResultViewModelMessage.InteractionCurveViewModel);
                Messenger.Default.Send(interactionResult);
                Busy = false;
            }
        }

        private bool ValidateData()
        {
            var validationErrors = Validators.Validate(concrete, steel, loadCases, bars, sectionCoordinates);
            if (validationErrors == string.Empty)
                return true;
            else
            {
                MessageBox.Show(validationErrors, "Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        private IProgress<ProgressArgument> progressIndicator;
        private int progress;

        public int Progress
        {
            get { return progress; }
            set
            {
                if (value != progress)
                {
                    progress = value;
                    RaisePropertyChanged(() => Progress);
                }
            }
        }

        private string currentLoadCase;

        public string CurrentLoadCase
        {
            get { return currentLoadCase; }
            set
            {
                if (value != currentLoadCase)
                {
                    currentLoadCase = value;
                    RaisePropertyChanged(() => CurrentLoadCase);
                }
            }
        }

        private bool busy;

        public bool Busy
        {
            get { return busy; }
            set
            {
                if (value != busy)
                {
                    busy = value;
                    RaisePropertyChanged(() => Busy);
                }
            }
        }

        private void updateProgress(ProgressArgument argument)
        {
            Progress = argument.Progress;
            CurrentLoadCase = argument.LoadCaseName;
        }

        private Concrete concrete;

        private Steel steel;
        private IList<LoadCase> loadCases;
        private IList<PointD> sectionCoordinates;
        private IList<Bar> bars;
    }
}