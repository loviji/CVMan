using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
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

namespace hydrogen.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Twister.xaml
    /// </summary>


    public partial class Twister : UserControl, IContent
    {

       

        private string passedDigit { get; set; }
        public Twister()
        { 
        //{
        //    if (NavigationContext.QueryString.TryGetValue("parameter", out parameter))
        //    {
        //        tbParam.Text = parameter;
        //    }
            InitializeComponent();
        }

       

       

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            tbParam.Text = e.Fragment;
            //DoYourStuff(e.Fragment);
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
           // throw new NotImplementedException();
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
           // tbParam.Text = "s";
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            
        }
    }

}