using GalaSoft.MvvmLight;
using SectionsEC.Calculations.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.ViewModel

{
    public class ConcretePageViewModel : ViewModelBase
    {
        public ConcreteViewModel ConcreteVM { get; private set; }

        public ConcretePageViewModel()
        {
            ConcreteVM = new ConcreteViewModel();
            ConcreteVM.ConcreteUpdated += () => UpdateConcrete(ConcreteVM.Concrete);
        }

        public delegate void UpdatingConcreteEventHandler(Concrete concrete);

        public event UpdatingConcreteEventHandler UpdateConcrete;

        private Concrete selectedMaterial;

        public Concrete SelectedMaterial
        {
            get
            {
                return ConcreteVM.Concrete;
            }
            set
            {
                if (value != selectedMaterial)
                {
                    selectedMaterial = value;
                    ConcreteVM.Concrete = value;

                    UpdateConcrete(SelectedMaterial);
                }
            }
        }
    }
}