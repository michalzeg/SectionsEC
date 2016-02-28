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
    public class CustomSectionWindowViewModel : ViewModelBase
    {
        public CustomSectionWindowViewModel()
        {
            this.Points = new ObservableCollection<PointD>();
        }

        public ObservableCollection<PointD> Points { get; set; }
    }
}
