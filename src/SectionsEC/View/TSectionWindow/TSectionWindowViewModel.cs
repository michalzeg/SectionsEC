using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.WindowClasses;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Common.Sections;
using SectionsEC.Common.Geometry;

namespace SectionsEC.ViewModel
{
    public class TSectionViewModel : ViewModelBase
    {
        public TSectionViewModel()
        {
            Bf = 1;
            Bw = 0.3;
            Hf = 0.2;
            Hw = 1;
            Cover = 0.02;
            TopBarDiameter = 0.01;
            BottomBarDiameter = 0.016;
            TopBarsNumber = 10;
            BottomBarsNumber = 4;
        }

        private void updateCoordinates()
        {
            var coordinates = TSectionCoordinates.CalculateSectionCoordinates(bf, bw, hf, hw);
            var bars = TSectionCoordinates.CalculateReinforcementCoordinates(bf, bw, hf, hw,
                topBarDiameter, bottomBarDiameter, topBarsNumber, bottomBarsNumber, cover);

            Messenger.Default.Send<IList<PointD>>(coordinates);
            Messenger.Default.Send<IList<Bar>>(bars);
        }

        private double bf;

        public double Bf
        {
            get { return bf; }
            set
            {
                if (value != bf)
                {
                    bf = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => Bf);
                }
            }
        }

        private double bw;

        public double Bw
        {
            get { return bw; }
            set
            {
                if (value != bw)
                {
                    bw = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => Bw);
                }
            }
        }

        private double hf;

        public double Hf
        {
            get { return hf; }
            set
            {
                if (value != hf)
                {
                    hf = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => Hf);
                }
            }
        }

        private double hw;

        public double Hw
        {
            get { return hw; }
            set
            {
                if (value != hw)
                {
                    hw = value;
                    updateCoordinates();
                    RaisePropertyChanged(() => Hw);
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