using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.Views;
using SectionsEC.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using SectionsEC.Helpers;
using SectionsEC.Dimensioning;

namespace SectionsEC.Commands
{
    [Obsolete]
    public static class RibbonCommands
    {
        public static void New () {}
        public static void Close () {}

        public static void ShowMaterials ()
        {
            var materialWindow = new MaterialWindow();
            materialWindow.ShowDialog();
            materialWindow.DataContext = SimpleIoc.Default.GetInstance<MaterialWindowViewModel>();
        }
        public static void ShowCustomSection () {}
        public static void ShowCircularSection () {}
        public static void ShowTSection () {}
        public static void ShowLoadCases ()
        {
            var loadCasesWindow = new LoadCasesWindow();
            loadCasesWindow.ShowDialog();
            loadCasesWindow.DataContext = SimpleIoc.Default.GetInstance<LoadCaseWindowViewModel>();
        }

        public static void Run ()
        {
        }
        public static void InteractionCurve () {}
        public static void Report () {}

    }
}
