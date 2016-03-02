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
using SectionsEC.Helpers;
using GalaSoft.MvvmLight.Messaging;
using SectionsEC.ViewModel;

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
            //Messenger.Default.Register<Grid>(this, createCanvas);
        }

        

        private void DataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var sectionCoordinates = ((IEnumerable<PointD>)this.dataGridPoints.ItemsSource).ToList();
            //if (sectionCoordinates.Count!=0)
            //sectionDrawing.Perimeter(sectionCoordinates);
            Messenger.Default.Send<IList<PointD>>(sectionCoordinates);

            var barData = ((IEnumerable<Bar>)this.dataGridBars.ItemsSource).ToList();
            //if (barData.Count != 0)
            //barsDrawing.Bars(barData);
            Messenger.Default.Send<IList<Bar>>(barData);

        }

        
    }
}
