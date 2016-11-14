﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.Extensions;
using SectionsEC.Dimensioning;

namespace SectionsEC.ViewModel
{
    public class SectionCapacityResultViewModel :ViewModelBase
    {
        public SectionCapacityResultViewModel()
        {
            Messenger.Default.Register<CalculationResults>(this,updateResult);
            Messenger.Default.Register<DetailedResult>(this, updateDetailedResult);
        }

        public string Capacity { get; private set; }

        public string DetailedResults { get; private set; }

        private void updateResult(CalculationResults result)
        {
            Capacity = result.Mrd.ToFormatedString();
            RaisePropertyChanged(() => Capacity);
        }
        private void updateDetailedResult(DetailedResult detailedResult)
        {
            this.DetailedResults = detailedResult.Text.ToString();
            RaisePropertyChanged(() => DetailedResults);
        }
        
    }
}
