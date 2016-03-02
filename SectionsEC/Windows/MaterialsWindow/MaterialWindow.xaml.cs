using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SectionsEC.ViewModel;
using GalaSoft.MvvmLight.Ioc;

namespace SectionsEC.Views
{
    /// <summary>
    /// Interaction logic for MaterialWindow.xaml
    /// </summary>
    public partial class MaterialWindow : Window
    {
        private MaterialWindowViewModel vm;
        public MaterialWindow()
        {
            InitializeComponent();
            //vm = SimpleIoc.Default.GetInstance<MaterialWindowViewModel>();
            vm = new MaterialWindowViewModel();
            this.DataContext = vm;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            vm.SendData();
        }
    }
}
