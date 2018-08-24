using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.WindowClasses;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Helpers;

namespace SectionsEC.ViewModel
{
    public class CircularSectionViewModel : ViewModelBase
    {
        public CircularSectionViewModel()
        {
            SectionDiameter = 1;
            NumberOfBars = 10;
            Cover = 20d / 1000;
            BarDiameter = 20d / 1000;
        }

        private void updateCoordinates()
        {
            var coordinates = CircularSectionCoordinates.CalculateSectionCoordinates(SectionDiameter, Cover);
            var bars = CircularSectionCoordinates.CalculateReinforcementCoordinates(SectionDiameter, Cover, BarDiameter, NumberOfBars);

            Messenger.Default.Send(coordinates);
            Messenger.Default.Send(bars);
        }

        private double sectionDiameter;

        public double SectionDiameter
        {
            get { return sectionDiameter; }
            set
            {
                if (value != sectionDiameter)
                {
                    sectionDiameter = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => SectionDiameter);
                }
            }
        }

        private double cover;

        public double Cover
        {
            get { return cover; }
            set
            {
                if (value != cover)
                {
                    cover = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => Cover);
                }
            }
        }

        private double barDiameter;

        public double BarDiameter
        {
            get { return barDiameter; }
            set
            {
                if (value != barDiameter)
                {
                    barDiameter = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => BarDiameter);
                }
            }
        }

        private long numberOfBars;

        public long NumberOfBars
        {
            get { return numberOfBars; }
            set
            {
                if (value != numberOfBars)
                {
                    numberOfBars = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => NumberOfBars);
                }
            }
        }
    }
}