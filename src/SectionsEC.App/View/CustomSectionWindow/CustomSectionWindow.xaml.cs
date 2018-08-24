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
using SectionsEC.Drawing;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.ViewModel;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.Sections;

namespace SectionsEC.Views
{
    /// <summary>
    /// Interaction logic for CustomWindow.xaml
    /// </summary>
    public partial class CustomWindow : Window
    {
        public CustomWindow()
        {
            InitializeComponent();
            this.DataContext = new CustomSectionWindowViewModel();
        }

        private void DataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var sectionCoordinates = ((IEnumerable<PointD>)this.dataGridPoints.ItemsSource).ToList();
            Messenger.Default.Send<IList<PointD>>(sectionCoordinates);

            var barData = ((IEnumerable<Bar>)this.dataGridBars.ItemsSource).ToList();
            Messenger.Default.Send<IList<Bar>>(barData);
        }
    }
}