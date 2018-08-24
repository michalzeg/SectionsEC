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
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.Common.LoadCases;
using SectionsEC.ViewModel;

namespace SectionsEC.Views
{
    /// <summary>
    /// Interaction logic for LoadCasesWindow.xaml
    /// </summary>
    public partial class LoadCasesWindow : Window
    {
        public LoadCasesWindow()
        {
            InitializeComponent();
            this.DataContext = new LoadCaseWindowViewModel();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var loadCaseList = ((IEnumerable<LoadCase>)this.dataLoadCases.ItemsSource).ToList();
            Messenger.Default.Send<IList<LoadCase>>(loadCaseList);
        }
    }
}