using GalaSoft.MvvmLight;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.ViewModel
{
    public class SteelPageViewModel : ViewModelBase
    {
        public SteelViewModel SteelVM { get; private set; }

        public SteelPageViewModel()
        {
            this.SteelVM = new SteelViewModel();
            SteelVM.SteelUpdated += () => UpdateSteel(SteelVM.Steel);
        }

        public delegate void UpdatingEventHandler(Steel steel);

        public event UpdatingEventHandler UpdateSteel;

        private Steel selectedMaterial;

        public Steel SelectedMaterial
        {
            get
            {
                return SteelVM.Steel;
            }
            set
            {
                if (value != selectedMaterial)
                {
                    selectedMaterial = value;
                    SteelVM.Steel = value;

                    UpdateSteel(SelectedMaterial);
                }
            }
        }
    }
}