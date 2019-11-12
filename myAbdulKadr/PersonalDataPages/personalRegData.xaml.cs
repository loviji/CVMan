using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using myAbdulKadr.Model;
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

namespace myAbdulKadr.PersonalDataPages
{
    /// <summary>
    /// Interaction logic for personalRegData.xaml
    /// </summary>
    public partial class personalRegData : UserControl, IContent
    {
        public personalRegData()
        {
            InitializeComponent();
        }
        private static peopleEntities dbContext = new peopleEntities();
        private int selectedPersonID { get; set; }

        private void SaveNewPosition(object sender, RoutedEventArgs e)
        {
            
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
           
            

            selectedPersonID = Convert.ToInt32(e.Fragment);
            if (selectedPersonID != 0)
            {
                populateFormControls(selectedPersonID);
            }
        }

        private void populateFormControls(int selectedPersonID)
        {
            var addr = dbContext.address.Where(d => d.empID == selectedPersonID).SingleOrDefault();
            if (addr != null)
            {
                RegCountry.Text = addr.countryName;
                RegDistrict.Text = addr.cityName;
                RegAddr.Text = addr.address1;
            }
            var emp = dbContext.employee.SingleOrDefault(s => s.ID == selectedPersonID);
            if(emp!=null)
            {
                pincode.Text = emp.FINCODE;
            }
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
