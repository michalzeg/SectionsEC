﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Calculations.Materials;
using SectionsEC.Calculations.StressFunctions;

namespace SectionsEC.ViewModel
{
    public class MaterialWindowViewModel : ViewModelBase
    {
        public MaterialWindowViewModel()
        {
            this.ConcretePageVM = new ConcretePageViewModel();
            this.ConcreteChartVM = new ChartPageViewModel();
            this.ConcretePageVM.UpdateConcrete += this.updateConcrete;

            this.SteelPageVM = new SteelPageViewModel();
            this.SteelChartVM = new ChartPageViewModel();
            this.SteelPageVM.UpdateSteel += this.updateSteel;
        }

        private Concrete concrete;
        private Steel steel;

        public ConcretePageViewModel ConcretePageVM { get; private set; }
        public ChartPageViewModel ConcreteChartVM { get; private set; }
        public SteelPageViewModel SteelPageVM { get; private set; }
        public ChartPageViewModel SteelChartVM { get; private set; }

        private void updateConcrete(Concrete concrete)
        {
            this.concrete = concrete;

            Func<double, double> characteristicFunction = (e) => StressFunction.ConcreteStressCharacteristic(e, concrete);
            this.ConcreteChartVM.AddCharacteristicChart("fck", concrete.Ecu2, characteristicFunction);

            Func<double, double> designFunction = (e) => StressFunction.ConcreteStressDesign(e, concrete);
            this.ConcreteChartVM.AddDesignChart("fcd", concrete.Ecu2, designFunction);
        }

        private void updateSteel(Steel steel)
        {
            this.steel = steel;

            Func<double, double> characteristicFunction = (e) => StressFunction.SteelStressCharacteristic(e, steel);
            this.SteelChartVM.AddCharacteristicChart("fyk", steel.Euk, characteristicFunction);

            Func<double, double> designFunction = (e) => StressFunction.SteelStressDesign(e, steel);
            this.SteelChartVM.AddDesignChart("fyd", steel.Eud, designFunction);
        }

        public void SendData()
        {
            Messenger.Default.Send<Concrete>(this.concrete);
            Messenger.Default.Send<Steel>(this.steel);
        }
    }
}