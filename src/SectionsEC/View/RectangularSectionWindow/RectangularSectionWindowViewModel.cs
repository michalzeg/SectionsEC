using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.WindowClasses;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.Sections;

namespace SectionsEC.ViewModel
{
    public class RectangularSectionViewModel : ViewModelBase
    {
        public RectangularSectionViewModel()
        {
            B = 0.5;
            H = 1;
            Cover = 0.02;
            TopBarDiameter = 0.01;
            BottomBarDiameter = 0.016;
            TopBarsNumber = 10;
            BottomBarsNumber = 4;
        }

        private void updateCoordinates()
        {
            var coordinates = RectangularSectionCoordinates.CalculateSectionCoordinates(b, h);
            var bars = RectangularSectionCoordinates.CalculateReinforcementCoordinates(b, h, topBarDiameter, bottomBarDiameter, TopBarsNumber, bottomBarsNumber, cover);

            Messenger.Default.Send<IList<PointD>>(coordinates);
            Messenger.Default.Send<IList<Bar>>(bars);
        }

        private double b;

        public double B
        {
            get { return b; }
            set
            {
                if (value != b)
                {
                    b = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => B);
                }
            }
        }

        private double h;

        public double H
        {
            get { return h; }
            set
            {
                if (value != h)
                {
                    h = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => H);
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

        private double topBarDiameter;

        public double TopBarDiameter
        {
            get { return topBarDiameter; }
            set
            {
                if (value != topBarDiameter)
                {
                    topBarDiameter = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => TopBarDiameter);
                }
            }
        }

        private double bottomBarDiameter;

        public double BottomBarDiameter
        {
            get { return bottomBarDiameter; }
            set
            {
                if (value != bottomBarDiameter)
                {
                    bottomBarDiameter = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => BottomBarDiameter);
                }
            }
        }

        private long topBarsNumber;

        public long TopBarsNumber
        {
            get { return topBarsNumber; }
            set
            {
                if (value != topBarsNumber)
                {
                    topBarsNumber = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => TopBarsNumber);
                }
            }
        }

        private long bottomBarsNumber;

        public long BottomBarsNumber
        {
            get { return bottomBarsNumber; }
            set
            {
                if (value != bottomBarsNumber)
                {
                    bottomBarsNumber = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => BottomBarsNumber);
                }
            }
        }
    }
}