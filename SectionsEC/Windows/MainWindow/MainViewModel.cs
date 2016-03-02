using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SectionsEC.Views;

namespace SectionsEC.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainPanelViewModel MainPanelVM { get; set; }

        
        public MainViewModel()
        {
            this.MainPanelVM = new MainPanelViewModel();

            this.ShowMaterials = new RelayCommand(() =>
            {
                var materialWindow = new MaterialWindow();
                materialWindow.Show();
            });
            this.ShowCustomSection = new RelayCommand(() =>
            {
                var sectionWindow = new CustomWindow();
                sectionWindow.Show();
            });
        }
        public RelayCommand ShowMaterials { get; private set; }
        public RelayCommand ShowCustomSection { get; private set; }

    }
}