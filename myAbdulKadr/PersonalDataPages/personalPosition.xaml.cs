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

namespace hydrogen.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personalPosition.xaml
    /// </summary>
    public partial class personalPosition : UserControl, IContent
    {
        private string selectedID { get; set; }

        public personalPosition()
        {
            InitializeComponent();
        }

        void IContent.OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            selectedID = e.Fragment;
            //txtIDSaver.Text = selectedID;
         
        }

        void IContent.OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
           // throw new NotImplementedException();
        }

        void IContent.OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        void IContent.OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
           // throw new NotImplementedException();
        }
    }
}
