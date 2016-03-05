using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.ViewModel
{
    public class RightPanelViewModel :ViewModelBase
    {
        public RightPanelViewModel()
        {

        }

        public string Capacity { get; private set; }
        public string DetailedResults { get; private set; }
    }
}
