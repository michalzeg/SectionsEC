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
using SectionsEC.Materials;

namespace SectionsEC.Views
{
    /// <summary>
    /// Interaction logic for ConcretePage.xaml
    /// </summary>
    public partial class ConcretePage : UserControl
    {
        public ConcretePage()
        {
            InitializeComponent();

            var concreteList = MaterialOperations.GetMaterials().Concrete;
            this.concreteGradeComboBox.ItemsSource = concreteList;
            this.concreteGradeComboBox.SelectedIndex = 0;
        }
    }
}
