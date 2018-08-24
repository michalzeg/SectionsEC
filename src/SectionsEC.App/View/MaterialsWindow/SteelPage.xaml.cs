using SectionsEC.Calculations.Materials;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SectionsEC.Views
{
    /// <summary>
    /// Interaction logic for SteelPage.xaml
    /// </summary>
    public partial class SteelPage : UserControl
    {
        public SteelPage()
        {
            InitializeComponent();
            steelGradeComboBox.ItemsSource = MaterialProvider.GetSteel();
            steelGradeComboBox.SelectedIndex = 0;
        }
    }
}