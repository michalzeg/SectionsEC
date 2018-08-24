using GalaSoft.MvvmLight;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.Sections;
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
            this.Bars = new ObservableCollection<Bar>();
        }

        public ObservableCollection<PointD> Points { get; set; }
        public ObservableCollection<Bar> Bars { get; set; }
    }
}