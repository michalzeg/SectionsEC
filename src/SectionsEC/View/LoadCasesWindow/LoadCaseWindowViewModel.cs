using GalaSoft.MvvmLight;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.ViewModel
{
    public class LoadCaseWindowViewModel : ViewModelBase
    {
        public LoadCaseWindowViewModel()
        {
            this.LoadCases = new ObservableCollection<LoadCase>();
        }

        public ObservableCollection<LoadCase> LoadCases { get; set; }
    }
}